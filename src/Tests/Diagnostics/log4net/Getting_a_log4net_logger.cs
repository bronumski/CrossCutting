using FluentAssertions;
using NUnit.Framework;

namespace CrossCutting.Diagnostics.log4net
{
    public class Getting_a_log4net_logger
    {
        [Test]
        public void Should_get_logger()
        {
            new Log4NetLoggerProvider().Create("foo").Should().BeOfType<Log4NetLogger>();
        }
    }
}   