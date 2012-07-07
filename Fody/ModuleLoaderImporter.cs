using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;


public class ModuleLoaderImporter
{
    ModuleWeaver moduleWeaver;
    InitializeMethodFinder initializeMethodFinder;
    TypeSystem typeSystem;

    public ModuleLoaderImporter(ModuleWeaver moduleWeaver, InitializeMethodFinder initializeMethodFinder, TypeSystem typeSystem)
    {
        this.moduleWeaver = moduleWeaver;
        this.initializeMethodFinder = initializeMethodFinder;
        this.typeSystem = typeSystem;
    }

    public void Execute()
    {
        const MethodAttributes attributes = MethodAttributes.Static
                                            | MethodAttributes.SpecialName
                                            | MethodAttributes.RTSpecialName;
        var cctor = GetCctor(attributes);
        var il = cctor.Body.GetILProcessor();
        il.Append(il.Create(OpCodes.Call, initializeMethodFinder.InitializeMethod));
        il.Append(il.Create(OpCodes.Ret));
    }

    private MethodDefinition GetCctor(MethodAttributes attributes)
    {
        var moduleClass = moduleWeaver.ModuleDefinition.Types.FirstOrDefault(x => x.Name == "<Module>");
        if (moduleClass == null)
        {
            throw new WeavingException("Found no module class!");
        }
        var cctor = moduleClass.Methods.FirstOrDefault(x => x.Name == ".cctor");
        if (cctor == null)
        {
            cctor = new MethodDefinition(".cctor", attributes, typeSystem.Void);
            moduleClass.Methods.Add(cctor);
        }
        return cctor;
    }
}