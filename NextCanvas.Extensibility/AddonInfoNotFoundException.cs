using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NextCanvas.Extensibility
{
    public class AddonInfoNotFoundException : Exception
    {
        public AddonInfoNotFoundException()
        {
        }

        public AddonInfoNotFoundException(string message) : base(message)
        {
        }

        public AddonInfoNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AddonInfoNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
