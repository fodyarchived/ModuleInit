using System;
using System.Reflection;
using Mono.Cecil;

public class ModuleWeaver
{
    static Assembly assembly;
    public Action<string> LogInfo { get; set; }
    public ModuleDefinition ModuleDefinition { get; set; }

    static ModuleWeaver()
    {
        assembly = typeof (ModuleWeaver).Assembly;
    }

    public ModuleWeaver()
    {
        LogInfo = s => { };
    }

    public void Execute()
    {
        var initializeMethodFinder = new InitializeMethodFinder(this);
        initializeMethodFinder.Execute();
        var importer = new ModuleLoaderImporter(this, initializeMethodFinder, ModuleDefinition.TypeSystem);
        importer.Execute();
    }
}