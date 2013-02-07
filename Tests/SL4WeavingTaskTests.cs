using NUnit.Framework;


[TestFixture]
public class SL4WeavingTaskTests : BaseTaskTests
{

    public SL4WeavingTaskTests()
        : base(@"..\..\..\AssemblyToProcess\bin\DebugSilverlight4\AssemblyToProcess.dll")
    {
    }

}