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
        var testResult = weavingTask.ExecuteTestRun("AssemblyToProcess.dll", false);
        var type = testResult.Assembly.GetType("ModuleInitializer");
        var info = type.GetField("InitializeCalled", BindingFlags.Static | BindingFlags.Public);
        Assert.True((bool)info.GetValue(null));
    }
}