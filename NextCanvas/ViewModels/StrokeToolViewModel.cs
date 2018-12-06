using System;
using System.Windows.Ink;
using NextCanvas;

namespace NextCanvas.ViewModels
{
    public class StrokeToolViewModel : ToolViewModel
    {
        // No parameter-less constructor.
        public StrokeToolViewModel(StrokeTool model, Uri icon = null) : base(model, icon)
        {

        }

        public Type StrokeType => Model.StrokeType;
        public StrokeDelegate<Stroke> StrokeConstructor => Model.StrokeConstructor;

        protected override Tool BuildDefaultModel()
        {
            throw new InvalidOperationException("Cannot create a new StrokeToolViewModel without any model.");
        }

        public new StrokeTool Model => (StrokeTool) base.Model;
    }
}
