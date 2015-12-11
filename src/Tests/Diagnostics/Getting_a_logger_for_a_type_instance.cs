using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace CrossCutting.Diagnostics
{
    class Getting_a_logger_for_a_type_instance : IRequireStubedLoggerProvider
    {
        [Test]
        public void Should_get_a_logger_for_a_type()
        {
            var expectedLogger = Substitute.For<ILogger>();

            LoggerProvider.Create("System.Object").Returns(expectedLogger);

            new object().Log().Should().BeSameAs(expectedLogger);
        }

        public ILoggerProvider LoggerProvider { get; set; }
    }
}