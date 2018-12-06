#region

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using NextCanvas;
using NextCanvas.Content;
using NextCanvas.Serialization;
using NextCanvas.Utilities.Content;
using NextCanvas.Content.ViewModels;

#endregion

namespace NextCanvas.ViewModels
{
    public class DocumentViewModel : ViewModelBase<Document>, IDisposable
    {
        private DocumentResourceLocator _locator;
        private int _selectedIndex;
        public bool CanDeletePage => Pages.Count > 1;
        public const string LogSenderString = "Document";
        public DocumentViewModel(Document model = null) : base(model)
        {
            Initialize();
        }

        public ObservableViewModelCollection<PageViewModel, Page> Pages { get; private set; }
        public ObservableViewModelCollection<ResourceViewModel, Resource> Resources { get; set; }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (value > Pages.Count - 1) throw new IndexOutOfRangeException("ur out of range");
                _selectedIndex = value;
                UpdateSelectedPage();
            }
        }

        private void UpdateSelectedPage()
        {
            if (_selectedIndex >= Pages.Count)
            {
                _selectedIndex = Pages.Count - 1;
            }
            OnPropertyChanged(nameof(SelectedIndex));
            OnPropertyChanged(nameof(SelectedPage));
        }

        public PageViewModel SelectedPage
        {
            get => Pages[_selectedIndex];
            set => SelectedIndex = Pages.IndexOf(value);
        }

        internal IResourceViewModelLocator ResourceLocator => _locator;

        private void Initialize()
        {
            Resources = new ObservableViewModelCollection<ResourceViewModel, Resource>(Model.Resources,
                r => new ResourceViewModel(r));
            _locator = new DocumentResourceLocator(this);
            Pages = new ObservableViewModelCollection<PageViewModel, Page>(Model.Pages,
                GivePageViewModel, SetLocator, PageRemoved); // set locator
            Pages.CollectionChanged += Pages_CollectionChanged;
        }

        private PageViewModel GivePageViewModel(Page m)
        {
            var page = new PageViewModel(m, _locator);
            SetLocator(page);
            return page;
        }

        private void SetLocator(PageViewModel vm)
        {
            vm.Locator = _locator;
            vm.Elements.ItemRemoved += ElementRemoved;
        }
        private void PageRemoved(PageViewModel p)
        {
            CleanupResources();
        }
        private void ElementRemoved(ContentElementViewModel obj)
        {
            if (!(obj is ResourceElementViewModel)) return;
            LogManager.AddLogItem("Resource-using element has been deleted, running a resource cleanup.");
            CleanupResources();
        }
        private void CleanupResources()
        {
            var usedResources = Pages.Select(p => p.Elements)
                .SelectMany(o => o)
                .OfType<ResourceElementViewModel>()
                .Select(e => e.Resource);
            foreach (var resource in Resources.Where(r => usedResources.All(used => used != r)).ToList())
            {
                LogManager.AddLogItem($"Cleaned up unused resource: {resource.Name}, freeing {resource.Data.Length} bytes.");
                resource.Dispose();
                Resources.Remove(resource);
            }
        }
        private void Pages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (SelectedIndex > 0 && e.OldStartingIndex >= SelectedIndex) SelectedIndex = e.OldStartingIndex - 1; // To avoid 3/2 for example
            OnPropertyChanged(nameof(CanDeletePage));
            UpdateSelectedPage();
        }

        public ResourceViewModel AddResource(FileStream stream)
        {
            LogManager.AddLogItem($"Adding resource from file stream: {stream.Name}");
            try
            {
                return AddResource(stream, stream.Name);
            }
            finally
            {
                stream.Dispose();
                LogManager.AddLogItem("Done, file stream disposed.");
            }
        }

        private string CreateResourceName(string name)
        {
            var baseName = Path.GetFileNameWithoutExtension(name);
            var extension = Path.GetExtension(name);
            var fileName = baseName + "_" + _random.Next(0, short.MaxValue) + "_" + Resources.Count + extension;
            return fileName;
        }

        private ResourceViewModel GetExistingResource(Stream fileStream)
        {
            var testResource = Resources.FirstOrDefault(r => r.DataMD5Hash == fileStream.GetMD5FromFile());
            return testResource;
        }
        private Random _random = new Random();
        public ResourceViewModel AddResource(Stream stream, string name)
        {
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return AddResource(memoryStream, name);
        }
        public ResourceViewModel AddResource(MemoryStream stream, string name)
        {
            LogManager.AddLogItem($"Adding resource from stream: {name}");
            var testResource = GetExistingResource(stream);
            if (testResource != null)
            {
                LogManager.AddLogItem($"Resource already found: {testResource.Name} ; hash: {testResource.DataMD5Hash}");
                return testResource;
            }
            var fileName = CreateResourceName(name);
            var resource = new ResourceViewModel(new Resource(fileName, stream));
            Resources.Add(resource);
            LogManager.AddLogItem($"Added new resource: {resource.Name}");
            return resource;
        }

        private class DocumentResourceLocator : IResourceViewModelLocator
        {
            public DocumentResourceLocator(IEnumerable<ResourceViewModel> resources)
            {
                Resources = resources;
            }

            public DocumentResourceLocator(DocumentViewModel document)
            {
                Resources = document.Resources;
            }

            private IEnumerable<ResourceViewModel> Resources { get; }

            public Resource GetResourceDataFor(Resource resource)
            {
                return GetResourceViewModelInternal(resource).Model;
            }

            public ResourceViewModel GetResourceViewModelDataFor(ResourceViewModel resource)
            {
                return GetResourceCheck(resource);
            }

            public ResourceViewModel GetResourceViewModelDataFor(Resource resource)
            {
                return GetResourceCheck(resource);
            }

            public bool HasResourceGotValuableData(Resource dataResource)
            {
                return dataResource.Data != null && Resources.Any(r =>
                           r.Name == dataResource.Name && r.DataMD5Hash == dataResource.DataMD5Hash);
            }

            public bool HasResourceGotValuableData(ResourceViewModel dataResource)
            {
                return HasResourceGotValuableData(dataResource.Model);
            }

            private ResourceViewModel GetResourceViewModelInternal(Resource resource)
            {
                var matchingResource = Resources.FirstOrDefault(r => r.Name == resource.Name && r.Data != null);
                if (matchingResource == null) throw new InstanceNotFoundException();
                return matchingResource;
            }

            private ResourceViewModel GetResourceCheck(ResourceViewModel resource)
            {
                if (HasResourceGotValuableData(resource)) return resource; // Give the resource back.
                try
                {
                    return GetResourceViewModelInternal(resource.Model);
                }
                catch (InstanceNotFoundException)
                {
                    return null;
                }
            }

            private ResourceViewModel GetResourceCheck(Resource resource)
            {
                if (HasResourceGotValuableData(resource)) return new ResourceViewModel(resource); // Give a vm
                try
                {
                    return GetResourceViewModelInternal(resource);
                }
                catch (InstanceNotFoundException)
                {
                    return null;
                }
            }
        }

        public void Dispose()
        {
            foreach (var resource in Resources)
            {
                resource.Dispose(); // Good bye memory streams.
            }
        }
    }
}