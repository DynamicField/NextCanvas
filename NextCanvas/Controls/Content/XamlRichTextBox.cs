#region

using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

#endregion

namespace NextCanvas.Controls.Content
{
    public class XamlRichTextBox : RichTextBox
    {
        // Using a DependencyProperty as the backing store for XamlText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XamlTextProperty =
            DependencyProperty.Register("XamlText", typeof(string), typeof(XamlRichTextBox),
                new FrameworkPropertyMetadata("", XamlChanged));


        public bool HasTextChangedOnce
        {
            get => (bool)GetValue(HasTextChangedOnceProperty);
            set => SetValue(HasTextChangedOnceProperty, value);
        }

        // Using a DependencyProperty as the backing store for HasTextChangedOnce.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasTextChangedOnceProperty =
            DependencyProperty.Register("HasTextChangedOnce", typeof(bool), typeof(XamlRichTextBox), new PropertyMetadata(false));

        public static readonly DependencyProperty UpdateModeProperty = DependencyProperty.Register(
            "UpdateMode", typeof(UpdateMode), typeof(XamlRichTextBox), new PropertyMetadata(UpdateMode.LostFocus));

        public UpdateMode UpdateMode
        {
            get => (UpdateMode)GetValue(UpdateModeProperty);
            set => SetValue(UpdateModeProperty, value);
        }

        private bool givingToBinding;

        public string XamlText
        {
            get => (string)GetValue(XamlTextProperty);
            set => SetValue(XamlTextProperty, value);
        }

        private static void XamlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is string value && value != string.Empty)
            {
                var textBox = (XamlRichTextBox)d;
                if (textBox.givingToBinding)
                {
                    textBox.givingToBinding = false;
                    return;
                }
                if (textBox.isTextInput) return;
                using (var stream = new MemoryStream())
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(value);
                    writer.Flush();
                    stream.Position = 0;
                    var textRange = new TextRange(textBox.Document.ContentStart, textBox.Document.ContentEnd);
                    var previousCaret = textBox.CaretPosition;
                    textRange.Load(stream, DataFormats.Xaml);
                    try
                    {
                        textBox.CaretPosition = previousCaret;
                    }
                    catch
                    {
                        textBox.CaretPosition = textBox.Document.ContentStart;
                    }
                }
            }
        }

        public void UpdateXaml()
        {
            HasTextChangedOnce = true;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            using (var mem = new MemoryStream())
            {
                new TextRange(Document.ContentStart, Document.ContentEnd).Save(mem, DataFormats.Xaml);
                givingToBinding = true;
                mem.Position = 0;
                using (var read = new StreamReader(mem, Encoding.UTF8, true))
                {
                    var result = read.ReadToEnd();
                    XamlText = result;
                }
            }
            stopwatch.Stop();
            LogManager.AddLogItem($"Successfully updated the XAML text in {stopwatch.Elapsed:g}");
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            if (HasTextChangedOnce) UpdateXaml();
        }

        private bool isTextInput;
        protected override void OnTextChanged(TextChangedEventArgs e)
        {        
            base.OnTextChanged(e);
            HasTextChangedOnce = true;
            if (UpdateMode == UpdateMode.TextInput)
            {
                isTextInput = true;
                givingToBinding = true;
                UpdateXaml();
                isTextInput = false;
            }      
        }
    }

    public enum UpdateMode
    {
        LostFocus,
        TextInput
    }
}