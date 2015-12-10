using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace CrossCutting.Diagnostics
{
    class Getting_a_logger_for_a_type
    {
        [Test]
        public void Should_get_a_logger_for_a_type()
        {
            var expectedLogger = Substitute.For<ILogger>();
            var logProvider = Substitute.For<ILoggerProvider>();

            LoggerProviderFactories.SetLoggingProvider(logProvider);

            logProvider.Create("System.Object").Returns(expectedLogger);

            new object().Log().Should().BeSameAs(expectedLogger);
        }
    }
}