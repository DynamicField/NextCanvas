using System;
using System.Collections.Specialized;
using NextCanvas.Models;

namespace NextCanvas.ViewModels
{
    public class DocumentViewModel : ViewModelBase<Document>
    {
        public ObservableViewModelCollection<PageViewModel, Page> Pages { get; set; }
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

        public DocumentViewModel(Document model) : base(model)
        {
            Initialize();
        }
    }
}
