using System.Collections.Generic;
using Newtonsoft.Json;

namespace NextCanvas.Models
{
    public class Document
    {
        public List<Page> Pages { get; set; } = new List<Page>();
        public Document()
        {
            Pages.Add(new Page());
        }
        [JsonConstructor]
        public Document(List<Page> pages)
        {
            Pages = pages;
        }
    }
}
