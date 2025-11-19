using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.Text;

namespace Attributes.WPF
{
    [Generator]
    internal class WithDependencyPropertyGenerator : GeneratorBase<WithDependencyPropertyAttribute>
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

                if (null == propertyType || null == propertyName)
                {
                    continue;
                }

                sb.Append($@"
        [global::System.CodeDom.Compiler.GeneratedCode(""{AppInfo.AppName}"", ""{AppInfo.Version}"")]
        [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public {propertyType} {propertyName}
        {{
            get {{ return ({propertyType})GetValue({propertyName}Property); }}
            set {{ SetValue({propertyName}Property, value); }}
        }}
        public static readonly global::System.Windows.DependencyProperty {propertyName}Property =
            global::System.Windows.DependencyProperty.Register(""{propertyName}"", typeof({propertyType}), typeof({className}), new global::System.Windows.PropertyMetadata(({propertyType}){defaultValue}));
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
