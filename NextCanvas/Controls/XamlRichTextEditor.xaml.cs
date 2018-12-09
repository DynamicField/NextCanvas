using NextCanvas.Controls.Content;
using NextCanvas.Utilities;
using NextCanvas.Views;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace NextCanvas.Controls
{
    /// <summary>
    /// Logique d'interaction pour XamlRichTextEditor.xaml
    /// </summary>
    public partial class XamlRichTextEditor : UserControl
    {
        public XamlRichTextEditor()
        {
            InitializeComponent();
            Unloaded += (sender, args) => { TextBox.UpdateXaml(); };
            _factory = new UniqueWindowFactory<SeparateTextEditorWindow>(() =>
            {
                var rtfRichTextEditor = new XamlRichTextEditor
                {
                    Width = ActualWidth,
                    Height = ActualHeight,
                };
                rtfRichTextEditor.SetBinding(XamlTextProperty, new Binding
                {
                    Source = this,
                    Path = new PropertyPath(nameof(XamlText)),
                    Mode = BindingMode.TwoWay
                });
                rtfRichTextEditor.EditorFormatShown = true;
                var window = new SeparateTextEditorWindow(rtfRichTextEditor);
                var widthBind = new Binding(nameof(Width))
                {
                    Source = this,
                    Mode = BindingMode.OneWay
                };
                var heightBind = new Binding(nameof(Height))
                {
                    Source = this,
                    Mode = BindingMode.OneWay
                };
                rtfRichTextEditor.SetBinding(WidthProperty, widthBind);
                rtfRichTextEditor.SetBinding(HeightProperty, heightBind);
                window.Loaded += (o, args) =>
                {
                    rtfRichTextEditor.TextBox.UpdateMode = UpdateMode.TextInput;
                    var widthBindTwo = new Binding(nameof(Width))
                    {
                        Source = this,
                        Mode = BindingMode.TwoWay
                    };
                    var heightBindTwo = new Binding(nameof(Height))
                    {
                        Source = this,
                        Mode = BindingMode.TwoWay
                    };
                    rtfRichTextEditor.SetBinding(WidthProperty, widthBindTwo);
                    rtfRichTextEditor.SetBinding(HeightProperty, heightBindTwo);
                };
                return window;
            }, this);
        }

        public static readonly DependencyProperty EditorFormatShownProperty = DependencyProperty.Register(
            "EditorFormatShown", typeof(bool), typeof(XamlRichTextEditor), new UIPropertyMetadata(true));

        public bool EditorFormatShown
        {
            get => (bool)GetValue(EditorFormatShownProperty);
            set => SetValue(EditorFormatShownProperty, value);
        }

        public string XamlText
        {
            get => (string)GetValue(XamlTextProperty);
            set => SetValue(XamlTextProperty, value);
        }

        // Using a DependencyProperty as the backing store for XamlText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XamlTextProperty =
            DependencyProperty.Register("XamlText", typeof(string), typeof(XamlRichTextEditor), new PropertyMetadata(""));

        private bool _doNotReact = false;
        private void TextBox_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            _doNotReact = true;
            UpdateBoldButton();
            UpdateUnderlineButton();
            UpdateItalicButton();
            UpdateFontSizeComboBox();
            UpdateFontFamilyComboBox();
            UpdateBulletsListButton();
            UpdateColorButton();
            _doNotReact = false;
        }

        private void UpdateColorButton()
        {
            if (TextBox.Selection.GetPropertyValue(TextElement.ForegroundProperty) is Brush color)
            {
                ColorRectangle.Fill = color;
            }
            else
            {
                ColorRectangle.Fill = new SolidColorBrush(Colors.Black);
            }
        }
        private void UpdateBulletsListButton()
        {
            var list = VisualTreeUtilities.FindLogicalParent<List>(TextBox.Selection.Start.Parent);
            BulletsListButton.IsChecked = list != null;
        }

        private void UpdateItalicButton()
        {
            var fontStyle = (FontStyle)TextBox.Selection.GetPropertyValue(FontStyleProperty);
            var isItalic = fontStyle.Equals(FontStyles.Italic);
            ItalicButton.IsChecked = isItalic;
        }

        private void UpdateFontSizeComboBox()
        {
            var fontSize = TextBox.Selection.GetPropertyValue(FontSizeProperty);
            FontSizeBox.Text = fontSize != DependencyProperty.UnsetValue ? fontSize.ToString() : "";
        }

        private void UpdateFontFamilyComboBox()
        {
            var fontFamily = TextBox.Selection.GetPropertyValue(FontFamilyProperty); // Set font :)
            FontFamilyBox.SelectedItem = fontFamily != DependencyProperty.UnsetValue ? fontFamily : null;
        }

        private void UpdateUnderlineButton()
        {
            bool? isUnderlined;
            var fontDecorations = TextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            if (fontDecorations == DependencyProperty.UnsetValue)
            {
                isUnderlined = null;
            }
            else
            {
                isUnderlined = fontDecorations.Equals(TextDecorations.Underline);
            }

            UnderlineButton.IsChecked = isUnderlined;
        }

        private void UpdateBoldButton()
        {
            var fontWeight = TextBox.Selection.GetPropertyValue(FontWeightProperty);
            bool? isBold;
            if (fontWeight == DependencyProperty.UnsetValue)
            {
                isBold = null;
            }
            else
            {
                isBold = fontWeight.Equals(FontWeights.Bold);
            }

            BoldButton.IsChecked = isBold;
        }

        private void FontSizeChanged(object sender, TextChangedEventArgs e)
        {
            if (_doNotReact) return;
            SetFormatWhenNotChanged();
            var tempFix = FontSizeBox.Text;
            var processed = new string(tempFix.Where(char.IsDigit).ToArray());
            if (processed != tempFix)
            {
                FontSizeBox.Text = processed;
                return;
            }
            if (double.TryParse(processed, out var result) && result > 0)
            {
                SetFormatWhenNotChanged();
                FocusTextBox();
                TextBox.Selection.ApplyPropertyValue(FontSizeProperty, result);
                SettingsManager.Settings.DefaultFontSize = result;
            }
        }

        private void FontBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontFamilyBox.SelectedItem == null || _doNotReact) return;
            FocusTextBox();
            TextBox.Selection.ApplyPropertyValue(FontFamilyProperty, FontFamilyBox.SelectedItem); // Set font :)
            SettingsManager.Settings.DefaultFontFamily = (FontFamily)FontFamilyBox.SelectedItem;
        }

        private void RtfRichTextEditor_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!TextBox.HasTextChangedOnce)
            {
                FocusTextBox();
                SetFormatForEmptyTextBox();
                TextBox_OnSelectionChanged(null, null);
            }
        }

        private void SetFormatWhenNotChanged()
        {
            if (!TextBox.HasTextChangedOnce || string.IsNullOrWhiteSpace(new TextRange(TextBox.Document.ContentStart, TextBox.Document.ContentEnd).Text))
            {
                SetFormatForEmptyTextBox();
            }
        }

        private void SetFormatForEmptyTextBox()
        {
            var result = SettingsManager.Settings.DefaultFontSize;
            double.TryParse(FontSizeBox.Text, out result);
            TextBox.CaretPosition.Paragraph?.Inlines.Clear();
            UpdateFormat(result);
        }

        private void UpdateFormat(double? result = null)
        {
            try
            {
                var inline = new Run("")
                {
                    FontFamily = (FontFamily)FontFamilyBox.SelectedItem,
                    FontSize = result ?? (double)TextBox.Selection.GetPropertyValue(FontSizeProperty),
                    FontWeight = BoldButton.IsChecked ?? false ? FontWeights.Bold : FontWeights.Normal,
                    Foreground = ColorRectangle.Fill
                };
                TextBox.CaretPosition.Paragraph?.Inlines.Add(inline);
            }
            catch (Exception e)
            {
                LogManager.AddLogItem($"Couldn't update the empty text box: {e}", status: LogEntryStatus.Warning);
            }
        }

        private void FocusTextBox()
        {
            if (EditorFormatShown)
            {
                TextBox.Focus();
            }
        }

        private void FocusTextBox(object sender, RoutedEventArgs e)
        {
            if (_doNotReact) return;
            FocusTextBox();
            UpdateFormat();
        }

        private UniqueWindowFactory<SeparateTextEditorWindow> _factory;
        private void OpenInANewWindow(object sender, RoutedEventArgs e)
        {
            _factory.TryShowWindow();
        }

        private void ColorButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_doNotReact) return;
            var window = Window.GetWindow(this);
            var colorChooser = new ColorChooserWindow { Owner = window };
            colorChooser.ActionComplete += (o, args) =>
            {
                var brush = new SolidColorBrush(args.Color);
                brush.Freeze();
                SettingsManager.Settings.DefaultTextBoxColor = args.Color;
                colorChooser.Closed += (_, __) =>
                {
                    window?.Focus();
                    FocusTextBox();
                    TextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, brush);
                    TextBox_OnSelectionChanged(null, null);
                };
            };
            colorChooser.ShowDialog();
        }
    }
}
