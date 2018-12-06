using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextCanvas.ViewModels
{
    public class SettingsWindowViewModel : PropertyChangedObject
    {
        public SettingsViewModel Settings => SettingsManager.Settings;
        public DelegateCommand MoveUpCommand { get; private set; }
        public DelegateCommand MoveDownCommand { get; private set; }

        public SettingsWindowViewModel()
        {
            MoveUpCommand = new DelegateCommand(MoveUp, CanMoveUp);
            MoveDownCommand = new DelegateCommand(MoveDown, CanMoveDown);
            Settings.Tools.CollectionChanged += (sender, args) =>
            {
                MoveUpCommand.RaiseCanExecuteChanged();
                MoveDownCommand.RaiseCanExecuteChanged();
            };
        }
        private int _selectedToolIndex;
        public int SelectedToolIndex
        {
            get { return _selectedToolIndex; }
            set { _selectedToolIndex = value; OnPropertyChanged(nameof(SelectedToolIndex)); }
        }
        private void MoveUp(object t)
        {
            if (!(t is ToolViewModel tool)) return;
            MoveTool(tool, -1);
        }
        private void MoveDown(object t)
        {
            if (!(t is ToolViewModel tool)) return;
            MoveTool(tool, 1);
        }
        private bool CanMove(ToolViewModel t, int destination)
        {
            var index = Settings.Tools.IndexOf(t) + destination;
            var result = index >= 0 && index < Settings.Tools.Count;
            return result;
        }
        private void MoveTool(ToolViewModel t, int destination)
        {
            if (!CanMove(t, destination)) return;
            var index = Settings.Tools.IndexOf(t);
            Settings.Tools.Move(index, index + destination);
        }

        private bool CanMoveUp(object t) => CanMove((ToolViewModel)t, -1);
        private bool CanMoveDown(object t) => CanMove((ToolViewModel)t, 1);
    }
}
