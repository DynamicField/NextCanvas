using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextCanvas.Content;
using NextCanvas.Extensibility.Content;
using TestAddon.ViewModels;

namespace TestAddon
{
    [ContentAddonElement(typeof(HelloWorldElementViewModel), Name = "Hello world !")]
    public class HelloWorldElement : ContentElement
    {
        public int Fun { get; set; } = 1;
    }
}
