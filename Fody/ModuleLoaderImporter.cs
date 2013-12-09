using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;


public class ModuleLoaderImporter
{
	public ModuleWeaver ModuleWeaver;
    public InitializeMethodFinder InitializeMethodFinder;
    public TypeSystem TypeSystem;

	public void Execute()
	{
		var moduleClass = ModuleWeaver
            .ModuleDefinition
            .Types
            .FirstOrDefault(x => x.Name == "<Module>");
		if (moduleClass == null)
		{
			throw new WeavingException("Found no module class!");
		}
		var cctor = FindOrCreateCctor(moduleClass);
	    var body = cctor.Body;
	    body.SimplifyMacros();
        var returnPoints = body.Instructions
            .Where(x => x.OpCode == OpCodes.Ret)
            .ToList();

	    foreach (var instruction in returnPoints)
	    {
	        body.Instructions.Replace(instruction,
	            new[]
	            {
	                Instruction.Create(OpCodes.Call, InitializeMethodFinder.InitializeMethod),
	                Instruction.Create(OpCodes.Ret)
	            });
	    }

	    body.OptimizeMacros();
	}

    MethodDefinition FindOrCreateCctor(TypeDefinition moduleClass)
    {
        var cctor = moduleClass.Methods.FirstOrDefault(x => x.Name == ".cctor");
        if (cctor == null)
        {
            var attributes = MethodAttributes.Private
                             | MethodAttributes.HideBySig
                             | MethodAttributes.Static
                             | MethodAttributes.SpecialName
                             | MethodAttributes.RTSpecialName;
            cctor = new MethodDefinition(".cctor", attributes, TypeSystem.Void);
            moduleClass.Methods.Add(cctor);
            cctor.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
        }
        return cctor;
    }
}