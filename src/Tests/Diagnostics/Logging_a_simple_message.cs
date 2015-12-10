using NSubstitute;
using NUnit.Framework;

namespace CrossCutting.Diagnostics
{
    class Logging_a_simple_message
    {
        [TestCaseSource(typeof(LoggingExtensionTestCases), "GetTestCases")]
        public void Should_not_log_if_level_is_disabled(LogLevelTester logLevelTester)
        {
            var logger = Substitute.For<ILogger>();
            logger.LevelEnabled(logLevelTester.LogLevel).Returns(false);

            logLevelTester.LogMessageAction(logger, "Foo");

            logger.DidNotReceive().Log(Arg.Any<LogLevel>(), Arg.Any<string>());
        }

        [TestCaseSource(typeof(LoggingExtensionTestCases), "GetTestCases")]
        public void Should_log_if_debug_is_enabled(LogLevelTester logLevelTester)
        {
            var logger = Substitute.For<ILogger>();
            logger.LevelEnabled(logLevelTester.LogLevel).Returns(true);

            logLevelTester.LogMessageAction(logger, "Foo");

            logger.Received().Log(logLevelTester.LogLevel, "Foo");
        }
    }
}