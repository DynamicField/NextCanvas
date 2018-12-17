
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Threading;
using System.Windows;
using NextCanvas.Content.ViewModels;
using NextCanvas.Extensibility;
using NextCanvas.Utilities;
using NextCanvas.ViewModels;
using NextCanvas.Views;

namespace NextCanvas
{
    /// <inheritdoc />
    /// <summary>
    ///     Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static App GetCurrent()
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) // This is a trick to make XAML designer ok with the default app.
            {
                return new App();
            }
#endif
            return (App)Current;
        }

        public static string RootAddonsPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Addons");
        public static string RuntimePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Runtime");
        private const string JitProfileName = "JitProfile.profile";
        public App()
        {
            InitializeMultiCoreJit();
            LogManager.AddLogItem("Constructor app started :)", $"NextCanvas {Assembly.GetExecutingAssembly().GetName().Version}");
            WebBrowserElementViewModel.SetHighestIEMode();
            SetSettingsLanguage();
            AddUnhandledExceptionHandlers();
            Exit += (sender, args) => { SettingsManager.SaveSettings(); };
            SettingsViewModel.CultureChanged += SettingsViewModel_CultureChanged;
        }

        private static void InitializeMultiCoreJit()
        {
            Directory.CreateDirectory(RuntimePath);
            ProfileOptimization.SetProfileRoot(RuntimePath);
            ProfileOptimization.StartProfile(JitProfileName);
        }

        private void AddUnhandledExceptionHandlers()
        {
            AppDomain.CurrentDomain.UnhandledException += RestInPeperonies;
            DispatcherUnhandledException += (s, e) => RestInPeperonies(s, new UnhandledExceptionEventArgs(e.Exception, e.Handled));
        }

        private void SetAddons()
        {
            LogManager.AddLogItem("Loading the addons");
            Directory.CreateDirectory(RootAddonsPath);
            if (!Directory.EnumerateFiles(RootAddonsPath).Any())
            {
                LogManager.AddLogItem("No files in the Addons folder.");
                return;
            }
            Addons = Directory.EnumerateFiles(RootAddonsPath).Where(s => s.EndsWith("dll"))
                .Select(LoadAssemblySafe)
                .Where(a => !(a is null))
                .Select(CreateSafeAddonInfo)
                .Where(a => !(a is null))
                .ToArray();
            LogManager.AddLogItem($"Loaded {Addons.Length} addons.");
            foreach (var addon in Addons)
            {
                LogManager.AddLogItem($"...Addon: {addon.AddonInfoAttribute.Name}, Elements: {addon.ResolvedAddonElements.Length}");
                foreach (var element in addon.ResolvedAddonElements)
                {
                    LogManager.AddLogItem($"......Element: {element.Type.Name}");
                }
            }
        }
        public static List<string> ErrorQueue { get; } = new List<string>();
        private Assembly LoadAssemblySafe(string path)
        {
            try
            {
                return Assembly.ReflectionOnlyLoadFrom(path);
            }
            catch (Exception e)
            {
                ErrorQueue.Add(
                     $"We had a problem loading the library {Path.GetFileName(path)}. Please contact the developer of this addon or it can also be a bug other than that.\n{e.Message}");
                LogManager.AddLogItem($"Could not load assembly {path}. Exception: {e.Message}\nStack trace:\n{e.StackTrace}", status: LogEntryStatus.Error);
                return null;
            }
        }
        private AddonInfo CreateSafeAddonInfo(Assembly assembly)
        {
            try
            {
                return new AddonInfo(assembly, true);
            }
            catch (AddonInfoNotFoundException)
            {
                LogManager.AddLogItem($"No {nameof(AddonInfoAttribute)} found on the assembly {assembly.GetSimpleName()}.");
                return null;
            }
            catch (Exception e)
            {
                ErrorQueue.Add(
                    $"We had a problem loading the addon {assembly.GetSimpleName()}. Please contact the developer of this addon or it can also be a bug other than that.\n{e.Message}");
                LogManager.AddLogItem($"Could not load addon {assembly.FullName}. Exception: {e.Message}\nStack trace:\n{e.StackTrace}", status: LogEntryStatus.Error);
                return null;
            }
        }
        private static void SetSettingsLanguage()
        {
            if (SettingsManager.Settings.HasLanguage())
            {
                Thread.CurrentThread.CurrentUICulture = SettingsManager.Settings.PreferredLanguage;
            }
        }
        public AddonInfo[] Addons { get; private set; } = new AddonInfo[0];
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
                    var createWindow = (Window)Activator.CreateInstance(dataContext.Window.GetType());
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
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += CurrentDomain_ReflectionOnlyAssemblyResolve;
            SetAddons();
        }

        private Assembly CurrentDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var resolvedAssembly = Assembly.ReflectionOnlyLoad(args.Name);
            LogManager.AddLogItem($"Resolved reflection only assembly as requested by {args.RequestingAssembly.GetSimpleName()}: {resolvedAssembly.GetSimpleName()}", "ReflectionOnlyResolver");
            return resolvedAssembly;
        }

        public static IEnumerable<CultureInfo> SupportedCultures => new[]
        {
            CultureInfo.GetCultureInfo("en-US"),
            CultureInfo.GetCultureInfo("fr-FR"),
        };
        private static void RestInPeperonies(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (Exception)e.ExceptionObject;
            foreach (var window in Current.Windows.OfType<Window>())
                if (window is ScreenshotWindow || window.Topmost)
                    window.Close(); // It's top most and your computer will be LOCKED IF THIS THING ISNT CLOSED SO BETTER CLOSE IT.
            new ExceptionWindow(exception.ToString()).ShowDialog();
#if !DEBUG
            Environment.FailFast(exception.Message, exception);
#else
            Current.Shutdown(42);
#endif
        }
    }
}