using System.Collections.Generic;
using Newtonsoft.Json;
using NextCanvas.Models.Content;

namespace NextCanvas.Models
{
    public class Document
    {
        public List<Page> Pages { get; set; } = new List<Page>();
        public List<Resource> Resources { get; set; } = new List<Resource>();

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