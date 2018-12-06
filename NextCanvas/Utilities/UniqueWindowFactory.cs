#region

using System;
using System.Windows;

#endregion

namespace NextCanvas.Utilities
{
    public class UniqueWindowFactory<T> where T : Window
    {
        private T _window;
        private FrameworkElement _toUnload;
        public bool ShouldCloseOnUnload { get; set; } = true;
        private Func<T> _createWindow;
        public UniqueWindowFactory(Func<T> createWindow)
        {
            this._createWindow = createWindow;
        }
        public UniqueWindowFactory(Func<T> createWindow, FrameworkElement unload, bool closeOnUnload = true) : this(createWindow)
        {
            this._createWindow = createWindow;
            _toUnload = unload;
            ShouldCloseOnUnload = closeOnUnload;
        }
        public T TryShowWindow()
        {
            if (_window == null)
            {
                _window = _createWindow();
                if (_toUnload != null)
                    _toUnload.Unloaded += ToUnloadOnUnloaded;
                _window.Closed += WindowClosed;
                _window.Show();
            }
            else
            {
                _window.Focus();
            }
            return _window;
        }

        private void ToUnloadOnUnloaded(object sender, RoutedEventArgs e)
        {
            if (_window != null && _window.IsLoaded)
            {
                _window.Close();
            }
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            _window = null;
        }
    }
}
