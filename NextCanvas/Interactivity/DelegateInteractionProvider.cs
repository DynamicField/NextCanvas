using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextCanvas.Interactivity
{
    public class DelegateInteractionProvider<T> : IInteractionProvider<T> where T : IInteractionBase
    {
        private Func<T> Create { get; }

        public DelegateInteractionProvider(Func<T> creator)
        {
            Create = creator;
        }
        public T CreateInteraction()
        {
            return Create();
        }
    }
}
