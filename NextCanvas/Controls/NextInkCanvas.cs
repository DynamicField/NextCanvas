using NextCanvas.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NextCanvas.Controls
{
    /// <summary>
    /// Logique d'interaction pour NextInkCanvas.xaml
    /// lol i'm funny i called it NEXT XDDDDDD
    /// </summary>
    public partial class NextInkCanvas : InkCanvas
    {
        public NextInkCanvas()
        {
            DynamicRenderer = new NextDynamicRenderer();            
        }

        public StylusShape EraserShapeDP
        {
            get { return (StylusShape)GetValue(EraserShapeDPProperty); }
            set { SetValue(EraserShapeDPProperty, value); }
        }
        
        // Using a DependencyProperty as the backing store for EraserShapeDP.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EraserShapeDPProperty =
            DependencyProperty.Register("EraserShapeDP", typeof(StylusShape), typeof(NextInkCanvas), new PropertyMetadata((sender, e) => 
            {
                (sender as NextInkCanvas).EraserShape = e.NewValue as StylusShape;
            }));


        public bool UseCustomCursorDP
        {
            get { return (bool)GetValue(UseCustomCursorDPProperty); }
            set { SetValue(UseCustomCursorDPProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UseCustomCursorDP.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UseCustomCursorDPProperty =
            DependencyProperty.Register("UseCustomCursorDP", typeof(bool), typeof(NextInkCanvas), new PropertyMetadata((sender ,e) =>
            {
                (sender as NextInkCanvas).UseCustomCursor = (bool)e.NewValue;
            }));

    }
}
