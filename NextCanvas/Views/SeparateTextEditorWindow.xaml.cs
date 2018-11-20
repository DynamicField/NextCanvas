using NextCanvas.Controls;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace NextCanvas.Views
{
    /// <summary>
    /// Logique d'interaction pour SeparateTextEditorWindow.xaml
    /// </summary>
    public partial class SeparateTextEditorWindow : Window
    {
        public SeparateTextEditorWindow(RtfRichTextEditor editor)
        {
            InitializeComponent();
            this.editor = editor;
            Grid.Children.Add(editor);
            Height = editor.Height + SystemParameters.ResizeFrameHorizontalBorderHeight * 2 + SystemParameters.WindowCaptionHeight + 5;
            Width = editor.Width + SystemParameters.BorderWidth * 2 + SystemParameters.ResizeFrameVerticalBorderWidth + 5;
            Loaded += (sender, args) => { };
        }

        private readonly RtfRichTextEditor editor;
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            editor.Width = Grid.ActualWidth;
            editor.Height = Grid.ActualHeight;
        }
    }
}
