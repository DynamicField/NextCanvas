#region

using System;
using System.Windows;

#endregion

namespace NextCanvas.Utilities
{
    public class UniqueWindowFactory<T> where T : Window
    {
        private T window;
        private FrameworkElement toUnload;
        public bool ShouldCloseOnUnload { get; set; } = true;
        private Func<T> createWindow;
        public UniqueWindowFactory(Func<T> createWindow)
        {
            this.createWindow = createWindow;
        }
        public UniqueWindowFactory(Func<T> createWindow, FrameworkElement unload, bool closeOnUnload = true) : this(createWindow)
        {
            this.createWindow = createWindow;
            toUnload = unload;
            ShouldCloseOnUnload = closeOnUnload;
        }
        public T TryShowWindow()
        {
            if (window == null)
            {
                window = createWindow();
                if (toUnload != null)
                    toUnload.Unloaded += ToUnloadOnUnloaded;
                window.Closed += WindowClosed;
                window.Show();
            }
            else
            {
                window.Focus();
            }
            return window;
        }

        private void ToUnloadOnUnloaded(object sender, RoutedEventArgs e)
        {
            if (window != null && window.IsLoaded)
            {
                window.Close();
            }
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            window = null;
        }
    }
}
