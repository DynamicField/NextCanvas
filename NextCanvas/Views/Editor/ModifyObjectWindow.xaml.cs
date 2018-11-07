#region

using System;
using System.Windows;
using NextCanvas.Interactivity;

#endregion

namespace NextCanvas.Views.Editor
{
    /// <summary>
    /// Logique d'interaction pour ModifyObjectWindow.xaml
    /// </summary>
    public partial class ModifyObjectWindow : InteractionWindow, IModifyObjectInteraction
    {
        private ModifierData data = new ModifierData();
        public ModifyObjectWindow()
        {
            DataContext = data;
            InitializeComponent();
        }

        public ModifyObjectWindow(Window owner)
        {
            DataContext = data;
            InitializeComponent();
            Owner = owner;
        }

        public string HeaderText
        {
            get => data.HeaderText;
            set => data.HeaderText = value;
        }
        public string HeaderStart
        {
            get => data.HeaderStart;
            set => data.HeaderStart = value;
        }

    
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
        public object ObjectToModify
        {
            get => data.ObjectToModify;
            set => data.ObjectToModify = value;
        }

        private class ModifierData : PropertyChangedObject
        {
            private object objectToModify;

            public object ObjectToModify
            {
                get => objectToModify;
                set { objectToModify = value; OnPropertyChanged(nameof(ObjectToModify)); SetHeader(); }
            }
            private string headerText;
            private string _headerStart = "Modifying";

            public string HeaderText
            {
                get => headerText;
                set { headerText = value; OnPropertyChanged(nameof(HeaderText)); }
            }
            public string HeaderStart
            {
                get => _headerStart;
                set { _headerStart = value; SetHeader(); }
            }
            private void SetHeader()
            {
                if (objectToModify is INamedObject named)
                {
                    HeaderText = $"{HeaderStart} {named.Name}";
                }
            }
        }
    }
}
