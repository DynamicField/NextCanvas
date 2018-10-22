using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using NextCanvas.Models;
using NextCanvas.Models.Content;
using NextCanvas.Serialization;
using NextCanvas.ViewModels.Content;

namespace NextCanvas.ViewModels
{
    public class DocumentViewModel : ViewModelBase<Document>
    {
        public ObservableViewModelCollection<PageViewModel, Page> Pages { get; set; }
        public ObservableViewModelCollection<ResourceViewModel, Resource> Resources { get; set; }
        private int selectedIndex;
        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                if (value > Pages.Count - 1)
                {
                    throw new IndexOutOfRangeException("ur out of range");
                }
                selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
                OnPropertyChanged(nameof(SelectedPage));
            }
        }
        public PageViewModel SelectedPage
        {
            get
            {               
                return Pages[SelectedIndex];
            }
        }
        public DocumentViewModel()
        {
            Initialize();
        }

        private void Initialize()
        {
            Resources = new ObservableViewModelCollection<ResourceViewModel, Resource>(Model.Resources,
                r => new ResourceViewModel(r));
            Pages = new ObservableViewModelCollection<PageViewModel, Page>(Model.Pages, m => new PageViewModel(m));
            Pages.CollectionChanged += Pages_CollectionChanged;
        }

        private void Pages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (SelectedIndex > 0 && e.OldStartingIndex >= SelectedIndex)
            {
                SelectedIndex = e.OldStartingIndex - 1;
            }
        }

        public ResourceViewModel AddResource(FileStream fileStream)
        {         
            var testResource = Resources.FirstOrDefault(r => r.DataMD5Hash == fileStream.GetMD5FromFile());
            if (testResource != null)
            {
                return testResource;
            }
            var baseName = Path.GetFileNameWithoutExtension(fileStream.Name);
            var extension = Path.GetExtension(fileStream.Name);
            var fileName = baseName + Resources.Count + extension;
            var resource = new ResourceViewModel(new Resource(fileName, fileStream));
            Resources.Add(resource);
            return resource;
        }
        public DocumentViewModel(Document model) : base(model)
        {
            Initialize();
        }
    }
}
