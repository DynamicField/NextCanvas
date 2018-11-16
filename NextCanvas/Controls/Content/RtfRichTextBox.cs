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
            get { return (bool)GetValue(HasTextChangedOnceProperty); }
            set { SetValue(HasTextChangedOnceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasTextChangedOnce.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasTextChangedOnceProperty =
            DependencyProperty.Register("HasTextChangedOnce", typeof(bool), typeof(RtfRichTextBox), new PropertyMetadata(false));


        private bool givingToBinding;

        public RtfRichTextBox()
        {
            
        }

        public string RtfText
        {
            get => (string) GetValue(RtfTextProperty);
            set => SetValue(RtfTextProperty, value);
        }

        private static void RtfChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is string value && value != string.Empty)
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
                    textBox.Selection.Select(textBox.Document.ContentStart, textBox.Document.ContentStart);
                    textBox.CaretPosition = textBox.Document.ContentStart;
                    Keyboard.ClearFocus();
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

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            HasTextChangedOnce = true;
        }
    }
}