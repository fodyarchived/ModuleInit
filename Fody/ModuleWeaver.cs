using System;
using Mono.Cecil;

public class ModuleWeaver
{
    public Action<string> LogInfo { get; set; }
    public ModuleDefinition ModuleDefinition { get; set; }

    public ModuleWeaver()
    {
        LogInfo = s => { };
    }

    public void Execute()
    {
        var finder = new InitializeMethodFinder
        {
            ModuleWeaver = this
        };
        finder.Execute();
        var importer = new ModuleLoaderImporter
        {
            InitializeMethodFinder = finder,
            ModuleWeaver = this,
            TypeSystem = ModuleDefinition.TypeSystem
        };
        importer.Execute();
    }
}