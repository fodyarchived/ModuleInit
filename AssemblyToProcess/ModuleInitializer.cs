public static class ModuleInitializer
{
    public static bool InitializeCalled;

    public static void Initialize()
    {
        InitializeCalled = true;
    }
}