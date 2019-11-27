using System.Reflection;
using Fody;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

public class ModuleWeaverTests :
    VerifyBase
{
    [Fact]
    public void WithFields()
    {
        var weavingTask = new ModuleWeaver();
        var testResult = weavingTask.ExecuteTestRun(
            "AssemblyToProcess.dll",
            ignoreCodes: new[]
            {
                "0x8013129d"
            });
        AssertCalled(testResult, "ModuleInitializer");
        AssertCalled(testResult, "Foo.ModuleInitializer");
        AssertCalled(testResult, "Parent+ModuleInitializer");
    }

    static void AssertCalled(TestResult testResult, string name)
    {
        var type = testResult.Assembly.GetType(name);
        var info = type.GetField("InitializeCalled", BindingFlags.Static | BindingFlags.Public);
        Assert.True((bool)info.GetValue(null));
    }

    public ModuleWeaverTests(ITestOutputHelper output) :
        base(output)
    {
    }
}