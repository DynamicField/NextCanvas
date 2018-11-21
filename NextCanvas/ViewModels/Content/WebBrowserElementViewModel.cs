#region

using Microsoft.Win32;
using NextCanvas.Models.Content;
using NextCanvas.Properties;
using NextCanvas.Utilities;

#endregion

namespace NextCanvas.ViewModels.Content
{
    public class WebBrowserElementViewModel : ContentElementViewModel, INamedObject
    {
        public WebBrowserElementViewModel(WebBrowserElement model = null) : base(model)
        {

        }
        public WebBrowserElementViewModel() : this(null) { }
        public static void SetHighestIEMode()
        {
            var appName = System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe";
            using (var registryKey =
                Registry.CurrentUser.CreateSubKey(
                    @"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION"))
            {
                registryKey?.SetValue(appName, 42000, RegistryValueKind.DWord);
            }
        }

        public string Url
        {
            get => Model.Url;
            set
            {
                Model.Url = value;
                OnPropertyChanged(nameof(Url));
            }
        }

        public override double Top
        {
            get => base.Top;
            set => base.Top = value.Cap(0);
        }

        public new WebBrowserElement Model => (WebBrowserElement)base.Model;
        protected override ContentElement BuildDefaultModel()
        {
            return new WebBrowserElement { Width = 800, Height = 600 };
        }

        public string Name => DefaultObjectNamesResources.WebBrowser;
    }
}