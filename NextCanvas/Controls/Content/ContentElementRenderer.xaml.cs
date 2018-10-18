using NextCanvas.ViewModels.Content;
using NextCanvas.Models.Content;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NextCanvas.Controls.Content
{
    /// <summary>
    /// Logique d'interaction pour ContentElementRenderer.xaml
    /// </summary>
    public partial class ContentElementRenderer : UserControl
    {
        public ContentElementRenderer()
        {
            InitializeComponent();
        }
        public ContentElementRenderer(object element) : this()
        {
            DataContext = element;
        }
    }
}
