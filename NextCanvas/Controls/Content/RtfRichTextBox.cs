using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace NextCanvas.Controls.Content
{
    public class RtfRichTextBox : RichTextBox
    {
        // Using a DependencyProperty as the backing store for RtfText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RtfTextProperty =
            DependencyProperty.Register("RtfText", typeof(string), typeof(RtfRichTextBox),
                new FrameworkPropertyMetadata("", RtfChanged));

        private bool givingToBinding;

        private bool updateNeeded;

        public string RtfText
        {
            get => (string) GetValue(RtfTextProperty);
            set => SetValue(RtfTextProperty, value);
        }

        private static void RtfChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is string value)
            {
                var textBox = (RtfRichTextBox) d;
                if (textBox.givingToBinding)
                {
                    textBox.givingToBinding = false;
                    return;
                }

                textBox.SelectAll();
                using (var stream = new MemoryStream())
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(value);
                    writer.Flush();
                    stream.Position = 0;
                    textBox.Selection.Load(stream, DataFormats.Rtf);
                }
            }
        }

        private void UpdateRtf()
        {
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
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            if (updateNeeded) UpdateRtf();
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            updateNeeded = true;
        }
    }
}