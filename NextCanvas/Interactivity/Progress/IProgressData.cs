#region

using System.ComponentModel;

#endregion

namespace NextCanvas.Interactivity.Progress
{
    public interface IProgressData : INotifyPropertyChanged
    {
        double Progress { get; set; }
        string ProgressText { get; set; }
    }
}