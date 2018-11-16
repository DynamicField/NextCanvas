#region

using System;
using System.Text;
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
        private readonly ModifierData data = new ModifierData();
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

        public bool IsObjectCreation
        {
            get => data.IsObjectCreation;
            set => data.IsObjectCreation = value;
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
            private string _headerStart = "Editing";

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
            private bool isCreate;

            public bool IsObjectCreation
            {
                get => isCreate;
                set { isCreate = value; OnPropertyChanged(nameof(IsObjectCreation)); OnPropertyChanged(nameof(IsObjectCreationReversed));}
            }

            public bool IsObjectCreationReversed => !IsObjectCreation;
            private void SetHeader()
            {
                if (objectToModify is INamedObject named)
                {
                    var processed = named.Name;
                    var stringBuilder = new StringBuilder(processed);
                    stringBuilder[0] = char.ToLower(stringBuilder[0]);
                    HeaderText = $"{HeaderStart} {stringBuilder}";
                }
                else
                {
                    HeaderText = $"{HeaderStart} object...";
                }
            }
        }
    }
}
