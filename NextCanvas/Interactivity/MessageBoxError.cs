using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NextCanvas.Interactivity
{
    public class MessageBoxError : IErrorInteraction
    {
        public void ShowInteraction()
        {
            Task.Run(ShowMessageBox);
        }

        public void CloseInteraction()
        {
            throw new InvalidOperationException();
        }

        public MessageBoxError()
        {

        }

        public MessageBoxError(Window owner)
        {
            this.owner = owner;
        }

        private async Task ShowMessageBox()
        {
            if (owner != null)
            {
                await owner.Dispatcher.InvokeAsync(() => MessageBox.Show(owner, Content, Title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK,
                    MessageBoxOptions.None));
            }
            else
            {
                MessageBox.Show(Content, Title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK,
                    MessageBoxOptions.None);
            }
        }

        private readonly Window owner;
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
