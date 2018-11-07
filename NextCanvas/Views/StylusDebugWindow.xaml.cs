#region

using System.Linq;
using System.Windows;
using System.Windows.Input;

#endregion

namespace NextCanvas.Views
{
    /// <summary>
    ///     Logique d'interaction pour StylusDebugWindow.xaml
    /// </summary>
    public partial class StylusDebugWindow : Window
    {
        public StylusDebugWindow()
        {
            InitializeComponent();
            StylusMove += StylusDebugWindow_StylusMove;
            StylusButtonDown += StylusDebugWindow_StylusButtonDown;
            StylusOutOfRange += StylusDebugWindow_StylusOutOfRange;
            StylusInRange += StylusDebugWindow_StylusInRange;
        }

        private void StylusDebugWindow_StylusInRange(object sender, StylusEventArgs e)
        {
            EventsBox.Text = "Oh hi stylus !";
        }

        private void StylusDebugWindow_StylusOutOfRange(object sender, StylusEventArgs e)
        {
            EventsBox.Text = "Bye bye stylus :'(";
        }

        private void StylusDebugWindow_StylusButtonDown(object sender, StylusButtonEventArgs e)
        {
            var str = "";
            str += "Button : " + e.StylusButton.Name;
            str += " GUID : " + e.StylusButton.Guid;
            EventsBox.Text = str;
        }

        private void StylusDebugWindow_StylusMove(object sender, StylusEventArgs e)
        {
            var str = "";
            str += $"Is in air : {e.InAir}";
            var point = e.GetStylusPoints(this).FirstOrDefault();
            str += "Oh PRESSURE !!!! : " + point.PressureFactor;
            MainInfo.Text = str;
        }
    }
}