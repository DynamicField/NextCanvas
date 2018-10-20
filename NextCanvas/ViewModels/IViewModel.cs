namespace NextCanvas.ViewModels
{
    public interface IViewModel<out T>
    {
        T Model { get; }
    }
}