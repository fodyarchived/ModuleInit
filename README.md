## This is an add-in for [Fody](https://github.com/Fody/Fody/) 

Adds a module initializer to an assembly.

[Introduction to Fody](http://github.com/Fody/Fody/wiki/SampleUsage)

## Nuget package http://nuget.org/packages/ModuleInit.Fody 

Based on Einar Egilsson's suggestion using cecil to create module initializers [http://tech.einaregilsson.com/2009/12/16/module-initializers-in-csharp/]

## What it does 

### Finds a class, in the target assembly, named 'ModuleInitializer' with the following form.

    public static class ModuleInitializer
    {
        public static void Initialize()
        {
            //Init code
        }
    }

### Injects the following code into the module initializer of the target assembly. This code will be called when the assembly is loaded into memory


    static <Module>()
    {
        ModuleInitializer.Initialize();
    }

