using NextCanvas.Interactivity;
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

namespace NextCanvas.Views.Editor
{
    /// <summary>
    /// Logique d'interaction pour ModifyObjectWindow.xaml
    /// </summary>
    public partial class ModifyObjectWindow : InteractionWindow, IModifyObjectInteraction
    {
        public ModifyObjectWindow()
        {
            InitializeComponent();
        }

        public ModifyObjectWindow(Window owner)
        {
            InitializeComponent();
            Owner = owner;
        }

        private void SetHeader(object data)
        {
            if (data is INamedObject named)
            {
                HeaderText = $"Modifying {named.Name}";
            }
        }

        public string HeaderText
        {
            get => (string)GetValue(HeaderTextProperty);
            set => SetValue(HeaderTextProperty, value);
        }

        // Using a DependencyProperty as the backing store for HeaderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register("HeaderText", typeof(string), typeof(ModifyObjectWindow), new FrameworkPropertyMetadata("Modifying object..."));


        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            ActionComplete?.Invoke(this, EventArgs.Empty);
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ActionCanceled?.Invoke(this, EventArgs.Empty);
            Close();
        }

        public event EventHandler<EventArgs> ActionComplete;
        public event EventHandler ActionCanceled;
        private object objectToModify = new object();
        public object ObjectToModify
        {
            get => objectToModify;
            set => Dispatcher.Invoke(() => { DataContext = objectToModify = value; SetHeader(value); });
        }
    }
}
