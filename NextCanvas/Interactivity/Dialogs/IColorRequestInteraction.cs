using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NextCanvas.Interactivity.Dialogs
{
    public interface IColorRequestInteraction : IUserInteraction<ColorRequestEventArgs>
    {

    }
    public class ColorRequestEventArgs : EventArgs
    {
        public Color Color { get; }

        public ColorRequestEventArgs(Color c)
        {
            Color = c;
        }
    }
}
