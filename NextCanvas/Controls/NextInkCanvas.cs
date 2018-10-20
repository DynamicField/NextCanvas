using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using NextCanvas.Controls.Content;
using NextCanvas.Ink;

namespace NextCanvas.Controls
{
    /// <summary>
    /// Logique d'interaction pour NextInkCanvas.xaml
    /// lol i'm funny i called it NEXT XDDDDDD
    /// </summary>
    public class NextInkCanvas : InkCanvas
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
        // ReSharper disable once RedundantOverriddenMember
        protected override void OnStrokeCollected(InkCanvasStrokeCollectedEventArgs e)
        {
            //// Remove the original stroke and add a custom stroke.
            //this.Strokes.Remove(e.Stroke);
            //var customStroke = new SquareStroke(e.Stroke.StylusPoints, e.Stroke.DrawingAttributes);
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
        // ReSharper disable once InconsistentNaming
        public StylusShape EraserShapeDP
        {
            get => (StylusShape)GetValue(EraserShapeDPProperty);
            set => SetValue(EraserShapeDPProperty, value);
        }
        // Using a DependencyProperty as the backing store for EraserShapeDP.  This enables animation, styling, binding, etc...
        // ReSharper disable once InconsistentNaming
        public static readonly DependencyProperty EraserShapeDPProperty =
            DependencyProperty.Register("EraserShapeDP", typeof(StylusShape), typeof(NextInkCanvas), new PropertyMetadata((sender, e) =>
            {
                ((NextInkCanvas)sender).EraserShape = e.NewValue as StylusShape ?? throw new InvalidOperationException();
            }));
        // ReSharper disable once InconsistentNaming
        public bool UseCustomCursorDP
        {
            get => (bool)GetValue(UseCustomCursorDPProperty);
            set => SetValue(UseCustomCursorDPProperty, value);
        }

        // Using a DependencyProperty as the backing store for UseCustomCursorDP.  This enables animation, styling, binding, etc...
        // ReSharper disable once InconsistentNaming
        public static readonly DependencyProperty UseCustomCursorDPProperty =
            DependencyProperty.Register("UseCustomCursorDP", typeof(bool), typeof(NextInkCanvas), new PropertyMetadata((sender, e) =>
            {
                ((NextInkCanvas)sender).UseCustomCursor = (bool)e.NewValue;
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
                var casted = (NextInkCanvas) sender;
                if (e.OldValue is INotifyCollectionChanged old)
                {
                    old.CollectionChanged -= casted.ItemsSourceItemChanged;
                }
                casted.Children.Clear();
                if (e.NewValue is INotifyCollectionChanged newish)
                {
                    newish.CollectionChanged += casted.ItemsSourceItemChanged;
                }
                foreach (var item in (IEnumerable)e.NewValue)
                {
                    AddChild(casted, item);
                }
            }));

        private static void AddChild(InkCanvas canvas, object item)
        {
            var element = new ContentElementRenderer();
            canvas.Children.Add(element);
            element.Initialize(item);
        }
        private static void RemoveChild(InkCanvas canvas, object item)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            var toRemove = ((ICollection<UIElement>)canvas.Children).First(e => (e as FrameworkElement)?.DataContext == item);
            // Don't eat the exception it tastes bad
            canvas.Children.Remove(toRemove);
        }
        private void ItemsSourceItemChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in e.NewItems)
            {
                AddChild(this, item);
            }
            foreach (var item in e.OldItems)
            {
                RemoveChild(this, item);
            }
        }
    }
}
