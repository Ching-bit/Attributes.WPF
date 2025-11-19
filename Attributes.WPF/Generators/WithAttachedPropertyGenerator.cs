using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.Text;

namespace Attributes.WPF.Generators
{
    [Generator]
    internal class WithAttachedPropertyGenerator : GeneratorBase<WithAttachedPropertyAttribute>
    {
        protected override string GenerateCodeOnClass(string namespaceName, string className, IPropertySymbol[] props, IEnumerable<AttributeData> attributes)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($@"
namespace {namespaceName}
{{
    partial class {className}
    {{");
            foreach (AttributeData attribute in attributes)
            {
                string propertyType = StringHelper.ToGlobalFullName(((INamedTypeSymbol)attribute.ConstructorArguments[0].Value)?.ToDisplayString());
                string propertyName = StringHelper.ToCamel((string)attribute.ConstructorArguments[1].Value);
                string defaultValue = attribute.ConstructorArguments[2].ToCSharpString();
                string hostType = StringHelper.ToGlobalFullName(((INamedTypeSymbol)attribute.ConstructorArguments[3].Value)?.ToDisplayString());
                string propertyChangedCallback = (string)attribute.ConstructorArguments[4].Value;

                if (null == propertyType || null == propertyName)
                {
                    continue;
                }

                sb.Append($@"
        public static readonly global::System.Windows.DependencyProperty {propertyName}Property =
            global::System.Windows.DependencyProperty.RegisterAttached(
                ""{propertyName}"",
                typeof({propertyType}),
                typeof({className}),
                new global::System.Windows.PropertyMetadata({defaultValue}{(null == propertyChangedCallback ? string.Empty : $", {propertyChangedCallback}")}));

        [global::System.CodeDom.Compiler.GeneratedCode(""{AppInfo.AppName}"", ""{AppInfo.Version}"")]
        [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public static {propertyType} Get{propertyName}({hostType ?? "global::System.Windows.DependencyObject"} host)
        {{
            return ({propertyType})host.GetValue({propertyName}Property);
        }}

        [global::System.CodeDom.Compiler.GeneratedCode(""{AppInfo.AppName}"", ""{AppInfo.Version}"")]
        [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public static void Set{propertyName}({hostType ?? "global::System.Windows.DependencyObject"} host, {propertyType} value)
        {{
            host.SetValue({propertyName}Property, value);
        }}
");
            }

            sb.Append($@"
    }}
}}
");

            return sb.ToString();
        }

        protected override string GenerateCodeOnField(string namespaceName, string className, string fieldName, IEnumerable<AttributeData> attributes)
        {
            return string.Empty;
        }
    }
}
