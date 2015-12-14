using System;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace CrossCutting.Diagnostics.Windsor
{
    class Intercepting_void_method_calls : IRequireStubedLoggerProvider
    {
        private ILogger logger;
        private IInterface interceptedObject;
        private IWindsorContainer windsorContainer;

        [SetUp]
        public void SetUp()
        {
            interceptedObject = Substitute.For<IInterface>();
            
            logger = Substitute.For<ILogger>();

            logger.LevelEnabled(LogLevel.Debug).Returns(true);
            logger.LevelEnabled(LogLevel.Error).Returns(true);

            windsorContainer = new WindsorContainer()
                .AddFacility<LoggingFacility>()
                .Register(Component.For<IInterface>().UsingFactoryMethod((x, c) => interceptedObject).Interceptors<LoggingInterceptor>());
        }

        [Test]
        public void Should_log_call_when_method_is_called()
        {
            LogProvider.LoggerProvider.Create(interceptedObject.GetType().ToString()).Returns(logger);

            windsorContainer.Resolve<IInterface>().DoSomething();

            logger.Received().Log(LogLevel.Debug, "Calling method: 'DoSomething'");
        }

        [Test]
        public void Should_call_method_on_underlying_object()
        {
            LogProvider.LoggerProvider.Create(interceptedObject.GetType().ToString()).Returns(logger);
               
            windsorContainer.Resolve<IInterface>().DoSomething();

            interceptedObject.Received().DoSomething();
        }

        [Test]
        public void Should_log_the_method_invocation_has_completed_and_the_time_it_took()
        {
            LogProvider.LoggerProvider.Create(interceptedObject.GetType().ToString()).Returns(logger);

            windsorContainer.Resolve<IInterface>().DoSomething();

            logger.Received().Log(LogLevel.Debug, Arg.Is<string>(x => x.StartsWith("Method 'DoSomething' completed in '")));
        }

        [Test]
        public void Should_log_method_parameters()
        {
            LogProvider.LoggerProvider.Create(interceptedObject.GetType().ToString()).Returns(logger);

            windsorContainer.Resolve<IInterface>().DoSomethingWithParams(10, true);

            logger.Received().Log(
                LogLevel.Debug,
                new StringBuilder()
                    .AppendLine("Calling method: 'DoSomethingWithParams' with parameters")
                    .AppendLine("\tSystem.Int32 param: 10")
                    .Append("\tSystem.Boolean boolean: True")
                        .ToString());
        }

        [Test]
        public void Should_log_exception_thrown_from_method()
        {
            LogProvider.LoggerProvider.Create(interceptedObject.GetType().ToString()).Returns(logger);

            var expectedException = new Exception("Foo");

            interceptedObject.When(x => x.DoSomething()).Throw(expectedException);
            
            windsorContainer.Resolve<IInterface>().Invoking(x => x.DoSomething()).ShouldThrow<Exception>().And.Message.Should().Be("Foo");

            logger.Received().Log(LogLevel.Error, expectedException, "Method 'DoSomething' failed with exception 'System.Exception'");
        }

        internal interface IInterface
        {
            void DoSomething();

            void DoSomethingWithParams(int param, bool boolean);
        }

        public ILoggerProvider LoggerProvider { get; set; }
    }
}