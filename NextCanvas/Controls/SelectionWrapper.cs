using System;

namespace NextCanvas.Controls
{
    public class SelectionWrapper
    {
        private readonly Action<object> selectAction;
        public void Select(object item)
        {
            selectAction(item);
        }
        public SelectionWrapper(Action<object> selectAction)
        {
            this.selectAction = selectAction;
        }
    }
}
