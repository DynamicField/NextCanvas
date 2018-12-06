#region

using System;
using System.Text;
using System.Windows;
using NextCanvas.Interactivity;
using NextCanvas.Properties;

#endregion

namespace NextCanvas.Views.Editor
{
    /// <summary>
    /// Logique d'interaction pour ModifyObjectWindow.xaml
    /// </summary>
    public partial class ModifyObjectWindow : InteractionWindow, IModifyObjectInteraction
    {
        private readonly ModifierData _data = new ModifierData();
        public ModifyObjectWindow()
        {
            DataContext = _data;
            InitializeComponent();
        }

        public ModifyObjectWindow(Window owner)
        {
            DataContext = _data;
            InitializeComponent();
            Owner = owner;
        }

        public string HeaderText
        {
            get => _data.HeaderText;
            set => _data.HeaderText = value;
        }
        public string HeaderStart
        {
            get => _data.HeaderStart;
            set => _data.HeaderStart = value;
        }

        public bool IsObjectCreation
        {
            get => _data.IsObjectCreation;
            set => _data.IsObjectCreation = value;
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
            get => _data.ObjectToModify;
            set => _data.ObjectToModify = value;
        }

        private class ModifierData : PropertyChangedObject
        {
            private object _objectToModify;

            public object ObjectToModify
            {
                get => _objectToModify;
                set { _objectToModify = value; OnPropertyChanged(nameof(ObjectToModify)); SetHeader(); }
            }
            private string _headerText;
            private string _headerStart = EditorResources.ModifyObject_DefaultHeaderStart;
            public string HeaderText
            {
                get => _headerText;
                set { _headerText = value; OnPropertyChanged(nameof(HeaderText)); }
            }
            public string HeaderStart
            {
                get => _headerStart;
                set { _headerStart = value; SetHeader(); }
            }
            private bool _isCreate;

            public bool IsObjectCreation
            {
                get => _isCreate;
                set { _isCreate = value; OnPropertyChanged(nameof(IsObjectCreation)); OnPropertyChanged(nameof(IsObjectCreationReversed));}
            }

            public bool IsObjectCreationReversed => !IsObjectCreation;
            private void SetHeader()
            {
                if (_objectToModify is INamedObject named)
                {
                    var processed = named.Name;
                    var stringBuilder = new StringBuilder(processed);
                    stringBuilder[0] = char.ToLower(stringBuilder[0]);
                    HeaderText = $"{HeaderStart} {stringBuilder}";
                }
                else
                {
                    HeaderText = string.Format(EditorResources.ModifyObject_DefaultHeaderText, HeaderStart);
                }
            }
        }
    }
}
