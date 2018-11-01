using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace NextCanvas
{
    public abstract class PropertyChangedObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        internal static Dictionary<Thread, SynchronizationContext> Contexts { get; set; } = new Dictionary<Thread, SynchronizationContext>();
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
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
                    // Wait synchronously for the property to change.
                    // TODO? : Probably replace this with a batch of property changed for better performance.
                    context.Value.Send(_ => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)),
                        null);
                }
            }          
        }
    }
}