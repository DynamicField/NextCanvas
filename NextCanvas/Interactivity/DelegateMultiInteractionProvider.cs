using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextCanvas.Interactivity
{
    public class DelegateMultiInteractionProvider : IMultiInteractionProvider, IInteractionProvider<IInteractionBase>
    {
        public DelegateMultiInteractionProvider(IEnumerable<Func<IInteractionBase>> functions)
        {
            FunctionsList.AddRange(functions);
        }
        public List<Func<IInteractionBase>> FunctionsList { get; } = new List<Func<IInteractionBase>>();
        public T CreateInteraction<T>() where T : class, IInteractionBase
        {
            return FunctionsList.OfType<Func<T>>().FirstOrDefault()?.Invoke();
        }
        public IEnumerable<Type> AvailableInteractions
        {
            get
            {
                foreach (var function in FunctionsList)
                {
                    var type = function.GetType().GetGenericArguments()[0];
                    yield return type;
                }
            }
        }

        public virtual IInteractionBase CreateInteraction() => CreateInteraction<IInteractionBase>();
    }

    public class DelegateMultiInteractionProvider<TDefault> : DelegateMultiInteractionProvider,
        IInteractionProvider<TDefault> where TDefault : class, IInteractionBase
    {
        public DelegateMultiInteractionProvider(IEnumerable<Func<IInteractionBase>> functions) : base(functions)
        {
        }

        public override IInteractionBase CreateInteraction() => CreateInteraction<TDefault>();
        TDefault IInteractionProvider<TDefault>.CreateInteraction() => CreateInteraction<TDefault>();
    }
}
