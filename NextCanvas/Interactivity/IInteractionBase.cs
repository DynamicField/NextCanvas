using System.Threading.Tasks;

namespace NextCanvas.Interactivity
{
    public interface IInteractionBase
    {
        bool IsClosed { get; }
        Task Show();
        Task Close();
    }
}