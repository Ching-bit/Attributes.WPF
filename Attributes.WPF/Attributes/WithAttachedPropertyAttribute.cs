using System;

namespace Attributes.WPF
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class WithAttachedPropertyAttribute : Attribute
    {
        public WithAttachedPropertyAttribute(Type type, string name, object defaultValue,
            Type hostType = null, string propertyChangedCallback = null)
        {
            Type = type;
            Name = name;
            DefaultValue = defaultValue;
            HostType = hostType;
            PropertyChangedCallback = propertyChangedCallback;
        }

        public Type Type { get; set; }
        public string Name { get; set; }
        public object DefaultValue { get; set; }

        public Type HostType { get; set; }
        public string PropertyChangedCallback { get; set; }
    }
}
