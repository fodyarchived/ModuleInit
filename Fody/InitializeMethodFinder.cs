using System.Linq;
using Mono.Cecil;

public class InitializeMethodFinder
{
    ModuleWeaver moduleWeaver;
    public MethodDefinition InitializeMethod;

    public InitializeMethodFinder(ModuleWeaver moduleWeaver)
    {
        this.moduleWeaver = moduleWeaver;
    }

    public void Execute()
    {
        var moduleInitializer = moduleWeaver.ModuleDefinition.GetAllTypeDefinitions().FirstOrDefault(x => x.Name == "ModuleInitializer");
        if (moduleInitializer == null)
        {
            throw new WeavingException("Cound not find type 'ModuleInitializer'.");
        }
        InitializeMethod = moduleInitializer.Methods.FirstOrDefault(x => x.Name == "Initialize");
        if (InitializeMethod == null)
        {
            throw new WeavingException(string.Format("Could not find 'Initialize' method on '{0}'.", moduleInitializer.FullName));
        }
        if (!InitializeMethod.IsPublic)
        {
            throw new WeavingException(string.Format("Method '{0}' is not public.", InitializeMethod.FullName));
        }
        if (!InitializeMethod.IsStatic)
        {
            throw new WeavingException(string.Format("Method '{0}' is not static.", InitializeMethod.FullName));
        }
        if (InitializeMethod.Parameters.Count > 0)
        {
            throw new WeavingException(string.Format("Method '{0}' has parameters.", InitializeMethod.FullName));
        }
    }

}