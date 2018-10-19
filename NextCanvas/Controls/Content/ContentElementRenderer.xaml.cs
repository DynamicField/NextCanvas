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
using System.ComponentModel;

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
        public void Initialize(ContentElementViewModel v)
        {
            DataContext = v;
            InitializePropertiesBindings();
        }
        // Why do I do that ?
        // It's because for some reason the ink canvas just delete the binding expression idk why :(
        // This kind of a "hack" works but
        // FIND A BETTER SOLUTION PLS
        private void InitializePropertiesBindings()
        {
            var data = DataContext as ContentElementViewModel;
            if (data == null)
            {
                return; // why isnt it a contentelement blah blah?
            }
            DependencyPropertyDescriptor.FromProperty(InkCanvas.TopProperty, typeof(ContentElementRenderer)).AddValueChanged(this, (s, e) =>
            {
                if (GetBindingExpression(InkCanvas.TopProperty) != null && (double)GetValue(InkCanvas.TopProperty) != double.NaN)
                {
                    return;
                }
                data.Top = (double)GetValue(InkCanvas.TopProperty);
                var otherBinding = new Binding("Top") { Source = DataContext, Mode = BindingMode.TwoWay };
                SetBinding(InkCanvas.TopProperty, otherBinding); // Update then
            });
            DependencyPropertyDescriptor.FromProperty(InkCanvas.BottomProperty, typeof(ContentElementRenderer)).AddValueChanged(this, (s, e) =>
            {
                if (GetBindingExpression(InkCanvas.BottomProperty) != null && (double)GetValue(InkCanvas.BottomProperty) != double.NaN)
                {
                    return;
                }
                data.Bottom = (double)GetValue(InkCanvas.BottomProperty);
                var otherBinding = new Binding("Bottom") { Source = DataContext, Mode = BindingMode.TwoWay };
                SetBinding(InkCanvas.BottomProperty, otherBinding); // Update then
            });
            DependencyPropertyDescriptor.FromProperty(InkCanvas.RightProperty, typeof(ContentElementRenderer)).AddValueChanged(this, (s, e) =>
            {
                if (GetBindingExpression(InkCanvas.RightProperty) != null && (double)GetValue(InkCanvas.RightProperty) != double.NaN)
                {
                    return;
                }
                data.Right = (double)GetValue(InkCanvas.RightProperty);
                var otherBinding = new Binding("Right") { Source = DataContext, Mode = BindingMode.TwoWay };
                SetBinding(InkCanvas.RightProperty, otherBinding); // Update then
            });
            DependencyPropertyDescriptor.FromProperty(InkCanvas.LeftProperty, typeof(ContentElementRenderer)).AddValueChanged(this, (s, e) =>
            {
                if (GetBindingExpression(InkCanvas.LeftProperty) != null && (double)GetValue(InkCanvas.LeftProperty) != double.NaN)
                {
                    return;
                }
                data.Left = (double)GetValue(InkCanvas.LeftProperty);
                var otherBinding = new Binding("Left") { Source = DataContext, Mode = BindingMode.TwoWay };
                SetBinding(InkCanvas.LeftProperty, otherBinding); // Update then
            });
            DependencyPropertyDescriptor.FromProperty(WidthProperty, typeof(ContentElementRenderer)).AddValueChanged(this, (s, e) =>
            {
                if (GetBindingExpression(WidthProperty) != null)
                {
                    return;
                }
                data.Width = Width;
                var otherBinding = new Binding("Width") { Source = DataContext, Mode = BindingMode.TwoWay };
                SetBinding(WidthProperty, otherBinding); // Update then
            });
            DependencyPropertyDescriptor.FromProperty(HeightProperty, typeof(ContentElementRenderer)).AddValueChanged(this, (s, e) =>
            {
                if (GetBindingExpression(HeightProperty) != null)
                {
                    return;
                }
                data.Height = Height;
                var otherBinding = new Binding("Height") { Source = DataContext, Mode = BindingMode.TwoWay };
                SetBinding(HeightProperty, otherBinding); // Update then
            });
        }
    }
}
