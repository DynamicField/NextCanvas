using System;
using System.Windows;

namespace NextCanvas.Utilities
{
    public class UniqueWindowFactory<T> where T : Window
    {
        private T window;
        private Func<T> createWindow;
        public UniqueWindowFactory(Func<T> createWindow)
        {
            this.createWindow = createWindow;
        }

        public T TryShowWindow()
        {
            if (window == null)
            {
                window = createWindow();
                window.Closed += WindowClosed;
                window.Show();
            }
            else
            {
                window.Focus();
            }
            return window;
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            window = null;
        }
    }
}
