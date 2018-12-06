#region

using NextCanvas.Controls.Content;
using NextCanvas.Ink;
using NextCanvas;
using NextCanvas.Properties;
using NextCanvas.Content.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NextCanvas.Serialization;
using NextCanvas.Views.Editor;

#endregion

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
                    if (e.NewValue != null)
                    ((NextInkCanvas) sender).EraserShape = e.NewValue as StylusShape ?? throw new InvalidOperationException();
                }));

        // Using a DependencyProperty as the backing store for UseCustomCursorDP.  This enables animation, styling, binding, etc...
        // ReSharper disable once InconsistentNaming
        public static readonly DependencyProperty UseCustomCursorDPProperty =
            DependencyProperty.Register("UseCustomCursorDP", typeof(bool), typeof(NextInkCanvas),
                new PropertyMetadata((sender, e) => { if (e.NewValue == null) return; ((NextInkCanvas)sender).UseCustomCursor = (bool)e.NewValue; }));

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
                    var casted = (NextInkCanvas)sender;
                    if (e.OldValue is INotifyCollectionChanged old)
                    {
                        old.CollectionChanged -= casted.ItemsSourceItemChanged;
                    }
                    casted.Children.Clear();
                    if (e.NewValue == null)
                    {
                        return;
                    }
                    if (e.NewValue is INotifyCollectionChanged newish)
                    {
                        newish.CollectionChanged += casted.ItemsSourceItemChanged;
                    }

                    foreach (var item in (IEnumerable)e.NewValue)
                    {
                        AddChild(casted, item);
                    }
                }));

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.RegisterAttached("IsSelected", typeof(bool), 
            typeof(NextInkCanvas),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits ));

        public static bool GetIsSelected(DependencyObject o)
        {
            return (bool) o.GetValue(IsSelectedProperty);
        }

        public static void SetIsSelected(DependencyObject o, bool value)
        {
            o.SetValue(IsSelectedProperty, value);
        }
        public StrokeDelegate<Stroke> CustomStrokeInvocator
        {
            get => (StrokeDelegate<Stroke>)GetValue(CustomStrokeInvocatorProperty);
            set => SetValue(CustomStrokeInvocatorProperty, value);
        }

        // Using a DependencyProperty as the backing store for CustomStrokeInvocator.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomStrokeInvocatorProperty =
            DependencyProperty.Register("CustomStrokeInvocator", typeof(StrokeDelegate<Stroke>), typeof(NextInkCanvas), new PropertyMetadata(null));


        public ObservableCollection<ContentElementViewModel> SelectedItems
        {
            get => (ObservableCollection<ContentElementViewModel>)GetValue(SelectedItemsProperty);
            set => SetValue(SelectedItemsProperty, value);
        }

        // Using a DependencyProperty as the backing store for SelectedItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(ObservableCollection<ContentElementViewModel>), typeof(NextInkCanvas), new FrameworkPropertyMetadata(new ObservableCollection<ContentElementViewModel>(),
                (o, e) =>
                {
                    void SelectedItemsChangedExternally(object sender, NotifyCollectionChangedEventArgs args)
                    {
                        ((NextInkCanvas)o).UpdateSelection();
                    }
                    if (e.OldValue is ObservableCollection<ContentElementViewModel> old)
                    {
                        old.CollectionChanged -= SelectedItemsChangedExternally;
                    }
                    var collection = (ObservableCollection<ContentElementViewModel>)e.NewValue;
                    var canvas = (NextInkCanvas)o;
                    UpdateSelection(canvas, collection);
                    collection.CollectionChanged += SelectedItemsChangedExternally;
                }));


        public static readonly DependencyProperty IsLightweightRenderingProperty = DependencyProperty.Register(
            "IsLightweightRendering", typeof(bool), typeof(NextInkCanvas), new PropertyMetadata(false, LightweightRenderingChanged));

        public bool IsLightweightRendering
        {
            get => (bool) GetValue(IsLightweightRenderingProperty);
            set => SetValue(IsLightweightRenderingProperty, value);
        }
        public static readonly DependencyProperty IsHostedInLightweightRenderingProperty = DependencyProperty.RegisterAttached("IsHostedInLightweightRendering", typeof(bool),
            typeof(NextInkCanvas),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

        private static void LightweightRenderingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue) return;
            var canvas = (NextInkCanvas) d;
            SetIsHostedInLightweightRendering(canvas, (bool) e.NewValue);
            foreach (var uiElement in canvas.Children.OfType<UIElement>())
            {
                SetIsHostedInLightweightRendering(uiElement, (bool) e.NewValue);
            }
        }

        public static bool GetIsHostedInLightweightRendering(DependencyObject o)
        {
            return (bool)o.GetValue(IsHostedInLightweightRenderingProperty);
        }

        public static void SetIsHostedInLightweightRendering(DependencyObject o, bool value)
        {
            o.SetValue(IsHostedInLightweightRenderingProperty, value);
        }

        private static void UpdateSelection(NextInkCanvas canvas, ObservableCollection<ContentElementViewModel> c)
        {
            if (canvas._isSelectionInternal)
            {
                canvas._isSelectionInternal = canvas._shouldAutoSet;
                canvas._shouldAutoSet = false;
                return;
            }
            var elements = new List<FrameworkElement>();
            foreach (var element in c)
            {
                var item = GetElementFromDataContext(canvas, element);
                if (item != null)
                {
                    elements.Add(item);
                }
            }
            canvas._isSelectionInternal = true;
            canvas._shouldAutoSet = true;
            if (elements.Any())
                canvas.Select(elements);
        }

        private void UpdateSelection()
        {
            UpdateSelection(this, SelectedItems);
        }

        private bool _isInternal;
        private bool _isSelectionInternal;
        private bool _shouldAutoSet;
        private MemoryStream _lastDataObject;
        private double _pasteDiff;

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
            CommandManager.RegisterClassCommandBinding(typeof(NextInkCanvas),
                new CommandBinding(ApplicationCommands.SelectAll, CommandExecuted, CanExecuteCommand));
            Unloaded += NextInkCanvas_Unloaded;
        }

        private void NextInkCanvas_Unloaded(object sender, RoutedEventArgs e)
        {
            ItemsSource = null;
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

        public ObservableCollection<ContentElementViewModel> ItemsSource
        {
            get => (ObservableCollection<ContentElementViewModel>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public void SelectChildren(object dataContext)
        {
            Select(new[] { GetElementFromDataContext(this, dataContext) });
        }
        protected override void OnSelectionChanged(EventArgs e)
        {
            var elements = GetSelectedElements();
            RemoveAdorners(elements);
            SelectedItems.Clear();
            AddAdornersAndAddToSelected(elements);
            SetZIndexes(elements);
            base.OnSelectionChanged(e);
        }

        private void RemoveAdorners(ReadOnlyCollection<UIElement> elements)
        {
            foreach (var item in SelectedItems)
            {
                var element = GetElementFromDataContext(this, item);
                if (element == null || elements.Contains(element)) continue;
                SetIsSelected(element, false);
                var adorner = AdornerLayer.GetAdornerLayer(element);
                var array = adorner.GetAdorners(element);
                if (array == null) continue;
                foreach (var appliedAdorner in array)
                {
                    adorner.Remove(appliedAdorner);
                }
            }
        }

        private void AddAdornersAndAddToSelected(ReadOnlyCollection<UIElement> elements)
        {
            if (elements.Any())
            {
                foreach (var element in elements)
                {
                    _shouldAutoSet = false;
                    _isSelectionInternal = true;
                    if (element is FrameworkElement frameworkElement &&
                        GetDataContextFromElement(frameworkElement) is ContentElementViewModel item)
                    {
                        SetIsSelected(frameworkElement, true);
                        SelectedItems.Add(item);
                        var adornerLayer = AdornerLayer.GetAdornerLayer(frameworkElement);
                        adornerLayer.Add(new ModifyElementAdorner(frameworkElement));
                    }
                }
            }
        }

        private void SetZIndexes(ReadOnlyCollection<UIElement> elements)
        {
            if (elements.Count == 1)
            {
                var element = elements[0];
                var children = Children.Cast<UIElement>();
                var uiElements = children as UIElement[] ?? children.ToArray();
                var highestZIndex = uiElements
                    .Select(el => (int) el.GetValue(Panel.ZIndexProperty))
                    .OrderByDescending(i => i).First();
                var zIndex = (int) element.GetValue(Panel.ZIndexProperty);

                if (highestZIndex == 0 || zIndex != highestZIndex)
                {
                    element.SetValue(Panel.ZIndexProperty, highestZIndex + 1);
                }

                element.Focus();
                if (element is ContentElementRenderer render)
                {
                    render.FocusChild();
                }

                ReorderZIndexes(uiElements);
            }
        }

        private void ReorderZIndexes(IEnumerable<UIElement> elements)
        {
            var ordered = elements.OrderBy(u => (int) u.GetValue(Panel.ZIndexProperty));
            var count = 0;
            foreach (var element in ordered)
            {
                element.SetValue(Panel.ZIndexProperty, count++);
            }
        }
        protected override void OnSelectionMoving(InkCanvasSelectionEditingEventArgs e)
        {
            var elements = GetSelectedElements().OfType<FrameworkElement>();
            var browsers = elements.Select(GetDataContextFromElement).OfType<WebBrowserElementViewModel>();
            var models = browsers as WebBrowserElementViewModel[] ?? browsers.ToArray();
            if (!models.Any() || e.OldRectangle.Y > e.NewRectangle.Y)
            {
                base.OnSelectionMoving(e);
                return;
            }
            // Don't make a web browser overflow or the window will be covered with it and it's ugly :c
            var highest = models.OrderByDescending(w => w.Top + w.Height).First();
            var bottom = highest.Top + highest.Height - e.OldRectangle.Y + e.NewRectangle.Y;
            var height = (ScrollViewerReferent?.ActualHeight ?? ActualHeight);
            if (bottom > height)
            {
                var rect = e.NewRectangle;
                rect.Y -= bottom - height;
                e.NewRectangle = rect;
            }
            base.OnSelectionMoving(e);
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

            if (e.Command == ApplicationCommands.SelectAll)
            {
                var elements = canvas.Children;
                var strokes = canvas.Strokes;
                canvas.Select(strokes, elements.OfType<UIElement>());
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

            if (Clipboard.GetData(StrokeCollection.InkSerializedFormat) is MemoryStream stream)
            {
                SquareStroke.GetMinPoints(new StrokeCollection(stream).SelectMany(s => s.StylusPoints), out var minX,
                    out var minY, out var maxX, out var maxY);
                width -= maxX.X - minX.X;
                height -= maxY.Y - minY.Y;
            }
            Paste(new Point(horizontalOffset + width / 2 + _pasteDiff, verticalOffset + height / 2 + _pasteDiff));
        }
        private void UpdatePasteDifference()
        {
            if (!(Clipboard.GetData(StrokeCollection.InkSerializedFormat) is MemoryStream data))
            {
                _pasteDiff = 0;
                return;
            }

            if (_lastDataObject == null)
            {
                _pasteDiff = 0;
                _lastDataObject = data;
                return;
            }

            if (data.Length == _lastDataObject.Length
            ) // It's less heavier than having to create two stream readers and blah blah blah
            {
                _pasteDiff += 5;
            }
            else
            {
                _pasteDiff = 0;
            }

            _lastDataObject = data;
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

            if (e.Command == ApplicationCommands.SelectAll)
            {
                e.CanExecute = true;
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
            StrokeSerializer.ProcessStroke(customStroke);

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
                canvas._isInternal = true;
                l.Remove(item);
            }
        }

        private static FrameworkElement GetElementFromDataContext(NextInkCanvas canvas, object item)
        {
            return canvas.Children.Cast<FrameworkElement>().FirstOrDefault(e => e.DataContext == item);
        }
        private static object GetDataContextFromElement(NextInkCanvas canvas, FrameworkElement element)
        {
            return canvas.ItemsSource.First(e => element.DataContext == e);
        }

        private object GetDataContextFromElement(FrameworkElement element)
        {
            return GetDataContextFromElement(this, element);
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
                    if (_isInternal)
                    {
                        RemoveVisualChild(this, item);
                    }
                    else
                    {
                        RemoveChild(this, item);
                    }
                }
            }

            _isInternal = false;
        }

        private class ModifyElementAdorner : Adorner
        {
            private VisualCollection _visuals;
            private Button _settingsButton;
            public ModifyElementAdorner(UIElement adornedElement) : base(adornedElement)
            {
                _visuals = new VisualCollection(this);
                var image = new Image
                {
                    Width = 24
                };
                var bitmap =
                    new BitmapImage(new Uri("pack://application:,,,/NextCanvas;component/Images/Menu/Settings.png"));
                image.Source = bitmap;
                _settingsButton = new Button
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Content = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Children =
                        {
                            image,
                            new TextBlock
                            {
                                Text = RibbonResources.ToolGallery_EditItem,
                                VerticalAlignment = VerticalAlignment.Center,
                                Margin = new Thickness(3)
                            }
                        }
                    },
                    Margin = new Thickness(0),
                    Padding = new Thickness(5)
                };
                _settingsButton.Click += SettingsButton_Click;
                _visuals.Add(_settingsButton);
            }

            private void SettingsButton_Click(object sender, RoutedEventArgs e)
            {
                if (!(AdornedElement is FrameworkElement frameworkElement)) return;
                var window = new ModifyObjectWindow(Window.GetWindow(this))
                {
                    ObjectToModify = frameworkElement.DataContext
                };
                window.Show();
            }

            protected override Size ArrangeOverride(Size finalSize)
            {
                _settingsButton.Arrange(new Rect(-3, -10 - finalSize.Height, finalSize.Width, finalSize.Height));
                return _settingsButton.RenderSize;
            }

            protected override Size MeasureOverride(Size constraint)
            {
                _settingsButton.Measure(constraint);
                return _settingsButton.DesiredSize;
            }
            protected override Visual GetVisualChild(int index)
            {
                return _visuals[index];
            }

            protected override int VisualChildrenCount => _visuals.Count;
        }
    }
}