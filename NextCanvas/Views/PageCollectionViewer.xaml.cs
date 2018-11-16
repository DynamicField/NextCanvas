#region

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using NextCanvas.Interactivity;
using NextCanvas.Interactivity.Dialogs;
using NextCanvas.Utilities;
using NextCanvas.ViewModels;

#endregion

namespace NextCanvas.Views
{
    /// <summary>
    /// Logique d'interaction pour PageCollectionViewer.xaml
    /// </summary>
    public partial class PageCollectionViewer : Window
    {
        public PageCollectionViewer() : this(new MainWindowViewModel())
        {
            
        }
        public PageCollectionViewer(MainWindowViewModel viewModel)
        {
            var vm = new PageCollectionViewerViewModel(viewModel)
            {
                DialogProvider = DialogProvider
            };
            DataContext = vm;
            InitializeComponent();
            ListBox.ItemContainerGenerator.ItemsChanged += ItemContainerGenerator_ItemsChanged;
        }

        private void ItemContainerGenerator_ItemsChanged(object sender, System.Windows.Controls.Primitives.ItemsChangedEventArgs e)
        {
            for (var i = 0; i < ListBox.ItemContainerGenerator.Items.Count; i++)
            {
                try
                {
                    var element =
                        (FrameworkElement) ListBox.ItemContainerGenerator.ContainerFromIndex(i);
                    if (element == null)
                        continue;
                    object found;
                    found = VisualTreeUtilities.FindVisualChild<StackPanel>(element).FindName("Number");
                    var textBlock = found as TextBlock;
                    if (textBlock == null)
                        continue;
                    var bindingExpression = BindingOperations.GetMultiBindingExpression(textBlock, TextBlock.TextProperty);
                    bindingExpression?.UpdateTarget();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Oh no " + ex);
                }
            }                          
        }

        public IInteractionProvider<IUserRequestInteraction> DialogProvider => new DelegateInteractionProvider<IUserRequestInteraction>(() => new MessageBoxRequest(this));

        private void ListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DeleteButton.Command as DelegateCommand)?.RaiseCanExecuteChanged(); // It cannot be bound so we have no choice sadly :(
        }

        private void DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vm = (PageCollectionViewerViewModel) DataContext;
            vm.WindowViewModel.CurrentDocument.SelectedPage = ((ListBoxItem)sender).DataContext as PageViewModel;
        }
    }
}
