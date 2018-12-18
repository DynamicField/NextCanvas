#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using SynchronizationContext = System.Threading.SynchronizationContext;

#endregion

namespace NextCanvas
{
    public abstract class PropertyChangedObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private static List<WindowSyncData> Contexts { get; } = new List<WindowSyncData>();
        private static readonly object IsChanging = new object();
        protected virtual bool IsSync => false;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            lock (IsChanging)
            {
                try
                {
                    foreach (var context in Contexts.OrderByDescending(s => s.Dispatcher != Dispatcher.CurrentDispatcher ? 1 : 0))
                    {
                        if (context.Dispatcher == Dispatcher.CurrentDispatcher)
                        {
                            // Called on the same thread as the sync. No need to use the context.
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                        }
                        else
                        {
                            if (IsSync && context.Window.IsLoaded)
                            {
                                context.Context.Send(
                                    _ => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)),
                                    null);
                            }
                            else
                            {
                                context.Context.Post(
                                    _ => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)),
                                    null);
                            }
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Weird propertychanged exception: " + e);
                }
            }
        }

        public static void RegisterWindow(Window window)
        {
            lock (IsChanging)
            {
                Contexts.Add(new WindowSyncData(window));
            }
            window.Closed += RegisteredWindowClosed;
        }

        private static void RegisteredWindowClosed(object sender, System.EventArgs e)
        {
            lock (IsChanging)
            {
                Contexts.Remove(Contexts.First(w => w.Window == sender));
            }
            ((Window)sender).Closed -= RegisteredWindowClosed;
        }

        private class WindowSyncData
        {
            public SynchronizationContext Context { get; }
            public Window Window { get; }
            public Dispatcher Dispatcher { get; }

            public WindowSyncData(Window window, SynchronizationContext context = null)
            {
                Window = window;
                Context = context ?? SynchronizationContext.Current;
                Dispatcher = Window.Dispatcher;
            }
        }
    }
}