using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using NextCanvas.Interactivity;

namespace NextCanvas
{
    public class InteractionWindow : Window, IInteractionBase
    {
        public virtual void CloseInteraction()
        {
            Dispatcher.Invoke(Close);
        }

        public virtual void ShowInteraction()
        {
            Dispatcher.Invoke(Show);
        }

        protected void RefreshUI(DispatcherPriority priority = DispatcherPriority.ContextIdle)
        {
            Dispatcher.Invoke(() => { }, priority);
        }
    }
}