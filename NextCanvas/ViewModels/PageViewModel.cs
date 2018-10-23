using System.Windows.Ink;
using NextCanvas.Models;
using NextCanvas.Models.Content;
using NextCanvas.Utilities.Content;
using NextCanvas.ViewModels.Content;

namespace NextCanvas.ViewModels
{
    public class PageViewModel : ViewModelBase<Page>
    {
        private IResourceViewModelLocator locator;

        internal IResourceViewModelLocator Locator
        {
            get => locator;
            set
            {
                if (value == null)
                {
                    return;
                }
                locator = value;
                if (Elements != null)
                {
                    SetLocatorForCollection();
                }
            }
        }

        public StrokeCollection Strokes
        {
            get => Model.Strokes;
            set
            {
                Model.Strokes.Clear();
                foreach (Stroke item in value)
                {
                    Model.Strokes.Add(item);
                }
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

        public PageViewModel()
        {
            Initialize();
        }

        public PageViewModel(Page model) : base(model)
        {
            Initialize();
        }

        internal PageViewModel(Page model, IResourceViewModelLocator resourceLocator) : base(model)
        {
            Locator = resourceLocator;
            Initialize();
        }

        private void UseLocator(ContentElementViewModel vm)
        {
            if (vm is ResourceElementViewModel resourceElement)
            {
                resourceElement.Resource = locator.GetResourceViewModelDataFor(resourceElement.Resource);
                    // Get the deeta
            }
        }

        private void Initialize()
        {
            if (Locator != null)
            {
                Elements = new ObservableViewModelCollection<ContentElementViewModel, ContentElement>(
                    Model.Elements,
                    e => ContentElementViewModel.GetViewModel(e, Locator)); // With a locator.
            }
            else
            {
                Elements = new ObservableViewModelCollection<ContentElementViewModel, ContentElement>(
                    Model.Elements,
                    ContentElementViewModel.GetViewModel); // With a locator.
            }
            if (locator != null)
            {
                SetLocatorForCollection();
            }
        }

        private void SetLocatorForCollection()
        {
            Elements.ItemAdded = UseLocator;
        }
    }
}