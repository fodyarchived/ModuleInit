using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Mono.Cecil;

public class InitializeMethodFinder
{
    public ModuleWeaver ModuleWeaver;
    public MethodDefinition InitializeMethod;

    public void Execute()
    {
        var moduleInitializer = ModuleWeaver.ModuleDefinition.GetAllTypeDefinitions().FirstOrDefault(x => x.Name == "ModuleInitializer");
        if (moduleInitializer == null)
        {
	        try
	        {
		        SetClipboard();
		        throw new WeavingException("Could not find type 'ModuleInitializer'. A template has been copied to the Clipboard.");
	        }
	        catch (Exception)
			{
				throw new WeavingException("Could not find type 'ModuleInitializer'.");
	        }
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

	static void SetClipboard()
	{
		var templateClass = @"
public static class ModuleInitializer
{
    public static void Initialize()
    {
        //Your code goes here
    }
}";
		var thread = new Thread(() => Clipboard.SetText(templateClass));
		thread.SetApartmentState(ApartmentState.STA);
		thread.Start();
		thread.Join();
	}
}