using System.IO;
using System.Reflection;
using Mono.Cecil;
using NUnit.Framework;
// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable

[TestFixture]
public class ModuleWeaverTests
{
    Assembly assembly;
    string beforeAssemblyPath;
    string afterAssemblyPath;

    public ModuleWeaverTests()
    {
        var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "AssemblyToProcess.dll");
        beforeAssemblyPath = Path.GetFullPath(path);

        afterAssemblyPath = beforeAssemblyPath.Replace(".dll", "2.dll");
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
    }

    [Test]
    public void WithFields()
    {
        var type = assembly.GetType("ModuleInitializer");
        var info = type.GetField("InitializeCalled", BindingFlags.Static | BindingFlags.Public);
        Assert.IsTrue((bool)info.GetValue(null));
    }

#if (NET452)
    [Test]
    public void Verify()
    {
        Verifier.Verify(beforeAssemblyPath, afterAssemblyPath);
    }
#endif
}