using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using NextCanvas.Interactivity.Progress;

namespace NextCanvas.Views
{
    /// <summary>
    ///     Logique d'interaction pour ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : InteractionWindow, IProgressInteraction
    {
        public ProgressWindow(Window owner = null)
        {
            InitializeComponent();
            if (owner != null) Owner = owner;
            DataContext = Data;
            Data.PropertyChanged += ProgressWindow_PropertyChanged;
        }

        public override Task CloseAsync()
        {
            return Dispatcher.InvokeAsync(async () =>
            {
                Data.Progress = 100;
                await Task.Delay(200);
                Close();
            }).Task;
        }

        public IProgressData Data { get; } = new ProgressDataContext();

        private void ProgressWindow_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RefreshUI();
        }
    }
}