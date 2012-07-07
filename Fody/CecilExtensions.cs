using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

public static class CecilExtensions
{

    public static List<TypeDefinition> GetAllTypeDefinitions(this ModuleDefinition moduleDefinition)
    {
        var definitions = new List<TypeDefinition>();
        //First is always module so we will skip that;
        GetTypes(moduleDefinition.Types.Skip(1), definitions);
        return definitions;
    }
    static void GetTypes(IEnumerable<TypeDefinition> typeDefinitions, List<TypeDefinition> definitions)
    {
        foreach (var typeDefinition in typeDefinitions)
        {
            GetTypes(typeDefinition.NestedTypes, definitions);
            definitions.Add(typeDefinition);

        }
    }
    public static bool ContainsAttribute(this IEnumerable<CustomAttribute> attributes, string attributeName)
    {
        return attributes.Any(attribute => attribute.Constructor.DeclaringType.Name == attributeName);
    }
}