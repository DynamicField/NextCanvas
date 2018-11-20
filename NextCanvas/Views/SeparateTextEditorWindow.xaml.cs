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
using Fluent;

namespace NextCanvas.Views
{
    /// <summary>
    /// Logique d'interaction pour SeparateTextEditorWindow.xaml
    /// </summary>
    public partial class SeparateTextEditorWindow : RibbonWindow // For easier resizes or not, it just glows, its bootifool
    {
        public SeparateTextEditorWindow(RtfRichTextEditor editor)
        {
            InitializeComponent();
            this.editor = editor;
            Grid.Children.Add(editor);
            lastHeight = editor.Height;
            lastWidth = editor.Width;
            Loaded += (sender, args) =>
            {
                Height = lastHeight + (ActualHeight - Grid.ActualHeight);
                Width = lastWidth + (ActualWidth - Grid.ActualWidth);
            };
        }

        private double lastWidth, lastHeight;
        private readonly RtfRichTextEditor editor;
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            editor.Width = Grid.ActualWidth;
            editor.Height = Grid.ActualHeight;
        }
    }
}
