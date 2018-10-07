using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NextCanvas.Views
{
    /// <summary>
    /// Logique d'interaction pour StylusDebugWindow.xaml
    /// </summary>
    public partial class StylusDebugWindow : Window
    {
        public StylusDebugWindow()
        {
            InitializeComponent();
            this.StylusMove += StylusDebugWindow_StylusMove;
            StylusButtonDown += StylusDebugWindow_StylusButtonDown;
            StylusOutOfRange += StylusDebugWindow_StylusOutOfRange;
            StylusInRange += StylusDebugWindow_StylusInRange;
        }

        private void StylusDebugWindow_StylusInRange(object sender, StylusEventArgs e)
        {
            eventsBox.Text = "Oh hi stylus !";
        }

        private void StylusDebugWindow_StylusOutOfRange(object sender, StylusEventArgs e)
        {
            eventsBox.Text = "Bye bye stylus :'(";
        }

        private void StylusDebugWindow_StylusButtonDown(object sender, StylusButtonEventArgs e)
        {
            var str = "";
            str += "Button : " + e.StylusButton.Name;
            str += " GUID : " + e.StylusButton.Guid;
            eventsBox.Text = str;
        }

        private void StylusDebugWindow_StylusMove(object sender, StylusEventArgs e)
        {
            var str = "";
            str += $"Is in air : {e.InAir}";
            var point = e.GetStylusPoints(this).FirstOrDefault();
            if (point != null)
            {
                str += "Oh PRESSURE !!!! : " + point.PressureFactor.ToString();
            }
            mainInfo.Text = str;
        }
    }
}
