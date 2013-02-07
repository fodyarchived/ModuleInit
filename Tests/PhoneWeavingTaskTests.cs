#if(DEBUG)

using NUnit.Framework;

[TestFixture]
public class PhoneWeavingTaskTests : BaseTaskTests
{

    public PhoneWeavingTaskTests()
        : base(@"..\..\..\AssemblyToProcess\bin\DebugPhone\AssemblyToProcess.dll")
    {
    }

}

#endif