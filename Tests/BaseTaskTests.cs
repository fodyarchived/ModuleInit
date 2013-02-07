using System.IO;
using System.Reflection;
using Mono.Cecil;
using NUnit.Framework;

public abstract class BaseTaskTests
{
    Assembly assembly;
    string beforeAssemblyPath;
    string afterAssemblyPath;

    protected BaseTaskTests(string filePath)
    {

        beforeAssemblyPath = Path.GetFullPath(filePath);
#if (!DEBUG)

        beforeAssemblyPath = beforeAssemblyPath.Replace("Debug", "Release");
#endif

        afterAssemblyPath = beforeAssemblyPath.Replace(".dll", "2.dll");
        File.Copy(beforeAssemblyPath, afterAssemblyPath, true);

        var moduleDefinition = ModuleDefinition.ReadModule(afterAssemblyPath);
        var weavingTask = new ModuleWeaver
        {
            ModuleDefinition = moduleDefinition,
        };

        weavingTask.Execute();
        moduleDefinition.Write(afterAssemblyPath);

        assembly = Assembly.LoadFile(afterAssemblyPath);
    }


    [Test]
    public void WithFields()
    {
        var type = assembly.GetType("ModuleInitializer");
        var info = type.GetField("InitializeCalled", BindingFlags.Static | BindingFlags.Public);
        Assert.IsTrue((bool)info.GetValue(null));
    }


#if(DEBUG)
    [Test]
    public void PeVerify()
    {
        Verifier.Verify(beforeAssemblyPath,afterAssemblyPath);
    }
#endif

}