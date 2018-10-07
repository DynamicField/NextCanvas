using NextCanvas.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NextCanvas
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            AppDomain.CurrentDomain.UnhandledException += RestInPeperonies;
        }

        private void RestInPeperonies(object sender, UnhandledExceptionEventArgs e)

        {
            var exception = e.ExceptionObject as Exception;
            new ExceptionWindow(exception.ToString()).ShowDialog();
            Application.Current.Shutdown(1);
        }
    }
}
