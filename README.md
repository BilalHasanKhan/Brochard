[![brochard MyGet Build Status](https://www.myget.org/BuildSource/Badge/brochard?identifier=098718e3-f53d-4bcd-b29e-cb9da86823c0)](https://www.myget.org/)

# Brochard

Brochard is the implementation of Orchard CMS in Asp.Net VNext (also known as DNX)

## Getting Started

First off, follow the instructions here https://github.com/aspnet/home in order to install DNVM. Next install Visual Studio 2015, or what ever you flavour of editor is.

Next you want to clone the Repo. 'git clone https://github.com/OrchardCMS/Brochard.git' and checkout the master branch.

Run the build.cmd file included in the repository to bootstrap DNX and build solution.

Next navigate to 'D:\Brochard\src\Orchard.Web' or where ever your retrospective folder is on the command line in Administrative mode.

run.. 'dnx web' -> Hey you just kicked up the Orchard host.

Then in your browser, call the url... http://localhost:5001/setup/index

## Using Brochard

### Creating a host

When running Brochard, you need a client. The default implementation is to have a client talk to a host.

The client is any project that creates the host.

To create the host in a web project you would do

```c#
public class Startup {
    public IServiceProvider ConfigureServices(IServiceCollection services) {
        return services
            // AddHostSample is where the magic is done. This extension method lives in the Host (Orchard.Hosting.Web)
            .AddHostSample()
            .BuildServiceProvider();
    }
}
```

The Host has a small wrapper


```c#
public static IServiceCollection AddHostSample([NotNull] this IServiceCollection services) {
    // This will setup all your core services for a host
    return services.AddHost(internalServices => {
        // The core of the host
        internalServices.AddHostCore();
        //... All extra things you want registered so that you don't have to touch the core host.
    });
```

### Additional module locations

Additional locations for module discovery can be added in your host setup.

```c#
public static IServiceCollection AddHostSample([NotNull] this IServiceCollection services) {
    return services.AddHost(internalServices => {
        internalServices.AddHostCore();

        // Add folders the easy way
        internalServices.AddModuleFolder("~/Core/Orchard.Core");
        internalServices.AddModuleFolder("~/Modules");

        // Add folders the move configurable way
        internalServices.Configure<ExtensionHarvestingOptions>(options => {
            var expander = new ModuleLocationExpander(
                DefaultExtensionTypes.Module,
                new[] { "~/Core/Orchard.Core", "~/Modules" },
                "Module.txt"
                );

            options.ModuleLocationExpanders.Add(expander);
        });
    });
}
```

### Tenant Configuration

All tenant configuration lives in .\App_Data\Sites\Default within settings files.

i.e. Settings.txt
```yaml
State: Running
Name: Default
RequestUrlHost: localhost:5001
RequestUrlPrefix:
```

However, you may override these values within a json or xml file. The hierarchy is:

Settings.txt
  -> Settings.xml
     -> Settings.json

### Event Bus

The event bus must be set up in your host (Anyone using the default host will have it)

```c#
public class ShellModule : IModule {
    public void Configure(IServiceCollection serviceCollection) {
        // More registration
        serviceCollection.AddNotifierEvents(); // The important line
        // More registration
    }
}
```

This will allow you to register types of IEventHandler, and in turn execute the eventing modal.

Lets take the example of a Dog, you want to tell it to bark..

```c#
public interface ITestEvent : IEventHandler {
    void Talk(string value);
}

public class TestEvent1 : ITestEvent {
    public void Talk(string value) {
        Console.WriteLine("Talk Event ONE! " + value);
    }
}

public class TestEvent2 : ITestEvent {
    public void Talk(string value) {
        Console.WriteLine("Talk Event TWO! " + value);
    }
}
```

Next we want to call all Talk on ITestEvent... You need to inject in IEventNotifier,
then call notify on the type of interface you want to call passing the method
with the properties to it.

```c#
private readonly IEventNotifier _eventNotifier;

public Class1(IEventNotifier eventNotifier) {
    _eventNotifier = eventNotifier;
}

public void Call() {
    _eventNotifier.Notify<ITestEvent>(e => e.Talk("Bark!"));
}
```

The output will be
```
Talk Event ONE! Bark!
Talk Event TWO! Bark!
```

###Testing

We currently use XUnit to do unit testing, with Coypu and Chrome to do UI testing.

###Contributing

We currently follow the [Microsoft Asp.Net engineering guidelines](https://github.com/aspnet/Home/wiki/Engineering-guidelines)
