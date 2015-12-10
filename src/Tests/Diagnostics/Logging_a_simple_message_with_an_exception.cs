using System;
using NSubstitute;
using NUnit.Framework;

namespace CrossCutting.Diagnostics
{
    class Logging_a_simple_message_with_an_exception
    {
        [TestCaseSource(typeof(LoggingExtensionTestCases), "GetTestCases")]
        public void Should_not_log_if_level_is_disabled(LogLevelTester logLevelTester)
        {
            var logger = Substitute.For<ILogger>();
            logger.LevelEnabled(logLevelTester.LogLevel).Returns(false);

            logLevelTester.LogMessageWithExceptionAction(logger, new Exception(), "Foo");

            logger.DidNotReceive().Log(Arg.Any<LogLevel>(), Arg.Any<Exception>(), Arg.Any<string>());
        }

        [TestCaseSource(typeof(LoggingExtensionTestCases), "GetTestCases")]
        public void Should_log_if_debug_is_enabled(LogLevelTester logLevelTester)
        {
            var logger = Substitute.For<ILogger>();
            logger.LevelEnabled(logLevelTester.LogLevel).Returns(true);

            var expectedException = new Exception();

            logLevelTester.LogMessageWithExceptionAction(logger, expectedException,  "Foo");

            logger.Received().Log(logLevelTester.LogLevel, expectedException, "Foo");
        }
    }
}