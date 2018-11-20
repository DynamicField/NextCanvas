using System;
using NextCanvas.Utilities;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using NextCanvas.Controls.Content;
using NextCanvas.Views;

namespace NextCanvas.Controls
{
    /// <summary>
    /// Logique d'interaction pour RtfRichTextEditor.xaml
    /// </summary>
    public partial class RtfRichTextEditor : UserControl
    {
        public RtfRichTextEditor()
        {
            InitializeComponent();
            Unloaded += (sender, args) => { TextBox.UpdateRtf(); };
            factory = new UniqueWindowFactory<SeparateTextEditorWindow>(() =>
            {
                var rtfRichTextEditor = new RtfRichTextEditor
                {
                    Width = ActualWidth,
                    Height = ActualHeight,
                };
                rtfRichTextEditor.SetBinding(RtfTextProperty, new Binding
                {
                    Source = this,
                    Path = new PropertyPath(nameof(RtfText)),
                    Mode = BindingMode.TwoWay
                });
                rtfRichTextEditor.EditorFormatShown = true;
                var window = new SeparateTextEditorWindow(rtfRichTextEditor);
                var widthBind = new Binding(nameof(Width))
                {
                    Source = this,
                    Mode = BindingMode.TwoWay
                };
                var heightBind = new Binding(nameof(Height))
                {
                    Source = this,
                    Mode = BindingMode.TwoWay
                };
                rtfRichTextEditor.SetBinding(WidthProperty, widthBind);
                rtfRichTextEditor.SetBinding(HeightProperty, heightBind);
                window.Loaded += (o, args) =>
                {
                    rtfRichTextEditor.TextBox.UpdateMode = UpdateMode.TextInput;
                    this.doNotReact = true;
                };
                return window;
            });
        }

        public static readonly DependencyProperty EditorFormatShownProperty = DependencyProperty.Register(
            "EditorFormatShown", typeof(bool), typeof(RtfRichTextEditor), new UIPropertyMetadata(true));

        public bool EditorFormatShown
        {
            get => (bool)GetValue(EditorFormatShownProperty);
            set => SetValue(EditorFormatShownProperty, value);
        }
        public string RtfText
        {
            get => (string)GetValue(RtfTextProperty);
            set => SetValue(RtfTextProperty, value);
        }

        // Using a DependencyProperty as the backing store for RtfText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RtfTextProperty =
            DependencyProperty.Register("RtfText", typeof(string), typeof(RtfRichTextEditor), new PropertyMetadata(""));

        private bool doNotReact = false;
        private void TextBox_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            doNotReact = true;
            var selection = TextBox.Selection;
            var fontFamily = TextBox.Selection.GetPropertyValue(FontFamilyProperty); // Set font :)
            var fontSize = selection.GetPropertyValue(FontSizeProperty);
            bool? isBold;
            bool? isUnderlined;
            var fontStyle = (FontStyle)TextBox.Selection.GetPropertyValue(FontStyleProperty);
            var fontDecorations = TextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            var fontWeight = TextBox.Selection.GetPropertyValue(FontWeightProperty);
            if (fontWeight == DependencyProperty.UnsetValue)
            {
                isBold = null;
            }
            else
            {
                isBold = fontWeight.Equals(FontWeights.Bold);
            }
            var isItalic = fontStyle.Equals(FontStyles.Italic);
            if (fontDecorations == DependencyProperty.UnsetValue)
            {
                isUnderlined = null;
            }
            else
            {
                isUnderlined = fontDecorations.Equals(TextDecorations.Underline);
            }
            FontSizeBox.Text = fontSize != DependencyProperty.UnsetValue ? fontSize.ToString() : "";
            FontFamilyBox.SelectedItem = fontFamily != DependencyProperty.UnsetValue ? fontFamily : null;
            BoldButton.IsChecked = isBold;
            ItalicButton.IsChecked = isItalic;
            UnderlineButton.IsChecked = isUnderlined;
            var list = VisualTreeUtilities.FindLogicalParent<List>(TextBox.Selection.Start.Parent);
            BulletsListButton.IsChecked = list != null;
            doNotReact = false;
        }
        private void FontSizeChanged(object sender, TextChangedEventArgs e)
        {
            if (doNotReact) return;
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
            if (FontFamilyBox.SelectedItem == null || doNotReact) return;
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
                    FontFamily = (FontFamily) FontFamilyBox.SelectedItem,
                    FontSize = result ?? (double) TextBox.Selection.GetPropertyValue(FontSizeProperty),
                    FontWeight = BoldButton.IsChecked ?? false ? FontWeights.Bold : FontWeights.Normal
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
            if (doNotReact) return;
            FocusTextBox();
            UpdateFormat();
        }

        private UniqueWindowFactory<SeparateTextEditorWindow> factory;
        private void OpenInANewWindow(object sender, RoutedEventArgs e)
        {
            factory.TryShowWindow();
        }
    }
}
