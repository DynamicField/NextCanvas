using System.Threading.Tasks;

namespace NextCanvas.Interactivity
{
    public interface IInteractionBase
    {
        Task ShowAsync();
        Task CloseAsync();
    }
}