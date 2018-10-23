using System;
using System.Globalization;
using System.Threading;
using System.Windows;
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
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            AppDomain.CurrentDomain.UnhandledException += RestInPeperonies;
        }

        private static void RestInPeperonies(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (Exception) e.ExceptionObject;
            new ExceptionWindow(exception.ToString()).ShowDialog();
            Current.Shutdown(1);
        }
    }
}