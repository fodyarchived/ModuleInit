using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

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

    public static void Replace(this Collection<Instruction> collection, Instruction instruction, IEnumerable<Instruction> instructions)
    {
        var indexOf = collection.IndexOf(instruction);
        collection.RemoveAt(indexOf);
        foreach (var instruction1 in instructions)
        {
            collection.Insert(indexOf, instruction1);
            indexOf++;
        }
    }
}