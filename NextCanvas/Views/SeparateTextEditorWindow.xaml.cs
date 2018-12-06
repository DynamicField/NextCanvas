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
    public partial class SeparateTextEditorWindow : Window // For easier resizes or not, it just glows, its bootifool
    {
        public SeparateTextEditorWindow(XamlRichTextEditor editor)
        {
            InitializeComponent();
            this._editor = editor;
            Grid.Children.Add(editor);
            _lastHeight = editor.Height;
            _lastWidth = editor.Width;
            Loaded += (sender, args) => { UpdateWindowDimensions(); };
            editor.SizeChanged += Editor_SizeChanged;
        }

        private void Editor_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SizeToContent = SizeToContent.WidthAndHeight;
            SizeToContent = SizeToContent.Manual;
        }

        private void UpdateWindowDimensions()
        {
            Height = _lastHeight + (ActualHeight - Grid.ActualHeight);
            Width = _lastWidth + (ActualWidth - Grid.ActualWidth);
        }

        private int _noResizeCount = 0;
        private double _lastWidth, _lastHeight;
        private readonly XamlRichTextEditor _editor;
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            _noResizeCount = 1;
            _editor.Width = Grid.ActualWidth;
            _editor.Height = Grid.ActualHeight;
        }
    }
}
