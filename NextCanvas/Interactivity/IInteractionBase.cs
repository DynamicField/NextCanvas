using System.ComponentModel;

namespace NextCanvas.Interactivity
{
    public interface IInteractionBase
    {
        void Show();
        void Close();
        bool IsClosed { get; }
    }
}