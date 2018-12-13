using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NextCanvas.Extensibility
{
    public class AddonInfo
    {
        public AddonInfoAttribute AddonInfoAttribute { get; }
        public AddonElementData[] ResolvedAddonElements { get; }
        public DataTemplate[] AvailableDataTemplates { get; } = new DataTemplate[0];
        public AddonInfo(Assembly assembly, bool isReflectionOnlyLoad = false)
        {
            if (isReflectionOnlyLoad)
            {
                if (assembly.CustomAttributes.All(c => c.AttributeType.FullName != typeof(AddonInfoAttribute).FullName))
                {
                    ThrowAddonInfoNotFound();
                }
                assembly = Assembly.Load(assembly.GetName());
            }
            var attribute = assembly.GetCustomAttributes().OfType<AddonInfoAttribute>().FirstOrDefault();
            if (attribute is null) ThrowAddonInfoNotFound();
            AddonInfoAttribute = attribute;
            ResolvedAddonElements = assembly.ExportedTypes
                .Select(t => new AddonElementData(t.GetCustomAttribute<AddonElementAttribute>(), t))
                .Where(data => data.Attribute != null).ToArray();
            try
            {
                var dictionary =
                    (ResourceDictionary) Application.LoadComponent(
                        new Uri($"{assembly.GetName().Name};component/DataTemplates.xaml", UriKind.Relative));
                AvailableDataTemplates = dictionary.Values.OfType<DataTemplate>().ToArray();
            }
            catch (Exception e)
            {
                Debug.WriteLine("No data templates? " + e.Message);
            }
        }

        private static void ThrowAddonInfoNotFound()
        {
            throw new AddonInfoNotFoundException("This assembly does not have an AddonInfoAttribute.");
        }
    }

    public class AddonElementData
    {
        public AddonElementAttribute Attribute { get; }
        public Type Type { get; }

        public AddonElementData(AddonElementAttribute attribute, Type type)
        {
            Attribute = attribute;
            Type = type;
        }
    }
}
