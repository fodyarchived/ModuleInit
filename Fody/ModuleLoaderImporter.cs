using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;


public class ModuleLoaderImporter
{
    ModuleWeaver moduleWeaver;
    InitializeMethodFinder initializeMethodFinder;
    TypeSystem typeSystem;
    MethodDefinition cctor;
    bool isNewCctor;

    public ModuleLoaderImporter(ModuleWeaver moduleWeaver, InitializeMethodFinder initializeMethodFinder, TypeSystem typeSystem)
    {
        this.moduleWeaver = moduleWeaver;
        this.initializeMethodFinder = initializeMethodFinder;
        this.typeSystem = typeSystem;
    }

    public void Execute()
    {
        GetCctor();
        var il = cctor.Body.GetILProcessor();
        il.Append(il.Create(OpCodes.Call, initializeMethodFinder.InitializeMethod));
        if (isNewCctor)
        {
            il.Append(il.Create(OpCodes.Ret));
        }
    }

    void GetCctor()
    {
        const MethodAttributes attributes = MethodAttributes.Static
                                            | MethodAttributes.SpecialName
                                            | MethodAttributes.RTSpecialName;
        var moduleClass = moduleWeaver.ModuleDefinition.Types.FirstOrDefault(x => x.Name == "<Module>");
        if (moduleClass == null)
        {
            throw new WeavingException("Found no module class!");
        }
         cctor = moduleClass.Methods.FirstOrDefault(x => x.Name == ".cctor");
        if (cctor == null)
        {
            isNewCctor = true;
            cctor = new MethodDefinition(".cctor", attributes, typeSystem.Void);
            moduleClass.Methods.Add(cctor);
        }
        
    }
}