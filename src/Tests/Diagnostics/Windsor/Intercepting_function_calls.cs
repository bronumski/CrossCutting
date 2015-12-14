using System;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace CrossCutting.Diagnostics.Windsor
{
    class Intercepting_function_calls : IRequireStubedLoggerProvider
    {
        private ILogger logger;
        private IInterface interceptedObject;
        private IWindsorContainer windsorContainer;
        private object expectedObject;

        [SetUp]
        public void SetUp()
        {
            interceptedObject = Substitute.For<IInterface>();

            expectedObject = new object();

            interceptedObject.GetSomething().Returns(expectedObject);

            logger = Substitute.For<ILogger>();

            logger.LevelEnabled(LogLevel.Debug).Returns(true);
            logger.LevelEnabled(LogLevel.Error).Returns(true);

            windsorContainer = new WindsorContainer()
                .AddFacility<LoggingFacility>()
                .Register(Component.For<IInterface>().UsingFactoryMethod((x, c) => interceptedObject).Interceptors<LoggingInterceptor>());
        }

        [Test]
        public void Should_log_call_when_function_is_called()
        {
            LogProvider.LoggerProvider.Create(interceptedObject.GetType().ToString()).Returns(logger);

            windsorContainer.Resolve<IInterface>().GetSomething();

            logger.Received().Log(LogLevel.Debug, "Calling method: 'GetSomething'");
        }

        [Test]
        public void Should_log_the_function_invocation_has_completed_and_the_time_it_took_and_the_return_value()
        {
            LogProvider.LoggerProvider.Create(interceptedObject.GetType().ToString()).Returns(logger);

            windsorContainer.Resolve<IInterface>().GetSomething();

            logger.Received().Log(LogLevel.Debug, Arg.Is<string>(x => x.StartsWith("Method 'GetSomething' returned value 'System.Object' in '")));
        }

        [Test]
        public void Should_call_function_on_underlying_object()
        {
            LogProvider.LoggerProvider.Create(interceptedObject.GetType().ToString()).Returns(logger);

            var actualObject = windsorContainer.Resolve<IInterface>().GetSomething();

            actualObject.Should().BeSameAs(expectedObject);
        }

        [Test]
        public void Should_log_method_parameters()
        {
            LogProvider.LoggerProvider.Create(interceptedObject.GetType().ToString()).Returns(logger);

            windsorContainer.Resolve<IInterface>().GetSomethingWithParams(10, true);

            logger.Received().Log(
                LogLevel.Debug,
                new StringBuilder()
                    .AppendLine("Calling method: 'GetSomethingWithParams' with parameters")
                    .AppendLine("\tSystem.Int32 param: 10")
                    .Append("\tSystem.Boolean boolean: True")
                        .ToString());
        }

        [Test]
        public void Should_log_exception_thrown_from_method()
        {
            LogProvider.LoggerProvider.Create(interceptedObject.GetType().ToString()).Returns(logger);

            var expectedException = new Exception("Foo");

            interceptedObject.When(x => x.GetSomething()).Throw(expectedException);

            windsorContainer.Resolve<IInterface>().Invoking(x => x.GetSomething()).ShouldThrow<Exception>().And.Message.Should().Be("Foo");

            logger.Received().Log(LogLevel.Error, expectedException, "Method 'GetSomething' failed with exception 'System.Exception'");
        }

        internal interface IInterface
        {
            object GetSomething();

            object GetSomethingWithParams(int param, bool boolean);
        }

        public ILoggerProvider LoggerProvider { get; set; }
    }
}