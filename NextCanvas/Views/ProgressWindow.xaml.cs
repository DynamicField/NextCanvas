#region

using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
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
            PropertyChangedObject.RegisterWindow(this);
        }

        public override void CloseInteraction()
        {
            Dispatcher.BeginInvoke((Action)(async () =>
            {
                RefreshUI();
                await Task.Delay(100);
                Close();
            }));
        }

        public IProgressData Data { get; } = new ProgressDataContext();
    }
}