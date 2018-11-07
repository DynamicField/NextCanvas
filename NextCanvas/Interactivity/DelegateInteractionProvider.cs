#region

using System;

#endregion

namespace NextCanvas.Interactivity
{
    public class DelegateInteractionProvider<T> : IInteractionProvider<T> where T : IInteractionBase
    {
        public DelegateInteractionProvider(Func<T> creator)
        {
            CreateMethod = creator;
        }

        private Func<T> CreateMethod { get; }

        public T CreateInteraction()
        {
            return CreateMethod();
        }
    }
}