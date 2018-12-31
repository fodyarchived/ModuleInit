using System.Collections.Generic;
using System.Linq;
using Fody;
using Mono.Cecil;

public class InitializeMethodFinder
{
    public ModuleWeaver ModuleWeaver;
    public List<MethodDefinition> InitializeMethods = new List<MethodDefinition>();

    public void Execute()
    {
        var moduleInitializers = ModuleWeaver.ModuleDefinition
            .GetTypes()
            .Where(x => x.Name == "ModuleInitializer").ToList();
        if (moduleInitializers.Count == 0)
        {
            throw new WeavingException(@"Could not find type 'ModuleInitializer'.
public static class ModuleInitializer
{
    public static void Initialize()
    {
        //code goes here
    }
}");
        }

        foreach (var moduleInitializer in moduleInitializers)
        {
            Add(moduleInitializer);
        }
    }

    void Add(TypeDefinition moduleInitializer)
    {
        var initializeMethod = moduleInitializer.Methods.FirstOrDefault(x => x.Name == "Initialize");
        if (initializeMethod == null)
        {
            throw new WeavingException($"Could not find 'Initialize' method on '{moduleInitializer.FullName}'.");
        }

        if (!initializeMethod.IsPublic)
        {
            throw new WeavingException($"Method '{initializeMethod.FullName}' is not public.");
        }

        if (!initializeMethod.IsStatic)
        {
            throw new WeavingException($"Method '{initializeMethod.FullName}' is not static.");
        }

        if (initializeMethod.Parameters.Count > 0)
        {
            throw new WeavingException($"Method '{initializeMethod.FullName}' has parameters.");
        }
        InitializeMethods.Add(initializeMethod);
    }
}