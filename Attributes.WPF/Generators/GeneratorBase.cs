using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Attributes.WPF
{
    internal abstract class GeneratorBase<T> : IIncrementalGenerator where T : Attribute
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            string attributeFullName = typeof(T).FullName;

            // Field on class
            IncrementalValuesProvider<INamedTypeSymbol> classes =
                AttributeHelper.GetClassesWithTheAttribute(context, attributeFullName);
            var compilationAndClasses = context.CompilationProvider.Combine(classes.Collect());
            context.RegisterSourceOutput(compilationAndClasses,
                (spc, source) => GenerateCodeOnClasses(source.Left, source.Right, spc, attributeFullName));

            // Field on properties
            IncrementalValuesProvider<IFieldSymbol> fields =
                AttributeHelper.GetFieldsWithTheAttribute(context, attributeFullName);
            var compilationAndFields = context.CompilationProvider.Combine(fields.Collect());
            context.RegisterSourceOutput(compilationAndFields,
                (spc, source) => GenerateCodeOnFields(source.Left, source.Right, spc, attributeFullName));
        }

        private void GenerateCodeOnClasses(Compilation compilation, ImmutableArray<INamedTypeSymbol> classes,
            SourceProductionContext context, string attributeFullName)
        {
            foreach (var classSymbol in classes)
            {
                string namespaceName = classSymbol.ContainingNamespace.IsGlobalNamespace ?
                    null : classSymbol.ContainingNamespace.ToDisplayString();

                IPropertySymbol[] props = classSymbol.GetMembers()
                    .OfType<IPropertySymbol>()
                    .Where(p => p.GetMethod != null)
                    .ToArray();

                IEnumerable<AttributeData> attributes = classSymbol.GetAttributes().Where(x => x.AttributeClass?.ToDisplayString() == attributeFullName);

                string source = GenerateCodeOnClass(namespaceName, classSymbol.Name, props, attributes);
                context.AddSource($"{typeof(T).Name}.{classSymbol.Name}.g.cs",
                    SourceText.From(source, Encoding.UTF8));
            }
        }

        protected abstract string GenerateCodeOnClass(string namespaceName, string className, IPropertySymbol[] props, IEnumerable<AttributeData> attributes);

        private void GenerateCodeOnFields(Compilation compilation, ImmutableArray<IFieldSymbol> fields,
            SourceProductionContext context, string attributeFullName)
        {
            foreach (IFieldSymbol field in fields)
            {
                INamedTypeSymbol classSymbol = field.ContainingType;
                string namespaceName = classSymbol.ContainingNamespace.IsGlobalNamespace ?
                    null : classSymbol.ContainingNamespace.ToDisplayString();

                IEnumerable<AttributeData> attributes = classSymbol.GetAttributes().Where(x => x.AttributeClass?.ToDisplayString() == attributeFullName);

                string source = GenerateCodeOnField(namespaceName, classSymbol.Name, field.Name, attributes);
                context.AddSource($"{typeof(T).Name}.{classSymbol.Name}.{field.Name}.g.cs",
                    SourceText.From(source, Encoding.UTF8));
            }
        }

        protected abstract string GenerateCodeOnField(string namespaceName, string className, string fieldName, IEnumerable<AttributeData> attributes);
    }
}
