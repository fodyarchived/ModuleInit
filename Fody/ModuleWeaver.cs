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
        var initializeMethodFinder = new InitializeMethodFinder
                                     {
                                         ModuleWeaver = this
                                     };
        initializeMethodFinder.Execute();
        var importer = new ModuleLoaderImporter
                       {
                           InitializeMethodFinder = initializeMethodFinder,
                           ModuleWeaver = this,
                           TypeSystem = ModuleDefinition.TypeSystem
                       };
        importer.Execute();
    }
}