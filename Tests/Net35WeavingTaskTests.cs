using NUnit.Framework;

[TestFixture]
public class Net35WeavingTaskTests : BaseTaskTests
{

    public Net35WeavingTaskTests()
        : base(@"..\..\..\AssemblyToProcess\bin\DebugDotNet3.5\AssemblyToProcess.dll")
    {
    }

}