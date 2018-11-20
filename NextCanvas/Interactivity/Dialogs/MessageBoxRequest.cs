#region

using System;
using System.Threading.Tasks;
using System.Windows;

#endregion

namespace NextCanvas.Interactivity.Dialogs
{
    public class MessageBoxRequest : IUserRequestInteraction
    {
        public void ShowInteraction()
        {
            Task.Run(ProcessDialog);
        }

        public void CloseInteraction()
        {
            throw new InvalidOperationException();
        }

        private async Task ProcessDialog()
        {
            var result = await owner.Dispatcher.InvokeAsync(() => MessageBox.Show(owner, Content, Title, MessageBoxButton.YesNo, MessageBoxImage.Question,
                MessageBoxResult.No));
            ActionComplete?.Invoke(this, new DialogResultEventArgs(result.ToString(), result == MessageBoxResult.Yes || result == MessageBoxResult.OK));
        }

        public event EventHandler<DialogResultEventArgs> ActionComplete;
        public event EventHandler ActionCanceled;
        public string Title { get; set; }
        public string Content { get; set; }
        private readonly Window owner;

        public MessageBoxRequest(Window owner)
        {
            this.owner = owner;
        }
    }
}
