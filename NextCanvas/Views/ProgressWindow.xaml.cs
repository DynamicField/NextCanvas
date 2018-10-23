using System;
using System.Threading.Tasks;
using System.Windows;
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

        public ProgressWindow()
        {
            InitializeComponent();
            DataContext = Data;
            IsClosed = false;
        }       

        public static DependencyProperty IsClosedProperty => IsClosedPropertyInternal.DependencyProperty;

        Task IInteractionBase.Close()
        {
            return Dispatcher.InvokeAsync(async () =>
            {
                await Task.Delay(75);
                Close();
            }).Task;
        }

        Task IInteractionBase.Show()
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