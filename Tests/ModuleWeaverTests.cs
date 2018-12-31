using System.Reflection;
using Fody;
using Xunit;
#pragma warning disable 618

// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable

public class ModuleWeaverTests
{
    [Fact]
    public void WithFields()
    {
        var weavingTask = new ModuleWeaver();
        var testResult = weavingTask.ExecuteTestRun("AssemblyToProcess.dll", ignoreCodes: new[]
        {
            "0x8013129d"
        });
        AssertCalled(testResult, "ModuleInitializer");
        AssertCalled(testResult, "Foo.ModuleInitializer");
    }

    static void AssertCalled(TestResult testResult, string name)
    {
        var type = testResult.Assembly.GetType(name);
        var info = type.GetField("InitializeCalled", BindingFlags.Static | BindingFlags.Public);
        Assert.True((bool)info.GetValue(null));
    }
}