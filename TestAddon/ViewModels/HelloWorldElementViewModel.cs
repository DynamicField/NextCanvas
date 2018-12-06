using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextCanvas.Content;
using NextCanvas.Content.ViewModels;

namespace TestAddon.ViewModels
{
    public class HelloWorldElementViewModel : ContentElementViewModel
    {
        public HelloWorldElementViewModel(HelloWorldElement model = null) : base(model)
        {
            Fun++;
        }
        public HelloWorldElementViewModel() : base() { }
        protected override ContentElement BuildDefaultModel() => new HelloWorldElement();
        public new HelloWorldElement Model => (HelloWorldElement)base.Model;

        public int Fun
        {
            get => Model.Fun;
            set
            {
                Model.Fun = value;
                OnPropertyChanged(nameof(Fun));
            }
        }
    }
}
