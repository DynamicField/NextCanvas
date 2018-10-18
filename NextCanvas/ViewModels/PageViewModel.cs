using NextCanvas.Models;
using NextCanvas.Models.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

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
            Elements = new ObservableViewModelCollection<IViewModel<ContentElement>, ContentElement>(Model.Elements, (e) => e.GetAssociatedViewModel());
        }
        public StrokeCollection Strokes
        {
            get => Model.Strokes;
            set
            {
                Model.Strokes.Clear();
                foreach (Stroke item in Strokes)
                {
                    Model.Strokes.Add(item);
                }
            }
        }
        public ObservableViewModelCollection<IViewModel<ContentElement>, ContentElement> Elements { get; set; } 
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
