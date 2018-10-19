using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextCanvas.ViewModels;
using NextCanvas.ViewModels.Content;

namespace NextCanvas.Models.Content
{
    public class ContentElement
    {
        public double Left { get; set; } = 0;
        public double Top { get; set; } = 0;
        public double Bottom { get; set; } = 0;
        public double Right { get; set; } = 0;

        public double Width { get; set; } = 200;
        public double Height { get; set; } = 200;

    }
}
