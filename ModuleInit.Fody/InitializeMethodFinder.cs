using System.Linq;
using Fody;
using Mono.Cecil;

public class InitializeMethodFinder
{
    public ModuleWeaver ModuleWeaver;
    public MethodDefinition InitializeMethod;

    public void Execute()
    {
        var moduleInitializer = ModuleWeaver.ModuleDefinition
            .GetTypes()
            .SingleOrDefault(x => x.Name == "ModuleInitializer");
        if (moduleInitializer == null)
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

        InitializeMethod = moduleInitializer.Methods.FirstOrDefault(x => x.Name == "Initialize");
        if (InitializeMethod == null)
        {
            throw new WeavingException($"Could not find 'Initialize' method on '{moduleInitializer.FullName}'.");
        }
        if (!InitializeMethod.IsPublic)
        {
            throw new WeavingException($"Method '{InitializeMethod.FullName}' is not public.");
        }
        if (!InitializeMethod.IsStatic)
        {
            throw new WeavingException($"Method '{InitializeMethod.FullName}' is not static.");
        }
        if (InitializeMethod.Parameters.Count > 0)
        {
            throw new WeavingException($"Method '{InitializeMethod.FullName}' has parameters.");
        }
    }
}