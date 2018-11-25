
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using NextCanvas.Properties;
using NextCanvas.ViewModels;
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
            WebBrowserElementViewModel.SetHighestIEMode();
            if (SettingsManager.Settings.HasLanguage())
            {
                Thread.CurrentThread.CurrentUICulture = SettingsManager.Settings.PreferredLanguage;
            }
            AppDomain.CurrentDomain.UnhandledException += RestInPeperonies;
            Exit += (sender, args) => { SettingsManager.SaveSettings(); };
            SettingsViewModel.CultureChanged += SettingsViewModel_CultureChanged;
        }

        private void SettingsViewModel_CultureChanged(object sender, EventArgs e)
        {
            var windows = Current.Windows.OfType<Window>();
            var windowsArray = windows as Window[] ?? windows.ToArray();
            if (!windowsArray.Any()) return;
            var dataContexts = windowsArray.Select(w => new
            {
                w.DataContext,
                Window = w
            });
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            foreach (var window in windowsArray)
            {
                window.Close();
            }
            GC.Collect();
            foreach (var dataContext in dataContexts)
            {
                try
                {
                    var createWindow = (Window) Activator.CreateInstance(dataContext.Window.GetType());
                    createWindow.DataContext = dataContext.DataContext;
                    createWindow.Show();
                }
                catch { /* oops whatever */ }
            }
            Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            LogManager.AddLogItem("OnStartup started.", "Initialisation");
            base.OnStartup(e);
        }
        public static IEnumerable<CultureInfo> SupportedCultures => new[]
        {
            CultureInfo.GetCultureInfo("en-US"),
            CultureInfo.GetCultureInfo("fr-FR"), 
        };
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