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
    public class RtfRichTextBox : RichTextBox
    {
        // Using a DependencyProperty as the backing store for RtfText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RtfTextProperty =
            DependencyProperty.Register("RtfText", typeof(string), typeof(RtfRichTextBox),
                new FrameworkPropertyMetadata("", RtfChanged));


        public bool HasTextChangedOnce
        {
            get => (bool)GetValue(HasTextChangedOnceProperty);
            set => SetValue(HasTextChangedOnceProperty, value);
        }

        // Using a DependencyProperty as the backing store for HasTextChangedOnce.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasTextChangedOnceProperty =
            DependencyProperty.Register("HasTextChangedOnce", typeof(bool), typeof(RtfRichTextBox), new PropertyMetadata(false));

        public static readonly DependencyProperty UpdateModeProperty = DependencyProperty.Register(
            "UpdateMode", typeof(UpdateMode), typeof(RtfRichTextBox), new PropertyMetadata(UpdateMode.LostFocus));

        public static readonly DependencyProperty XamlTextProperty = DependencyProperty.Register(
            "XamlText", typeof(string), typeof(RtfRichTextBox), new PropertyMetadata(""));

        public string XamlText
        {
            get { return (string) GetValue(XamlTextProperty); }
            set { SetValue(XamlTextProperty, value); }
        }
        public UpdateMode UpdateMode
        {
            get => (UpdateMode)GetValue(UpdateModeProperty);
            set => SetValue(UpdateModeProperty, value);
        }

        private bool givingToBinding;

        public RtfRichTextBox()
        {

        }

        public string RtfText
        {
            get => (string)GetValue(RtfTextProperty);
            set => SetValue(RtfTextProperty, value);
        }

        private static void RtfChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is string value && value != string.Empty)
            {
                var textBox = (RtfRichTextBox)d;
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
                    textRange.Load(stream, DataFormats.Rtf);
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

        public void UpdateRtf()
        {
            HasTextChangedOnce = true;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            using (var mem = new MemoryStream())
            {
                new TextRange(Document.ContentStart, Document.ContentEnd).Save(mem, DataFormats.Rtf);
                givingToBinding = true;
                mem.Position = 0;
                using (var read = new StreamReader(mem, Encoding.UTF8, true))
                {
                    var result = read.ReadToEnd();
                    RtfText = result;
                }
            }
            stopwatch.Stop();
            LogManager.AddLogItem($"Successfully updated the RTF text in {stopwatch.Elapsed:g}");
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            if (HasTextChangedOnce) UpdateRtf();
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
                UpdateRtf();
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