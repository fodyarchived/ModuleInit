using NUnit.Framework;

[TestFixture]
public class Net4WeavingTaskTests : BaseTaskTests
{

    public Net4WeavingTaskTests()
        : base(@"..\..\..\AssemblyToProcess\bin\DebugDotNet4\AssemblyToProcess.dll")
    {
    }

}