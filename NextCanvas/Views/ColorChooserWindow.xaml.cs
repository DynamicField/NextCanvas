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
using NextCanvas.Interactivity.Dialogs;

namespace NextCanvas.Views
{
    /// <summary>
    /// Logique d'interaction pour ColorChooserWindow.xaml
    /// </summary>
    public partial class ColorChooserWindow : InteractionWindow, IColorRequestInteraction
    {
        private ColorWindowData data;
        public ColorChooserWindow()
        {
            data = new ColorWindowData();
            DataContext = data;
            InitializeComponent();
        }
        private class ColorWindowData : PropertyChangedObject
        {
            public ColorWindowData()
            {

            }
            private double hue;

            public double Hue
            {
                get => hue;
                set { hue = value; OnPropertyChanged();  UpdateColors(); }
            }
            private double saturation = 90;

            public double Saturation
            {
                get => saturation;
                set { saturation = value; OnPropertyChanged(); UpdateColors(); }
            }
            private double lightness = 95;

            public double Lightness
            {
                get => lightness;
                set { lightness = value; OnPropertyChanged(); UpdateColors(); }
            }

            private void UpdateColors()
            {
                OnPropertyChanged(nameof(ResultColor));
                OnPropertyChanged(nameof(FullSaturationColor));
            }

            public Color ResultColor => HsvToRgb(hue * 3.6, saturation / 100, lightness / 100);
            public Color FullSaturationColor => HsvToRgb(hue * 3.6, 1, 1);

            // https://stackoverflow.com/questions/1335426/is-there-a-built-in-c-net-system-api-for-hsv-to-rgb
            private static Color HsvToRgb(double h, double s, double v)
            {
                var d = h;
                while (d < 0) { d += 360; };
                while (d >= 360) { d -= 360; };
                double R, G, B;
                if (v <= 0)
                { R = G = B = 0; }
                else if (s <= 0)
                {
                    R = G = B = v;
                }
                else
                {
                    var hf = d / 60.0;
                    var i = (int)Math.Floor(hf);
                    var f = hf - i;
                    var pv = v * (1 - s);
                    var qv = v * (1 - s * f);
                    var tv = v * (1 - s * (1 - f));
                    switch (i)
                    {

                        // Red is the dominant color

                        case 0:
                            R = v;
                            G = tv;
                            B = pv;
                            break;

                        // Green is the dominant color

                        case 1:
                            R = qv;
                            G = v;
                            B = pv;
                            break;
                        case 2:
                            R = pv;
                            G = v;
                            B = tv;
                            break;

                        // Blue is the dominant color

                        case 3:
                            R = pv;
                            G = qv;
                            B = v;
                            break;
                        case 4:
                            R = tv;
                            G = pv;
                            B = v;
                            break;

                        // Red is the dominant color

                        case 5:
                            R = v;
                            G = pv;
                            B = qv;
                            break;

                        // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                        case 6:
                            R = v;
                            G = tv;
                            B = pv;
                            break;
                        case -1:
                            R = v;
                            G = pv;
                            B = qv;
                            break;

                        // The color is not defined, we should throw an error.

                        default:
                            //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                            R = G = B = v; // Just pretend its black/white
                            break;
                    }
                }
                var r = Clamp((int)(R * 255.0));
                var g = Clamp((int)(G * 255.0));
                var b = Clamp((int)(B * 255.0));
                return Color.FromRgb(r, g, b);
            }

            /// <summary>
            /// Clamp a value to 0-255
            /// </summary>
            private static byte Clamp(int i)
            {
                if (i < 0) return 0;
                if (i > 255) return 255;
                return (byte)i;
            }
        }

        public event EventHandler<ColorRequestEventArgs> ActionComplete;
        public event EventHandler ActionCanceled;

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            ActionComplete?.Invoke(this, new ColorRequestEventArgs(data.ResultColor));
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ActionCanceled?.Invoke(this, EventArgs.Empty);
            Close();
        }
    }
}
