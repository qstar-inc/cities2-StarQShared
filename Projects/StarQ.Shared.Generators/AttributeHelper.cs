using Microsoft.CodeAnalysis;

namespace StarQ.Shared.Generators
{
    internal class AttributeHelper
    {
        public static bool HasAttribute(INamedTypeSymbol symbol, string attributeTypeName)
        {
            foreach (var attr in symbol.GetAttributes())
            {
                if (attr.AttributeClass?.Name == attributeTypeName)
                    return true;
            }
            return false;
        }
    }
}
