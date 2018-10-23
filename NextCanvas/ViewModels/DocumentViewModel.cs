using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using NextCanvas.Models;
using NextCanvas.Models.Content;
using NextCanvas.Serialization;
using NextCanvas.Utilities.Content;
using NextCanvas.ViewModels.Content;

namespace NextCanvas.ViewModels
{
    public class DocumentViewModel : ViewModelBase<Document>
    {
        private DocumentResourceLocator locator;
        private int selectedIndex;

        public DocumentViewModel()
        {
            Initialize();
        }

        public DocumentViewModel(Document model) : base(model)
        {
            Initialize();
        }

        public ObservableViewModelCollection<PageViewModel, Page> Pages { get; private set; }
        public ObservableViewModelCollection<ResourceViewModel, Resource> Resources { get; set; }

        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                if (value > Pages.Count - 1) throw new IndexOutOfRangeException("ur out of range");
                selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
                OnPropertyChanged(nameof(SelectedPage));
            }
        }

        public PageViewModel SelectedPage => Pages[SelectedIndex];

        internal IResourceViewModelLocator ResourceLocator => locator;

        private void Initialize()
        {
            Resources = new ObservableViewModelCollection<ResourceViewModel, Resource>(Model.Resources,
                r => new ResourceViewModel(r));
            locator = new DocumentResourceLocator(this);
            Pages = new ObservableViewModelCollection<PageViewModel, Page>(Model.Pages,
                m => new PageViewModel(m, locator), SetLocator); // set locator
            Pages.CollectionChanged += Pages_CollectionChanged;
        }

        private void SetLocator(PageViewModel vm)
        {
            vm.Locator = locator;
        }

        private void Pages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (SelectedIndex > 0 && e.OldStartingIndex >= SelectedIndex) SelectedIndex = e.OldStartingIndex - 1;
        }

        public ResourceViewModel AddResource(FileStream fileStream)
        {
            var testResource = Resources.FirstOrDefault(r => r.DataMD5Hash == fileStream.GetMD5FromFile());
            if (testResource != null) return testResource;
            var baseName = Path.GetFileNameWithoutExtension(fileStream.Name);
            var extension = Path.GetExtension(fileStream.Name);
            var fileName = baseName + Resources.Count + extension;
            var resource = new ResourceViewModel(new Resource(fileName, fileStream));
            Resources.Add(resource);
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
                if (matchingResource == null) throw new ArgumentException("Couldn't find any matching resource.");
                return matchingResource;
            }

            private ResourceViewModel GetResourceCheck(ResourceViewModel resource)
            {
                if (HasResourceGotValuableData(resource)) return resource; // Give the resource back.
                return GetResourceViewModelInternal(resource.Model);
            }

            private ResourceViewModel GetResourceCheck(Resource resource)
            {
                if (HasResourceGotValuableData(resource)) return new ResourceViewModel(resource); // Give a vm
                return GetResourceViewModelInternal(resource);
            }
        }
    }
}