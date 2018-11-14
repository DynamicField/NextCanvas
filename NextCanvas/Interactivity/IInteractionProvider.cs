using System;
using System.Collections.Generic;

namespace NextCanvas.Interactivity
{
    public interface IInteractionProvider<out T> where T : IInteractionBase
    {
        T CreateInteraction();
    }

    public interface IMultiInteractionProvider
    {
        T CreateInteraction<T>() where T : class, IInteractionBase;
        IEnumerable<Type> AvailableInteractions { get; }
    }
}