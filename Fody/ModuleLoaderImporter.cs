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
        var attributes = MethodAttributes.Static
                         | MethodAttributes.SpecialName
                         | MethodAttributes.RTSpecialName;
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
            cctor.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
        }
        cctor.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Call, initializeMethodFinder.InitializeMethod));
    }
}