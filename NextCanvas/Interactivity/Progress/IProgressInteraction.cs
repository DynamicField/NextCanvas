namespace NextCanvas.Interactivity.Progress
{
    public interface IProgressInteraction : IInteractionBase
    {
        IProgressData Data { get; }
    }
}