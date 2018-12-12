#region

using System.Windows.Ink;
using NextCanvas.Content;
using NextCanvas.Content.ViewModels;
using NextCanvas.Utilities.Content;

#endregion

namespace NextCanvas.ViewModels
{
    public class PageViewModel : ViewModelBase<Page>
    {
        private IResourceViewModelLocator _locator;

        public PageViewModel(Page model = null) : base(model)
        {
            Initialize();
        }

        internal PageViewModel(Page model, IResourceViewModelLocator resourceLocator) : base(model)
        {
            Locator = resourceLocator;
            Initialize();
        }

        internal IResourceViewModelLocator Locator
        {
            get => _locator;
            set
            {
                if (value == null) return;
                _locator = value;
                if (Elements != null) SetLocatorForCollection();
            }
        }

        public StrokeCollection Strokes
        {
            get => Model.Strokes;
            set
            {
                Model.Strokes.Clear();
                foreach (var item in value) Model.Strokes.Add(item);
            }
        }

        public ObservableViewModelCollection<ContentElementViewModel, ContentElement> Elements { get; set; }

        public int Width
        {
            get => Model.Width;
            set
            {
                Model.Width = value;
                OnPropertyChanged(nameof(Width));
            }
        }

        public int Height
        {
            get => Model.Height;
            set
            {
                Model.Height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        private void UseLocator(ContentElementViewModel vm)
        {
            if (vm is ResourceElementViewModel resourceElement)
                resourceElement.Resource =
                    _locator.GetResourceViewModelDataFor(resourceElement.Resource); // Get the deeta from the json.
        }

        private void Initialize()
        {
            if (Locator == null)
                Elements = new ObservableViewModelCollection<ContentElementViewModel, ContentElement>(Model.Elements,
                    ContentElementViewModelFinder.GetViewModel); // Without a locator.
            else
            {
                Elements = new ObservableViewModelCollection<ContentElementViewModel, ContentElement>(Model.Elements,
                    e => ContentElementViewModelFinder.GetViewModel(e, Locator)); // With a locator.
            }
            if (_locator != null) SetLocatorForCollection();
        }

        private void SetLocatorForCollection()
        {
            Elements.ItemAdded = UseLocator;
        }
    }
}