using System.Collections.Generic;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

public static class CecilExtensions
{
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