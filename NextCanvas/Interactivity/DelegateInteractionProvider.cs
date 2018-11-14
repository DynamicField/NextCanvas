#region

using System;
using System.Collections.Generic;

#endregion

namespace NextCanvas.Interactivity
{
    public class DelegateInteractionProvider<T> : IInteractionProvider<T>, IMultiInteractionProvider where T : class, IInteractionBase
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

        public TInteraction CreateInteraction<TInteraction>() where TInteraction : class, IInteractionBase
        {
            var value = CreateMethod() as TInteraction;
            return value;
        }

        public IEnumerable<Type> AvailableInteractions => new[] {typeof(T)};
    }
}