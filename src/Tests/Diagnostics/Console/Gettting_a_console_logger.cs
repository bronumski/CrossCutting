using FluentAssertions;
using NUnit.Framework;

namespace CrossCutting.Diagnostics.Console
{
    class Gettting_a_console_logger
    {
        [Test]
        public void Should_get_an_instance_of_a_console_logger()
        {
            new ConsoleLoggerProvider().Create(null).Should().BeOfType<ConsoleLogger>();
        }
    }
}