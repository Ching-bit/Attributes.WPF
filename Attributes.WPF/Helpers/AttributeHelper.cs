using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace Attributes.WPF
{
    internal static class AttributeHelper
    {
        public static IncrementalValuesProvider<INamedTypeSymbol> GetClassesWithTheAttribute(
            IncrementalGeneratorInitializationContext context, string attributeFullName)
        {
            return
                context.SyntaxProvider.CreateSyntaxProvider(
                        predicate: (s, _) => IsClassWithAttributes(s),
                        transform: (ctx, _) => GetClassSymbol(ctx, attributeFullName))
                    .Where(m => m != null);
        }

        private static bool IsClassWithAttributes(SyntaxNode node)
        {
            return node is ClassDeclarationSyntax c && c.AttributeLists.Count > 0;
        }

        private static INamedTypeSymbol GetClassSymbol(GeneratorSyntaxContext context, string attributeFullName)
        {
            ClassDeclarationSyntax classDecl = (ClassDeclarationSyntax)context.Node;
            ISymbol symbol = context.SemanticModel.GetDeclaredSymbol(classDecl);
            if (symbol?.GetAttributes().Any(a => a.AttributeClass?.ToDisplayString() == attributeFullName) == true)
            {
                return (INamedTypeSymbol)symbol;
            }
            return null;
        }

        public static IncrementalValuesProvider<IFieldSymbol> GetFieldsWithTheAttribute(
            IncrementalGeneratorInitializationContext context, string attributeFullName)
        {
            return
                context.SyntaxProvider.CreateSyntaxProvider(
                        predicate: (s, _) => IsFieldWithAttributes(s),
                        transform: (ctx, _) => GetFieldSymbol(ctx, attributeFullName))
                    .Where(m => m != null);
        }

        private static bool IsFieldWithAttributes(SyntaxNode node)
        {
            return node is FieldDeclarationSyntax f && f.AttributeLists.Count > 0;
        }

        private static IFieldSymbol GetFieldSymbol(GeneratorSyntaxContext context, string attributeFullName)
        {
            FieldDeclarationSyntax fieldDecl = (FieldDeclarationSyntax)context.Node;
            foreach (VariableDeclaratorSyntax variable in fieldDecl.Declaration.Variables)
            {
                IFieldSymbol symbol = context.SemanticModel.GetDeclaredSymbol(variable) as IFieldSymbol;
                if (symbol?.GetAttributes().Any(a => a.AttributeClass?.ToDisplayString() == attributeFullName) == true)
                {
                    return symbol;
                }
            }
            return null;
        }
    }
}
