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
        private readonly bool _hideInsteadOfClose;
        private bool _hidden;
        private Func<T> _createWindow;
        public UniqueWindowFactory(Func<T> createWindow, bool hideInsteadOfClose = false)
        {
            this._createWindow = createWindow;
            _hideInsteadOfClose = hideInsteadOfClose;
        }
        public UniqueWindowFactory(Func<T> createWindow, FrameworkElement unload, bool closeOnUnload = true, bool hideInsteadOfClose = false) : this(createWindow, hideInsteadOfClose)
        {
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
                if (!_hideInsteadOfClose)
                {
                    _window.Closed += WindowClosed;
                }
                else
                {
                    _window.Closing += WindowClosingHide;
                }
                _window.Show();
            }
            else if (!_hidden)
            {
                _window.Focus();
            }
            else
            {
                _window.Show();
            }
            return _window;
        }

        private void WindowClosingHide(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            _window.Hide();
            _hidden = true;
        }

        private void ToUnloadOnUnloaded(object sender, RoutedEventArgs e)
        {
            if (_window != null && (_window.IsLoaded || _hidden))
            {
                if (_hideInsteadOfClose)
                {
                    _window.Closing -= WindowClosingHide;
                }
                _window.Close();
            }
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            _window = null;
        }

        public void Close()
        {
            if (_window is null || _window.IsLoaded) return;
            _window.Close();
            _window = null;
        }
    }
}
