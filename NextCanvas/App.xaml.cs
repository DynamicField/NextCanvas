
using System;
using System.Globalization;
using System.Threading;
using System.Windows;
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
            // TODO: Add localization.
            WebBrowserElementViewModel.SetHighestIEMode();
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            AppDomain.CurrentDomain.UnhandledException += RestInPeperonies;
            Current.Exit += (sender, args) => { SettingsManager.SaveSettings(); };
        }

        private static void RestInPeperonies(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (Exception) e.ExceptionObject;
            foreach (var window in Current.Windows)
                if (window is ScreenshotWindow win)
                    win.CloseInteraction(); // It's top most and your computer will be LOCKED IF THIS THING ISNT CLOSED SO BETTER CLOSE IT.
            new ExceptionWindow(exception.ToString()).ShowDialog();
            Environment.FailFast(exception.Message, exception);
        }
    }
}