using NextCanvas.Ink;
using NextCanvas.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
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
            PreferredPasteFormats = new List<InkCanvasClipboardFormat>
            {
                InkCanvasClipboardFormat.InkSerializedFormat,
                InkCanvasClipboardFormat.Xaml,
                InkCanvasClipboardFormat.Text
            };

        }
        protected override void OnStrokeCollected(InkCanvasStrokeCollectedEventArgs e)
        {

            //// Remove the original stroke and add a custom stroke.
            //this.Strokes.Remove(e.Stroke);
            //var customStroke = new SquareStroke(e.Stroke.StylusPoints);
            //this.Strokes.Add(customStroke);

            //// Pass the custom stroke to base class' OnStrokeCollected method.
            //InkCanvasStrokeCollectedEventArgs args =
            //    new InkCanvasStrokeCollectedEventArgs(customStroke);
            //base.OnStrokeCollected(args);
            base.OnStrokeCollected(e);

        }
        protected override void OnStrokeErasing(InkCanvasStrokeErasingEventArgs e)
        {
            if (e.Stroke is SquareStroke stroke)
            {
                stroke.DisableSquare = true;
            }
            base.OnStrokeErasing(e);
        }
        public StylusShape EraserShapeDP
        {
            get => (StylusShape)GetValue(EraserShapeDPProperty);
            set => SetValue(EraserShapeDPProperty, value);
        }
        // Using a DependencyProperty as the backing store for EraserShapeDP.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EraserShapeDPProperty =
            DependencyProperty.Register("EraserShapeDP", typeof(StylusShape), typeof(NextInkCanvas), new PropertyMetadata((sender, e) =>
            {
                (sender as NextInkCanvas).EraserShape = e.NewValue as StylusShape;
            }));
        public bool UseCustomCursorDP
        {
            get => (bool)GetValue(UseCustomCursorDPProperty);
            set => SetValue(UseCustomCursorDPProperty, value);
        }

        // Using a DependencyProperty as the backing store for UseCustomCursorDP.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UseCustomCursorDPProperty =
            DependencyProperty.Register("UseCustomCursorDP", typeof(bool), typeof(NextInkCanvas), new PropertyMetadata((sender, e) =>
            {
                (sender as NextInkCanvas).UseCustomCursor = (bool)e.NewValue;
            }));

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(NextInkCanvas), new FrameworkPropertyMetadata(null, (sender, e) =>
            {
                if (e.OldValue == e.NewValue)
                {
                    return;
                }
                NextInkCanvas casted = sender as NextInkCanvas;

                if (e.OldValue is INotifyCollectionChanged old)
                {
                    old.CollectionChanged -= casted.ItemsSourceItemChanged;
                }
                casted.Children.Clear();
                if (e.NewValue is INotifyCollectionChanged newish)
                {
                    newish.CollectionChanged += casted.ItemsSourceItemChanged;
                }
                foreach (object item in e.NewValue as IEnumerable)
                {
                    AddChild(casted, item);
                }
            }));

        private static void AddChild(NextInkCanvas canvas, object item)
        {
            canvas.Children.Add(new Controls.Content.ContentElementRenderer()
            {
                DataContext = item
            });
        }
        private static void RemoveChild(NextInkCanvas canvas, object item)
        {
            UIElement toRemove = ((ICollection<UIElement>)canvas.Children).First(e => (e as FrameworkElement)?.DataContext == item);
            // Don't eat the exception it tastes bad
            canvas.Children.Remove(toRemove);
        }
        private void ItemsSourceItemChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (object item in e.NewItems)
            {
                AddChild(this, item);
            }
            foreach (object item in e.OldItems)
            {
                RemoveChild(this, item);
            }
        }
    }
}
