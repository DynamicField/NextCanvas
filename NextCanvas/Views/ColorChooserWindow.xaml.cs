using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NextCanvas.Interactivity.Dialogs;
using Color = System.Windows.Media.Color;

namespace NextCanvas.Views
{
    /// <summary>
    /// Logique d'interaction pour ColorChooserWindow.xaml
    /// </summary>
    public partial class ColorChooserWindow : InteractionWindow, IColorRequestInteraction
    {
        private ColorWindowData _data;
        public ColorChooserWindow()
        {
            _data = new ColorWindowData();
            DataContext = _data;
            InitializeComponent();
        }
        private class ColorWindowData : PropertyChangedObject
        {
            public ColorWindowData()
            {

            }
            private double _hue;

            public double Hue
            {
                get => _hue;
                set { _hue = value; OnPropertyChanged();  UpdateColors(); }
            }
            private double _saturation = 90;

            public double Saturation
            {
                get => _saturation;
                set { _saturation = value; OnPropertyChanged(); UpdateColors(); }
            }
            private double _lightness = 95;

            public double Lightness
            {
                get => _lightness;
                set { _lightness = value; OnPropertyChanged(); UpdateColors(); }
            }
            private double _opacity = 1;

            public double Opacity
            {
                get { return _opacity; }
                set { _opacity = value; OnPropertyChanged(); UpdateColors(); }
            }

            private void UpdateColors()
            {
                OnPropertyChanged(nameof(ResultColor));
                OnPropertyChanged(nameof(ResultTextColor));
                OnPropertyChanged(nameof(FullSaturationColor));
            }

            private Color ChangeOpacity(Color c)
            {
                if (Opacity == 0) Opacity += double.Epsilon;
                c.A = (byte)Math.Round(Opacity * 255);
                return c;
            } 
            public Color ResultColor => ChangeOpacity(HsvToRgb(_hue * 3.6, _saturation / 100, _lightness / 100));
            
            public string ResultTextColor
            {
                get => ResultColor.ToString();
                set
                {
                    try
                    {
                        var c = ColorTranslator.FromHtml(value);
                        _hue = c.GetHue() / 360 * 100;
                        _saturation = c.GetSaturation() * 100;
                        _lightness = c.GetBrightness()  * 100;
                        _opacity = c.A / (float) 255;
                        if (_lightness == 50)
                        {
                            _lightness = 100;
                        }
                        OnPropertyChanged(nameof(Hue));
                        OnPropertyChanged(nameof(Lightness));
                        OnPropertyChanged(nameof(Saturation));
                        OnPropertyChanged(nameof(Opacity));
                        OnPropertyChanged(nameof(ResultColor));
                        OnPropertyChanged(nameof(FullSaturationColor));
                    }
                    catch (Exception)
                    {
                        // whatever
                    }
                }
            }
            public Color FullSaturationColor => ChangeOpacity(HsvToRgb(_hue * 3.6, 1, 1));

            // https://stackoverflow.com/questions/1335426/is-there-a-built-in-c-net-system-api-for-hsv-to-rgb
            public static Color HsvToRgb(double h, double s, double v)
            {
                var d = h;
                while (d < 0) { d += 360; };
                while (d >= 360) { d -= 360; };
                double r, g, b;
                if (v <= 0)
                { r = g = b = 0; }
                else if (s <= 0)
                {
                    r = g = b = v;
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
                            r = v;
                            g = tv;
                            b = pv;
                            break;

                        // Green is the dominant color

                        case 1:
                            r = qv;
                            g = v;
                            b = pv;
                            break;
                        case 2:
                            r = pv;
                            g = v;
                            b = tv;
                            break;

                        // Blue is the dominant color

                        case 3:
                            r = pv;
                            g = qv;
                            b = v;
                            break;
                        case 4:
                            r = tv;
                            g = pv;
                            b = v;
                            break;

                        // Red is the dominant color

                        case 5:
                            r = v;
                            g = pv;
                            b = qv;
                            break;

                        // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                        case 6:
                            r = v;
                            g = tv;
                            b = pv;
                            break;
                        case -1:
                            r = v;
                            g = pv;
                            b = qv;
                            break;

                        // The color is not defined, we should throw an error.

                        default:
                            //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                            r = g = b = v; // Just pretend its black/white
                            break;
                    }
                }
                var finalR = Clamp((int)(r * 255.0));
                var finalG = Clamp((int)(g * 255.0));
                var finalB = Clamp((int)(b * 255.0));
                return Color.FromRgb(finalR, finalG, finalB);
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
            ActionComplete?.Invoke(this, new ColorRequestEventArgs(_data.ResultColor));
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ActionCanceled?.Invoke(this, EventArgs.Empty);
            Close();
        }

        private void ColorTextKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Keyboard.ClearFocus();
                MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
            }
        }
    }
}
