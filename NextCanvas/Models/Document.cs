using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextCanvas.Models
{
    public class Document
    {
        public List<Page> Pages { get; set; } = new List<Page>();
        public Document()
        {
            Pages.Add(new Page());
        }
    }
}
