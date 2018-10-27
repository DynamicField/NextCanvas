using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using NextCanvas.Interactivity;

namespace NextCanvas
{
    public class InteractionWindow : Window, IInteractionBase
    {
        public virtual Task CloseAsync()
        {
            return Dispatcher.InvokeAsync(Close).Task;
        }

        public virtual Task ShowAsync()
        {
            return Dispatcher.InvokeAsync(Show).Task;
        }

        protected void RefreshUI(DispatcherPriority priority = DispatcherPriority.ContextIdle)
        {
            Dispatcher.Invoke(() => { }, priority);
        }
    }
}