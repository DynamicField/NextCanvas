using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using NextCanvas.Content.ViewModels;

namespace NextCanvas.Extensibility.Content
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ContentAddonElementAttribute : AddonElementAttribute
    {
        public ContentAddonElementAttribute(object icon = null, Type viewModelType = null)
        {
            ViewModelType = viewModelType;
            if (!viewModelType?.IsSubclassOf(typeof(ContentElementViewModel)) ?? false)
            {
                throw new InvalidOperationException("The view model type is not a ContentElementViewModel.");
            }
            Icon = icon;
            if (Icon is string s)
            {
                //var assembly = iconAssembly.Assembly;
                //Stream str = null;
                //foreach (var name in assembly.GetManifestResourceNames())
                //{
                //    if (name.Contains(s))
                //    {
                //        str = assembly.GetManifestResourceStream(name);
                //        break;
                //    }
                //}
                //if (str is null) return;
                //var m = new MemoryStream();
                //str.CopyTo(m);
                //str = m;
                //var image = new BitmapImage();
                //image.BeginInit();
                //image.StreamSource = str;
                //image.CacheOption = BitmapCacheOption.OnLoad;
                //image.EndInit();
                //image.Freeze();
                //Icon = image;
                Icon = new Uri(s);
            }
        }
        public Type ViewModelType { get; }
        public string Name { get; set; }
        public object Icon { get; }
    }
}
