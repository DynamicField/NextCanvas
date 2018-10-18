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
            InitializePropertiesBindings();
        }
        // Why do I do that ?
        // It's because for some reason the ink canvas just delete the binding expression idk why :(
        private void InitializePropertiesBindings()
        {
            DependencyPropertyDescriptor.FromProperty(InkCanvas.TopProperty, typeof(ContentElementRenderer)).AddValueChanged(this, (s, e) =>
            {
                if (this.GetBindingExpression(InkCanvas.TopProperty) != null && (double)GetValue(InkCanvas.TopProperty) != double.NaN)
                {
                    return;
                }
                (DataContext as dynamic).Top = (double)GetValue(InkCanvas.TopProperty);
                var otherBinding = new Binding("Top") { Source = DataContext, Mode = BindingMode.TwoWay};
                this.SetBinding(InkCanvas.TopProperty, otherBinding); // Update then
            });
            DependencyPropertyDescriptor.FromProperty(InkCanvas.BottomProperty, typeof(ContentElementRenderer)).AddValueChanged(this, (s, e) =>
            {
                if (this.GetBindingExpression(InkCanvas.BottomProperty) != null && (double)GetValue(InkCanvas.BottomProperty) != double.NaN)
                {
                    return;
                }
                (DataContext as dynamic).Bottom = (double)GetValue(InkCanvas.BottomProperty);
                var otherBinding = new Binding("Bottom") { Source = DataContext, Mode = BindingMode.TwoWay };
                this.SetBinding(InkCanvas.BottomProperty, otherBinding); // Update then
            });
            DependencyPropertyDescriptor.FromProperty(InkCanvas.RightProperty, typeof(ContentElementRenderer)).AddValueChanged(this, (s, e) =>
            {
                if (this.GetBindingExpression(InkCanvas.RightProperty) != null && (double)GetValue(InkCanvas.RightProperty) != double.NaN)
                {
                    return;
                }
                (DataContext as dynamic).Right = (double)GetValue(InkCanvas.RightProperty);
                var otherBinding = new Binding("Right") { Source = DataContext, Mode = BindingMode.TwoWay };
                this.SetBinding(InkCanvas.RightProperty, otherBinding); // Update then
            });
            DependencyPropertyDescriptor.FromProperty(InkCanvas.LeftProperty, typeof(ContentElementRenderer)).AddValueChanged(this, (s, e) =>
            {
                if (this.GetBindingExpression(InkCanvas.LeftProperty) != null && (double)GetValue(InkCanvas.LeftProperty) != double.NaN)
                {
                    return;
                }
                (DataContext as dynamic).Left = (double)GetValue(InkCanvas.LeftProperty);
                var otherBinding = new Binding("Left") { Source = DataContext, Mode = BindingMode.TwoWay };
                this.SetBinding(InkCanvas.LeftProperty, otherBinding); // Update then
            });
            DependencyPropertyDescriptor.FromProperty(WidthProperty, typeof(ContentElementRenderer)).AddValueChanged(this, (s, e) =>
            {
                if (this.GetBindingExpression(WidthProperty) != null)
                {
                    return;
                }
                (DataContext as dynamic).Width = Width;
                var otherBinding = new Binding("Width") { Source = DataContext, Mode = BindingMode.TwoWay };
                this.SetBinding(WidthProperty, otherBinding); // Update then
            });
            DependencyPropertyDescriptor.FromProperty(HeightProperty, typeof(ContentElementRenderer)).AddValueChanged(this, (s, e) =>
            {
                if (this.GetBindingExpression(HeightProperty) != null)
                {
                    return;
                }
                (DataContext as dynamic).Height = Height;
                var otherBinding = new Binding("Height") { Source = DataContext, Mode = BindingMode.TwoWay };
                this.SetBinding(HeightProperty, otherBinding); // Update then
            });
        }
        public ContentElementRenderer(object element) : this()
        {
            DataContext = element;
        }
    }
}
