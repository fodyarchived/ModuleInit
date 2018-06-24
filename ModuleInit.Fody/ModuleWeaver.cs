using System.Collections.Generic;
using System.Linq;
using Fody;

public class ModuleWeaver : BaseModuleWeaver
{
    public override void Execute()
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
            TypeSystem = TypeSystem
        };
        importer.Execute();
    }

    public override IEnumerable<string> GetAssembliesForScanning()
    {
        return Enumerable.Empty<string>();
    }

    public override bool ShouldCleanReference => true;
}