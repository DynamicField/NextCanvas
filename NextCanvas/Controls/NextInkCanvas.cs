using NextCanvas.Controls.Content;
using NextCanvas.Ink;
using NextCanvas.Models;
using NextCanvas.ViewModels.Content;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using NextCanvas.Properties;

namespace NextCanvas.Controls
{
    /// <summary>
    ///     Logique d'interaction pour NextInkCanvas.xaml
    ///     lol i'm funny i called it NEXT XDDDDDD
    /// </summary>
    public class NextInkCanvas : InkCanvas
    {
        // Using a DependencyProperty as the backing store for ScrollViewerReferent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScrollViewerReferentProperty =
            DependencyProperty.Register("ScrollViewerReferent", typeof(ScrollViewer), typeof(NextInkCanvas),
                new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for EraserShapeDP.  This enables animation, styling, binding, etc...
        // ReSharper disable once InconsistentNaming
        public static readonly DependencyProperty EraserShapeDPProperty =
            DependencyProperty.Register("EraserShapeDP", typeof(StylusShape), typeof(NextInkCanvas),
                new PropertyMetadata((sender, e) =>
                {
                    ((NextInkCanvas)sender).EraserShape =
                        e.NewValue as StylusShape ?? throw new InvalidOperationException();
                }));

        // Using a DependencyProperty as the backing store for UseCustomCursorDP.  This enables animation, styling, binding, etc...
        // ReSharper disable once InconsistentNaming
        public static readonly DependencyProperty UseCustomCursorDPProperty =
            DependencyProperty.Register("UseCustomCursorDP", typeof(bool), typeof(NextInkCanvas),
                new PropertyMetadata((sender, e) => { ((NextInkCanvas)sender).UseCustomCursor = (bool)e.NewValue; }));

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<ContentElementViewModel>),
                typeof(NextInkCanvas),
                new FrameworkPropertyMetadata(null, (sender, e) =>
                {
                    if (e.OldValue == e.NewValue)
                    {
                        return;
                    }

                    if (e.NewValue == null)
                    {
                        return;
                    }

                    var casted = (NextInkCanvas)sender;
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


        public StrokeDelegate<Stroke> CustomStrokeInvocator
        {
            get => (StrokeDelegate<Stroke>)GetValue(CustomStrokeInvocatorProperty);
            set => SetValue(CustomStrokeInvocatorProperty, value);
        }

        // Using a DependencyProperty as the backing store for CustomStrokeInvocator.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomStrokeInvocatorProperty =
            DependencyProperty.Register("CustomStrokeInvocator", typeof(StrokeDelegate<Stroke>), typeof(NextInkCanvas), new PropertyMetadata(null));


        private bool isInternal;

        private MemoryStream lastDataObject;
        private double pasteDiff;

        public NextInkCanvas()
        {
            DynamicRenderer = new NextDynamicRenderer();
            SelectionHelper = new SelectionWrapper(SelectChildren);
            PreferredPasteFormats = new List<InkCanvasClipboardFormat>
            {
                InkCanvasClipboardFormat.InkSerializedFormat
            };
            CommandManager.RegisterClassCommandBinding(typeof(NextInkCanvas),
                new CommandBinding(ApplicationCommands.Delete, CommandExecuted, CanExecuteCommand));
            CommandManager.RegisterClassCommandBinding(typeof(NextInkCanvas),
                new CommandBinding(ApplicationCommands.Paste, CommandExecuted, CanExecuteCommand));
        }

        public SelectionWrapper SelectionHelper { get; }


        public ScrollViewer ScrollViewerReferent
        {
            get => (ScrollViewer)GetValue(ScrollViewerReferentProperty);
            set => SetValue(ScrollViewerReferentProperty, value);
        }

        // ReSharper disable once InconsistentNaming
        public StylusShape EraserShapeDP
        {
            get => (StylusShape)GetValue(EraserShapeDPProperty);
            set => SetValue(EraserShapeDPProperty, value);
        }

        // ReSharper disable once InconsistentNaming
        public bool UseCustomCursorDP
        {
            get => (bool)GetValue(UseCustomCursorDPProperty);
            set => SetValue(UseCustomCursorDPProperty, value);
        }

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public void SelectChildren(object dataContext)
        {
            Select(new[] { GetElementFromDataContext(this, dataContext) });
        }

        protected override void OnSelectionChanged(EventArgs e)
        {
            var elements = GetSelectedElements();
            if (elements.Count == 1)
            {
                var element = elements[0];
                var highestZIndex = Children.Cast<UIElement>().Select(el => (int)el.GetValue(Panel.ZIndexProperty))
                    .OrderByDescending(i => i).First();
                var zIndex = (int)element.GetValue(Panel.ZIndexProperty);
                if (highestZIndex == 0 || zIndex != highestZIndex)
                {
                    element.SetValue(Panel.ZIndexProperty, highestZIndex + 1);
                }
                element.Focus();
                if (element is ContentElementRenderer render)
                {
                    render.FocusChild();
                }
            }
            base.OnSelectionChanged(e);
        }

        private static void CommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var canvas = (NextInkCanvas)sender;
            if (e.Command == ApplicationCommands.Delete)
            {
                canvas.DeleteSelection();
            }

            if (e.Command == ApplicationCommands.Paste)
            {
                canvas.Paste();
            }
        }

        /// <summary>
        ///     Pastes the clipboard content to the *CENTER*.
        /// </summary>
        public new void Paste()
        {
            UpdatePasteDifference();
            if (!CanPaste())
            {
                return;
            }

            double horizontalOffset = 0, verticalOffset = 0;
            double width = ActualWidth, height = ActualHeight;
            if (ScrollViewerReferent != null) // sees if there is any useful scroll viewers.
            {
                horizontalOffset = ScrollViewerReferent.ContentHorizontalOffset;
                verticalOffset = ScrollViewerReferent.ContentVerticalOffset;
                width = ScrollViewerReferent.ActualWidth;
                height = ScrollViewerReferent.ActualHeight;
            }

            Paste(new Point(horizontalOffset + width / 2 + pasteDiff, verticalOffset + height / 2 + pasteDiff));
        }

        private void UpdatePasteDifference()
        {
            if (!(Clipboard.GetData(StrokeCollection.InkSerializedFormat) is MemoryStream data))
            {
                pasteDiff = 0;
                return;
            }

            if (lastDataObject == null)
            {
                pasteDiff = 0;
                lastDataObject = data;
                return;
            }

            if (data.Length == lastDataObject.Length
            ) // It's less heavier than having to create two stream readers and blah blah blah
            {
                pasteDiff += 5;
            }
            else
            {
                pasteDiff = 0;
            }

            lastDataObject = data;
        }

        private static void CanExecuteCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            var canvas = (NextInkCanvas)sender;
            if (e.Command == ApplicationCommands.Delete)
            {
                e.CanExecute = canvas.GetSelectedElements().Any() || canvas.GetSelectedStrokes().Any();
            }

            if (e.Command == ApplicationCommands.Paste)
            {
                e.CanExecute = canvas.CanPaste();
            }
        }

