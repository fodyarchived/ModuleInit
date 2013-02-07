using NUnit.Framework;

[TestFixture]
public class Net4ClientWeavingTaskTests : BaseTaskTests
{

    public Net4ClientWeavingTaskTests()
        : base(@"..\..\..\AssemblyToProcess\bin\DebugDotNet4Client\AssemblyToProcess.dll")
    {
    }

}