using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using NextCanvas.Interactivity;
using NextCanvas.Interactivity.Progress;

namespace NextCanvas.Views
{
    /// <summary>
    ///     Logique d'interaction pour ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window, IProgressInteraction
    {
        // Using a DependencyProperty as the backing store for IsClosed.  This enables animation, styling, binding, etc...
        private static readonly DependencyPropertyKey IsClosedPropertyInternal =
            DependencyProperty.RegisterReadOnly("IsClosed", typeof(bool), typeof(ProgressWindow),
                new FrameworkPropertyMetadata(false));

        public ProgressWindow(Window owner = null)
        {
            InitializeComponent();
            if (owner != null)
            {
                Owner = owner;
            }
            DataContext = Data;
            Data.PropertyChanged += ProgressWindow_PropertyChanged;
            IsClosed = false;
        }

        private void ProgressWindow_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle); // Wait for UI to refresh.
        }

        public static DependencyProperty IsClosedProperty => IsClosedPropertyInternal.DependencyProperty;

        Task IInteractionBase.CloseAsync()
        {
            return Dispatcher.InvokeAsync(async () =>
            {
                Data.Progress = 100;
                await Task.Delay(200);
                Close();
            }).Task;
        }

        Task IInteractionBase.ShowAsync()
        {
            return Dispatcher.InvokeAsync(Show).Task;
        }

        public bool IsClosed
        {
            get => (bool) GetValue(IsClosedPropertyInternal.DependencyProperty);
            private set => SetValue(IsClosedPropertyInternal, value);
        }

        protected override void OnClosed(EventArgs e)
        {
            IsClosed = true;
            base.OnClosed(e);
        }

        public IProgressData Data { get; } = new ProgressDataContext();
    }
}