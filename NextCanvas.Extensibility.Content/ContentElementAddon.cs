using NextCanvas.Content;
using NextCanvas.Content.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NextCanvas.Extensibility.Content
{
    /// <summary>
    /// Class used to serve as a ContentElement, being a serializable view model
    /// </summary>
    public class ContentElementAddon : ContentElementViewModel
    {
        public ContentElementAddon() : this(null)
        {
        }

        public ContentElementAddon(ContentElement model = null) : base(model)
        {
        }

        public static ContentElementAddon CreateFromModel(Type type, object model = null)
        {
            if (!type.IsSubclassOf(typeof(ContentElementAddon)))
            {
                throw new InvalidOperationException($"Invalid type. Expected ContentElementAddon. Got {type.Name}");
            }
            var hasNoParameterLess = !type.GetConstructors().Any(c => c.GetParameters().All(p => p.IsOptional));
            if (model is null && hasNoParameterLess)
            {
                return (ContentElementAddon)Activator.CreateInstance(type);
            }
            if (type.GetConstructors()
                .Any(c => c.GetParameters().FirstOrDefault()?.ParameterType.IsInstanceOfType(model) ?? false))
            {
                return (ContentElementAddon)Activator.CreateInstance(type, model);
            }

            if (!hasNoParameterLess)
            {
                var ctor = type.GetConstructors().OrderByDescending(c => c.GetParameters().Length).FirstOrDefault(c => c.GetParameters().All(p => p.IsOptional));
                var list = new List<object>();
                for (int i = 0; i < ctor.GetParameters().Length; i++)
                {
                    list.Add(Type.Missing);
                }
                return (ContentElementAddon) ctor
                    .Invoke(BindingFlags.OptionalParamBinding |
                            BindingFlags.InvokeMethod |
                            BindingFlags.CreateInstance,
                        null,
                        list.ToArray(),
                        CultureInfo.InvariantCulture);
            }
            var instance = (ContentElementAddon)Activator.CreateInstance(type);
            if (model is ContentElementAddonModel m)
                instance.SetModel(m);
            return instance;
        }
        /// <summary>
        /// Gets the stored property.
        /// </summary>
        /// <typeparam name="T">The property's type to find</typeparam>
        /// <param name="defaultValue">The default value to return if the property is undefined.</param>
        /// <param name="name">The name of the property</param>
        /// <returns>The requested property</returns>
        protected T GetProperty<T>(T defaultValue = default(T), [CallerMemberName] string name = null)
        {
            if (name is null)
            {
                return default(T);
            }
            AddIfNotPresent(name, defaultValue);
            var value = Model.Properties[name];
            try
            {
                return (T) value;
            }
            catch (InvalidCastException)
            {
                return (T) Convert.ChangeType(value, typeof(T));
            }
        }
        /// <inheritdoc cref="GetProperty{T}"/>
        /// <summary>
        /// Gets the property with the default value. This is the same as <see cref="GetProperty{T}"/>, except the name is clearer.
        /// </summary>
        protected T GetPropertyWithDefault<T>(T defaultValue, string name = null) => GetProperty(defaultValue, name);
        private void AddIfNotPresent<T>(string name, T defaultValue = default(T))
        {
            if (!Model.Properties.ContainsKey(name))
            {
                Model.Properties.Add(name, defaultValue);
            }
        }
        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <typeparam name="T">The property's type</typeparam>
        /// <param name="value">The value to set</param>
        /// <param name="name">The name of the property</param>
        protected void SetProperty<T>(T value, [CallerMemberName] string name = null)
        {
            if (name is null) return;
            AddIfNotPresent<T>(name);
            var presentValue = Model.Properties[name];
            if (!presentValue.Equals(default(T)) && !(presentValue is T || presentValue is long))
            {
                throw new InvalidOperationException("Tried to set a value to its not corresponding type.");
            }
            Model.Properties[name] = value;
            OnPropertyChanged(name);
        }

        internal void SetModel(ContentElementAddonModel m)
        {
            base.Model = m;
        }

        public new ContentElementAddonModel Model => (ContentElementAddonModel)base.Model;
        protected override ContentElement BuildDefaultModel()
        {
            return new ContentElementAddonModel(this);
        }
        
        [JsonObject(ItemTypeNameHandling = TypeNameHandling.All)]
        public class ContentElementAddonModel : ContentElement
        {
            private readonly ContentElementAddon _typeProvider;

            [JsonConstructor]
            internal ContentElementAddonModel() { }

            internal ContentElementAddonModel(ContentElementAddon typeProvider)
            {
                _typeProvider = typeProvider;
            }

            public Type ParentType => _typeProvider.GetType();
            [JsonProperty(TypeNameHandling = TypeNameHandling.All, ItemTypeNameHandling = TypeNameHandling.All)]
            internal Dictionary<string, object> Properties { get; private set; } = new Dictionary<string, object>();
        }
    }
}
