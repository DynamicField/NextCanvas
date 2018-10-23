using System.ComponentModel;

namespace NextCanvas.Interactivity
{
    public interface IProgressInteraction : IInteractionBase
    {
        double Progress { get; set; }
        string ProgressText { get; set; }
    }
}