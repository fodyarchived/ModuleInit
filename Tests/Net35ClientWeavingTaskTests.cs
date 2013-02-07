using NUnit.Framework;

[TestFixture]
public class Net35ClientWeavingTaskTests : BaseTaskTests
{

    public Net35ClientWeavingTaskTests()
        : base(@"..\..\..\AssemblyToProcess\bin\DebugDotNet3.5Client\AssemblyToProcess.dll")
    {
    }

}