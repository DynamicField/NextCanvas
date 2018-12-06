#region

using System;

#endregion

namespace NextCanvas.Controls
{
    public class SelectionWrapper
    {
        private readonly Action<object> _selectAction;

        public SelectionWrapper(Action<object> selectAction)
        {
            this._selectAction = selectAction;
        }

        public void Select(object item)
        {
            _selectAction(item);
        }
    }
}