using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextCanvas.Content;
using NextCanvas.Content.ViewModels;
using NextCanvas.Extensibility.Content;

namespace TestAddon.ViewModels
{
    [ContentAddonElement(Name = "Hello world!")]
    public class HelloWorldElementViewModel : ContentElementAddon
    {
        public HelloWorldElementViewModel() { }

        public HelloWorldElementViewModel(ContentElementAddonModel m = null) : base(m)
        {
            Fun++;
        }
        public int Fun
        {
            get => GetProperty<int>();
            set => SetProperty(value);
        }
    }
}
