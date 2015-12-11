# CrossCutting
[![Build status](https://ci.appveyor.com/api/projects/status/obc6jv1768rol144?svg=true)](https://ci.appveyor.com/project/bronumski/crosscutting)
[![NuGet Status](http://img.shields.io/nuget/v/CrossCutting.Core.svg?style=flat)](https://www.nuget.org/packages/CrossCutting.Core/) 

**Cross cutting libraries for .net applications**

# Logging

Tasks such as logging are features that are required across an application as a whole. Unfortunatly with
most logging frameworks logging poloutes an otherwise potentialy clean code base. Where it be through
injection or the actual act of logging. This also has an impact on unit tests where loggers have to be
initialized to prevent NullReferenceExceptions. On top of this the actual act of logging is noisy and can
also have detremental side effects.

The purpose of the CrossCutting library is to provide an easy set of extension methods that wrap over your
logging framework of choice allowing for less code, safe use and test free hastle.

### Getting started

Logging is provided by a set of statics. The type of logger provided is configured in the
LoggerProviderFactories class. By default this is configured with a sideefect free NullLogger that does
nothing. This allows the application with logging in mind without having to chose which logging framework to
use and where it is going to actually log to. It also means that tests will run without the test framework
having any impact.

To log a message the API exposes a simple extension methog on any object, which allows for the following in
your objects.

```csharp
class MyClass
{
    public void SomeMethod(RichObject richObject)
    {
        try
        {
            this.Log().Debug("Did something");
            
            richObject.Log().Debug("It is easy to write a log message from outside its context");
        }
        catch(Exception exception)
        {
            this.Log().Warn(exception, "Something went wrong");
        }
    }
}
```

Due to the nature of static objects we have to get the logger slightly differently

```csharp
static class MyStaticClass
{
    public static void SomeMethod(RichObject richObject)
    {
        LogProvider.GetLoggerFor<MyStaticClass>().Debug("Message from static class");
    }
}
```

### Output to the desired logging framework

To change where the log messages are sent the API needs to be given the logging implementation you want to use
The following is an example of configuring the API to use log4net.

```csharp
//Specify the provider instance
LogProvider.SetLoggingProvider(new Log4netLoggerProvider());

//Specify the provider type
LogProvider.SetLoggingProvider<Log4netLoggerProvider>();
```

## Safe, reduced sidefect logging

Logging should have no or minimal side effects, logging should not be the cause of your application crashing.
Most logging frameworks are very resiliant and tend to be safe when used correctly. However it is very easy
to use them in such a way that would case a system failure. Consider the following code which uses the
log4net logging framework as an illistration:

### The problem

```csharp
//Simple format exception when providing an array of arguments
logger.DebugFormat("{0},{1}", new object[] { "foo" });
//This would result in a format exception that would cause the executing method to fail

//Getting value from null object or method which throws an exception
logger.Debug(volatileObject.SomeValue());
logger.DebugFormat("{0},{1}", nullObject.SomeValue(), volatileObject.SomeValue());
//This could result in eather a null reference exception or some other unhandled exception further up the stack
```

All of these issues can easily be avoided with additional defensive code but what this adds in defence it
detracts in noise. Secondly when we resolve values from objects for logging purposes we also add a potential side effect
even when we don't even care about that log level. For example:

```csharp
//Even if we are not capturing debug level events the function to resolve the log message will still execute
logger.Debug(complexObject.SomeValue());

//Even if we are not capturing debug level events the function to resolve the log message will still execute
logger.Debug(complexObject.SomeValue());
```

Now most logging frameworks provide a mechanisum to prevent code be executed when the specified log level
is disabled. In log4net the Format methods do not call the string format to build the log message if the log
level is disabled. However if the format arguments are provided by method calls these will still be executed.
The logger does provide an IsEnabled property for each level but again this adds further logging noise.

### Logging safely with minimal noise

The CrossCutting logging extentions provide a number of safe logging methods on top of the methods that one
would expect to exist in a mature logging library. These methods make it easier for messages to be logged
more safely and with minimal noise. THere is however no foolproof way of preventing the developer making a
simple mistake without crippling the interface. You should use the following methods if you are logging more
than a simple string message.

```csharp
//Using a format message with rich arguments
this.Log().DebugFormat("Logging result {0} and {1}", () => myRichObject.SomeValue(), () => myRichObject.SomeOtherValue());

//Rendering a whole message from a function
this.Log().Debug(() =>
{
    return someComplexMessage;
}
```

The benefite of these two methods is that:
1) Delayes the execution of the functions untill they point of writting the message.
2) The functions will never execute if the log level used is not enabled.
3) Any exception thrown from resolving the message will not cause the application to crash and the api
will attempt to write as much as it can.

## Testing

There are times that as a developer we want to test that we are logging the required output. The api allows you
to supply your own stubbed object inorder to test. The following shows an example of this using NSubstitute
but any mocking framework or custom implementation will do.

```csharp
//Stubbed logging provider
var loggingProvider = Substitute.For<ILoggingProvider>();

//Set the default logging provider
LogProvider.SetLoggingProvider(loggingProvider);

//Stubbed logger
var logger = Substitute.For<ILogger>();

//Add logger to logging provider so that it is returned when calling for a logger
loggingProvider.Create(Arg.Any<string>()).Returns(logger);

//Do work

//Assert logger was called
logger.Received().Log(LogLevel.Debug, "Message");

//or if an exception was logged
logger.Received().Log(LogLevel.Debug, expectedException, "Message");

//Remember to reset the LogProvider at the test teardown to prevent unwanted side effects
LogProvider.Reset();
```

[![Build status](https://ci.appveyor.com/api/projects/status/obc6jv1768rol144?svg=true)](https://ci.appveyor.com/project/bronumski/crosscutting)
[![NuGet Status](http://img.shields.io/nuget/v/CrossCutting.Core.svg?style=flat)](https://www.nuget.org/packages/CrossCutting.Core/)