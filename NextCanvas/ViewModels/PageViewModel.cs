using System.Windows.Ink;
using NextCanvas.Models;
using NextCanvas.Models.Content;
using NextCanvas.ViewModels.Content;

namespace NextCanvas.ViewModels
{
    public class PageViewModel : ViewModelBase<Page>
    {
        public PageViewModel()
        {
            Initialize();
        }

        public PageViewModel(Page model) : base(model)
        {
            Initialize();
        }
        private void Initialize()
        {
            Elements = new ObservableViewModelCollection<ContentElementViewModel, ContentElement>(Model.Elements, e => ContentElementViewModel.GetViewModel(e));
        }
        public StrokeCollection Strokes
        {
            get => Model.Strokes;
            set
            {
                Model.Strokes.Clear();
                foreach (var item in value)
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
    }
}
