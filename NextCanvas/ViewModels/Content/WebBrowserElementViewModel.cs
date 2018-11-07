#region

using Microsoft.Win32;
using NextCanvas.Models.Content;

#endregion

namespace NextCanvas.ViewModels.Content
{
    public class WebBrowserElementViewModel : ContentElementViewModel
    {
        public WebBrowserElementViewModel()
        {

        }

        public WebBrowserElementViewModel(WebBrowserElement model) : base(model)
        {

        }

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

        public new WebBrowserElement Model => (WebBrowserElement)base.Model;
        protected override ContentElement BuildDefaultModel()
        {
            return new WebBrowserElement { Width = 800, Height = 600 };
        }
    }
}