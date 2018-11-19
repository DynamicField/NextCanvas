#region

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;

#endregion

namespace NextCanvas
{
    public abstract class PropertyChangedObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        internal static Dictionary<Thread, SynchronizationContext> Contexts { get; } = new Dictionary<Thread, SynchronizationContext>();
        private static readonly object isChanging = new object();
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            lock (isChanging)
            {
                foreach (var context in Contexts)
                {
                    if (Thread.CurrentThread == context.Key)
                    {
                        // Called on the same thread as the sync. No need to use the context.
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                    }
                    else
                    {
                        context.Value.Post(
                            _ => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)),
                            null);
                    }
                }
            }
        }
        
        public static void RegisterWindow(Window window)
        {
            if (!Contexts.ContainsKey(Thread.CurrentThread))
            {
                lock (isChanging)
                {
                    Contexts.Add(Thread.CurrentThread, SynchronizationContext.Current);
                }
                window.Closed += RegisteredWindowClosed;
            }
        }

        private static void RegisteredWindowClosed(object sender, System.EventArgs e)
        {
            lock (isChanging)
            {
                Contexts.Remove(Thread.CurrentThread);
            }
            ((Window)sender).Closed -= RegisteredWindowClosed;
        }
    }
}