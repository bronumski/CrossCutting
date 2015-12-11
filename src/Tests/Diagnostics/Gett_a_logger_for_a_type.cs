using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace CrossCutting.Diagnostics
{
    class Gett_a_logger_for_a_type
    {
        [TearDown]
        public void TearDown()
        {
            LogProvider.Reset();
        }

        [Test]
        public void Should_get_a_logger_for_a_type()
        {
            var expectedLogger = Substitute.For<ILogger>();
            var logProvider = Substitute.For<ILoggerProvider>();

            LogProvider.SetLoggingProvider(logProvider);

            logProvider.Create("System.Object").Returns(expectedLogger);

            LogProvider.LoggerFor<object>().Should().BeSameAs(expectedLogger);
        }
    }
}