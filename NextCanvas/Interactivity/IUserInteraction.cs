using System;

namespace NextCanvas.Interactivity
{
    public interface IUserInteraction : IUserInteraction<EventArgs>
    {
    }

    public interface IUserInteraction<T> : IInteractionBase where T : EventArgs
    {
        event EventHandler<T> ActionComplete;
        event EventHandler ActionCanceled;
    }
}