using System;

namespace Attributes.WPF
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class WithDependencyPropertyAttribute : Attribute
    {
        public WithDependencyPropertyAttribute(Type type, string name, object defaultValue)
        {
            Type = type;
            Name = name;
            DefaultValue = defaultValue;
        }

        public Type Type { get; set; }
        public string Name { get; set; }
        public object DefaultValue { get; set; }
    }
}
