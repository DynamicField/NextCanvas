using System;

namespace NextCanvas.Controls
{
    public class SelectionWrapper
    {
        private readonly Action<object> selectAction;

        public SelectionWrapper(Action<object> selectAction)
        {
            this.selectAction = selectAction;
        }

        public void Select(object item)
        {
            selectAction(item);
        }
    }
}