#region

using System.Collections;
using System.Linq;
using System.Windows;
using NextCanvas.Interactivity;
using NextCanvas.Interactivity.Dialogs;
using NextCanvas;
using NextCanvas.Properties;

#endregion

namespace NextCanvas.ViewModels
{
    public class PageCollectionViewerViewModel : PropertyChangedObject
    {
        public PageCollectionViewerViewModel(MainWindowViewModel viewModel)
        {
            WindowViewModel = viewModel;
            DeletePagesCommand = new DelegateCommand(DeletePages, CanDeletePages);
            WindowViewModel.CurrentDocument.Pages.CollectionChanged += PagesChanged;
        }

        private void PagesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DeletePagesCommand.RaiseCanExecuteChanged();
        }

        public PageCollectionViewerViewModel() : this(new MainWindowViewModel()) { }
        public MainWindowViewModel WindowViewModel { get; }
        public DelegateCommand DeletePagesCommand { get; private set; }

        private void DeletePages(object param)
        {
            if (!CanDeletePages(param)) return;
            var list = ((IList)param).Cast<PageViewModel>().ToList();
            var dialog =  DialogProvider.CreateInteraction();
            dialog.Content = DialogResources.PageCollectionViewer_DeleteDialogContent;
            dialog.Title = DialogResources.PageCollectionViewer_DeleteDialogTitle;
            dialog.ActionComplete += (sender, args) =>
            {
                if (args.ChosenButtonText != "Yes") return;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var page in list)
                    {
                        if (WindowViewModel.CurrentDocument.CanDeletePage)
                        {
                            WindowViewModel.CurrentDocument.Pages.Remove(page);
                        }
                    }
                });
            };
            dialog.ShowInteraction();
        }

        private bool CanDeletePages(object param)
        {
            var preList = param as IList;
            var list = preList?.Cast<PageViewModel>();
            return WindowViewModel.CurrentDocument.CanDeletePage && (list?.Any() ?? false);
        }
        private double wantedWidth = 225;

        public double WantedWidth
        {
            get => wantedWidth;
            set { wantedWidth = value; OnPropertyChanged(nameof(WantedWidth)); }
        }

        private double wantedHeight = 175;

        public double WantedHeight
        {
            get => wantedHeight;
            set { wantedHeight = value; OnPropertyChanged(nameof(WantedHeight)); }
        }
        
        public IInteractionProvider<IUserRequestInteraction> DialogProvider { get; set; }
        // Designer things
        internal static PageCollectionViewerViewModel SampleViewModel { get; } = new PageCollectionViewerViewModel(new MainWindowViewModel
        {
            CurrentDocument = new DocumentViewModel(new Document
            {
                Pages =
                {
                    new Page(),
                    new Page(),
                    new Page()
                }
            })
        });
    }
}
