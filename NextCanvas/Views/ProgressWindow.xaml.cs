using System;
using System.Windows;
using System.Windows.Threading;
using NextCanvas.Interactivity;

namespace NextCanvas.Views
{
    /// <summary>
    /// Logique d'interaction pour ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window, IProgressInteraction
    {
        public ProgressWindow()
        {
            InitializeComponent();
            IsClosed = false;
        }

        double IProgressInteraction.Progress
        {
            get => Dispatcher.Invoke(() => Progress);
            set => Dispatcher.Invoke(() => Progress = value);
        }
        string IProgressInteraction.ProgressText
        {
            get => Dispatcher.Invoke(() => ProgressText);
            set => Dispatcher.Invoke(() => ProgressText = value);
        }
        public double Progress
        {
            get => (double)GetValue(ProgressProperty);
            set => SetValue(ProgressProperty, value);
        }

        public string ProgressText
        {
            get => (string)GetValue(ProgressTextProperty);
            set => SetValue(ProgressTextProperty, value);
        }
        void IInteractionBase.Close()
        {
            Dispatcher.Invoke(Close);
        }

        void IInteractionBase.Show() => Dispatcher.Invoke(Show);
        // Using a DependencyProperty as the backing store for ProgressText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProgressTextProperty =
            DependencyProperty.Register("ProgressText", typeof(string), typeof(ProgressWindow), new FrameworkPropertyMetadata("Loading..."));

        // Using a DependencyProperty as the backing store for Progress.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProgressProperty =
            DependencyProperty.Register("Progress", typeof(double), typeof(ProgressWindow), new FrameworkPropertyMetadata((double)0));

        public bool IsClosed
        {
            get => (bool)GetValue(IsClosedPropertyInternal.DependencyProperty);
            private set => SetValue(IsClosedPropertyInternal, value);
        }

        // Using a DependencyProperty as the backing store for IsClosed.  This enables animation, styling, binding, etc...
        private static readonly DependencyPropertyKey IsClosedPropertyInternal =
            DependencyProperty.RegisterReadOnly("IsClosed", typeof(bool), typeof(ProgressWindow), new FrameworkPropertyMetadata(false));

        public static DependencyProperty IsClosedProperty => IsClosedPropertyInternal.DependencyProperty;
        protected override void OnClosed(EventArgs e)
        {
            IsClosed = true;
            base.OnClosed(e);
        }
    }
}
