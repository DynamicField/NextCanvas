using System.ComponentModel;

namespace NextCanvas.ViewModels
{
    public interface IViewModel<out T> : INotifyPropertyChanged
    {
        T Model { get; }
    }
}