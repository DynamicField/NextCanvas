using Fluent;
using NextCanvas.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NextCanvas.ViewModels
{
    public class MainWindowViewModel : ViewModelBase<MainWindowModel>
    {
        private DocumentViewModel document;
        public DocumentViewModel CurrentDocument
        {
            get => document;
            set
            {
                if (document != null)
                {
                    document.Pages.CollectionChanged -= PagesChanged;
                    document.PropertyChanged -= DocumentPropertyChanged;
                }
                document = value;
                Model.Document = document.Model;
                Subscribe();
                OnPropertyChanged(nameof(CurrentDocument));
                UpdatePageText();
            }
        }
        public ObservableCollection<Color> FavoriteColors { get; set; } = new ObservableCollection<Color>
        {
            Colors.Black,
            Colors.Blue,
            Colors.Red,
            Colors.Yellow,
            Colors.Purple
        };
        private void Subscribe() // To my youtube channel XD
        {
            document.Pages.CollectionChanged += PagesChanged;
            document.PropertyChanged += DocumentPropertyChanged;
        }

        private void DocumentPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdatePageText();
            UpdatePageManipulation();
        }

        private void UpdatePageManipulation()
        {
            PreviousPageCommand.RaiseCanExecuteChanged();
            NextPageCommand.RaiseCanExecuteChanged();
            DeletePageCommand.RaiseCanExecuteChanged();
        }

        private void PagesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdatePageText();
        }
        public DelegateCommand PreviousPageCommand { get; private set; }
        public DelegateCommand NextPageCommand { get; private set; }
        public DelegateCommand NewPageCommand { get; private set; }
        public DelegateCommand DeletePageCommand { get; private set; }
        public DelegateCommand ExtendPageCommand { get; private set; }
        public string PageDisplayText => CurrentDocument.SelectedIndex + 1 + "/" + CurrentDocument.Pages.Count;
        public MainWindowViewModel() : base()
        {
            Initalize();
        }
        private void UpdatePageText()
        {
            OnPropertyChanged(nameof(PageDisplayText));
        }
        private void Initalize()
        {
            document = new DocumentViewModel(Model.Document);
            Subscribe();
            PreviousPageCommand = new DelegateCommand(o => ChangePage(Direction.Backwards), o => CanChangePage(Direction.Backwards));
            NextPageCommand = new DelegateCommand(o => ChangePage(Direction.Forwards), o => CanChangePage(Direction.Forwards));
            NewPageCommand = new DelegateCommand(o => CreateNewPage());
            DeletePageCommand = new DelegateCommand(o => DeletePage(CurrentDocument.SelectedIndex), o => CanDeletePage);
            ExtendPageCommand = new DelegateCommand(o => ExtendPage(o.ToString()));
        }
        private void DeletePage(int index)
        {
            if (CanDeletePage)
            {
                CurrentDocument.Pages.RemoveAt(index);
                UpdatePageManipulation();
            }
        }
        private void ExtendPage(string direction)
        {
            if (direction.Equals("Right", StringComparison.InvariantCultureIgnoreCase))
            {
                CurrentDocument.SelectedPage.Width += 350;
            }
            if (direction.Equals("Bottom", StringComparison.InvariantCultureIgnoreCase))
            {
                CurrentDocument.SelectedPage.Height += 350;
            }
        }
        private bool CanDeletePage => CurrentDocument.Pages.Count > 1;
        private void ChangePage(Direction direction)
        {
            if (CanChangePage(direction))
            {
                document.SelectedIndex += (int)direction;
            }
        }
        private void CreateNewPage()
        {
            CurrentDocument.Pages.Add(new PageViewModel());
            ChangePage(Direction.Forwards);
        }
        private bool CanChangePage(Direction direction)
        {
            return (direction == Direction.Forwards && document.SelectedIndex + 1 != document.Pages.Count) ||
                   (direction == Direction.Backwards && document.SelectedIndex - 1 >= 0);
        }
        private enum Direction
        {
            Forwards = 1,
            Backwards = -1
        }
        public MainWindowViewModel(MainWindowModel model) : base(model)
        {
            Initalize();
        }
    }
}