        public void DeleteSelection()
        {
            var list = GetSelectedStrokes();
            Strokes.Remove(list);
            var elements = GetSelectedElements();
            if (elements.Any())
            {
                for (var i = elements.Count - 1; i >= 0; i--)
                {
                    RemoveChild(this, ((FrameworkElement)elements[i]).DataContext);
                }
            }
        }
        protected override void OnStrokeCollected(InkCanvasStrokeCollectedEventArgs e)
        {
            if (CustomStrokeInvocator == null)
            {
                base.OnStrokeCollected(e);
                return;
            }
            // Remove the original stroke and add a custom stroke.
            Strokes.Remove(e.Stroke);
            var customStroke = CustomStrokeInvocator(e.Stroke);
            // Store the type name, UwU
            var typeName = customStroke.GetType().FullName;
            if (typeName != null)
            {
                customStroke.AddPropertyData(AssemblyInfo.Guid, typeName);
            }

            Strokes.Add(customStroke);
            // Pass the custom stroke to base class' OnStrokeCollected method.
            var args = new InkCanvasStrokeCollectedEventArgs(customStroke);
            base.OnStrokeCollected(args);

        }
        protected override void OnStrokeErasing(InkCanvasStrokeErasingEventArgs e)
        {
            if (e.Stroke is SquareStroke stroke)
            {
                stroke.DisableSquare = true;
            }

            base.OnStrokeErasing(e);
        }

        private static void AddChild(InkCanvas canvas, object item)
        {
            var element = new ContentElementRenderer();
            canvas.Children.Add(element);
            element.Initialize(item);
        }

        private static void RemoveChild(NextInkCanvas canvas, object item)
        {
            if (canvas.ItemsSource is IList l)
            {
                canvas.isInternal = true;
                l.Remove(item);
            }
        }

        private static FrameworkElement GetElementFromDataContext(NextInkCanvas canvas, object item)
        {
            return canvas.Children.Cast<FrameworkElement>().First(e => e.DataContext == item);
        }

        private static void RemoveVisualChild(NextInkCanvas canvas, UIElement item)
        {
            canvas.Children.Remove(item);
        }

        private static void RemoveVisualChild(NextInkCanvas canvas, object dataContext)
        {
            RemoveVisualChild(canvas, GetElementFromDataContext(canvas, dataContext));
        }

        private void ItemsSourceItemChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    AddChild(this, item);
                }
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    if (isInternal)
                    {
                        RemoveVisualChild(this, item);
                    }
                    else
                    {
                        RemoveChild(this, item);
                    }
                }
            }

            isInternal = false;
        }
    }
}