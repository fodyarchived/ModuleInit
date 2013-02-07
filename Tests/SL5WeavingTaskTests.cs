#if(DEBUG)
using NUnit.Framework;

[TestFixture]
public class SL5WeavingTaskTests : BaseTaskTests
{

    public SL5WeavingTaskTests()
        : base(@"..\..\..\AssemblyToProcess\bin\DebugSilverlight5\AssemblyToProcess.dll")
    {
    }

}
#endif