#region

using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#endregion

namespace NextCanvas.Views
{
    /// <summary>
    /// Logique d'interaction pour SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            SettingsManager.SaveSettings();
            Close();
        }
    }
}
