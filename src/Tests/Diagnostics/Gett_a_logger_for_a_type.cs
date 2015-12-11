using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace CrossCutting.Diagnostics
{
    class Gett_a_logger_for_a_type : IRequireStubedLoggerProvider
    {
        [Test]
        public void Should_get_a_logger_for_a_type()
        {
            var expectedLogger = Substitute.For<ILogger>();

            LoggerProvider.Create("System.Object").Returns(expectedLogger);

            LogProvider.LoggerFor<object>().Should().BeSameAs(expectedLogger);
        }

        public ILoggerProvider LoggerProvider { get; set; }
    }
}