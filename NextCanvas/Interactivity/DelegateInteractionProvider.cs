using System;

namespace NextCanvas.Interactivity
{
    public class DelegateInteractionProvider<T> : IInteractionProvider<T> where T : IInteractionBase
    {
        public DelegateInteractionProvider(Func<T> creator)
        {
            Create = creator;
        }

        private Func<T> Create { get; }

        public T CreateInteraction()
        {
            return Create();
        }
    }
}