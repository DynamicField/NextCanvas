
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using NextCanvas.ViewModels.Content;
using NextCanvas.Views;

namespace NextCanvas
{
    /// <inheritdoc />
    /// <summary>
    ///     Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            LogManager.AddLogItem("Constructor app started :)", $"NextCanvas {Assembly.GetExecutingAssembly().GetName().Version}");
            // TODO: Add localization.
            WebBrowserElementViewModel.SetHighestIEMode();
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            AppDomain.CurrentDomain.UnhandledException += RestInPeperonies;
            Exit += (sender, args) => { SettingsManager.SaveSettings(); };
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            LogManager.AddCustomLogItem("OnStartup started.", "Initialisation");
            base.OnStartup(e);
        }

        private static void RestInPeperonies(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (Exception) e.ExceptionObject;
            foreach (var window in Current.Windows)
                if (window is ScreenshotWindow win)
                    win.CloseInteraction(); // It's top most and your computer will be LOCKED IF THIS THING ISNT CLOSED SO BETTER CLOSE IT.
            new ExceptionWindow(exception.ToString()).ShowDialog();
#if !DEBUG
            Environment.FailFast(exception.Message, exception);
#else
            Current.Shutdown(42);
#endif
        }
    }
}