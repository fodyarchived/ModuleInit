using System.IO;
using System.Reflection;
using Mono.Cecil;
using NUnit.Framework;

[TestFixture]
public class ModuleWeaverTests
{
    Assembly assembly;

    public ModuleWeaverTests()
    {
        var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "AssemblyToProcess.dll");
        var beforeAssemblyPath = Path.GetFullPath(path);

        var afterAssemblyPath = beforeAssemblyPath.Replace(".dll", "2.dll");
        File.Copy(beforeAssemblyPath, afterAssemblyPath, true);

        using (var moduleDefinition = ModuleDefinition.ReadModule(beforeAssemblyPath))
        {
            var weavingTask = new ModuleWeaver
            {
                ModuleDefinition = moduleDefinition,
            };

            weavingTask.Execute();
            moduleDefinition.Write(afterAssemblyPath);
        }

        assembly = Assembly.LoadFile(afterAssemblyPath);
#if(DEBUG)
        Verifier.Verify(beforeAssemblyPath, afterAssemblyPath);
#endif
    }

    [Test]
    public void WithFields()
    {
        var type = assembly.GetType("ModuleInitializer");
        var info = type.GetField("InitializeCalled", BindingFlags.Static | BindingFlags.Public);
        Assert.IsTrue((bool)info.GetValue(null));
    }
}