namespace NextCanvas.Interactivity
{
    public interface IInteractionProvider<out T> where T : IInteractionBase
    {
        T CreateInteraction();
    }
}