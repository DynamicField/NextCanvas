#region

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using NextCanvas.Interactivity.Progress;

#endregion

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

        public override void CloseInteraction()
        {
            Dispatcher.BeginInvoke((Action)(async () =>
            {
                Data.Progress = 100;
                await Task.Delay(100);
                Close();
            }));
        }

        public IProgressData Data { get; } = new ProgressDataContext();

        private void ProgressWindow_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RefreshUI();
        }
    }
}