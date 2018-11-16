using NextCanvas.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private void TextBox_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            var selection = TextBox.Selection;
            FontFamilyBox.SelectedItem = TextBox.Selection.GetPropertyValue(FontFamilyProperty); // Set font :)
            var fontSize = selection.GetPropertyValue(FontSizeProperty);
            if (fontSize != DependencyProperty.UnsetValue)
            {
                FontSizeBox.Text = fontSize.ToString();
            }
            else
            {
                FontSizeBox.Text = "";
            }
        }

        private bool focus = false;
        private void FontSizeChanged(object sender, TextChangedEventArgs e)
        {
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
            TextBox.CaretPosition.Paragraph?.Inlines.Add(new Run("")
            {
                FontFamily = (FontFamily) FontFamilyBox.SelectedItem,
                FontSize = result
            });
        }

        private void FocusTextBox()
        {
            if (EditorFormatShown)
            {
                TextBox.Focus();
            }
        }
    }
}
