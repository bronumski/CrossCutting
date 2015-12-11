using System;
using NSubstitute;
using NUnit.Framework;

namespace CrossCutting.Diagnostics
{
    class Logging_a_simple_string_formated_message_with_exception : IRequireStubedLoggerProvider
    {
        [TestCaseSource(typeof(LoggingExtensionTestCases), "GetTestCases")]
        public void Should_not_log_if_level_is_disabled(LogLevelTester logLevelTester)
        {
            var logger = Substitute.For<ILogger>();
            logger.LevelEnabled(logLevelTester.LogLevel).Returns(false);

            logLevelTester.LogFormatWithExceptionAction(logger, new Exception(), "{0}",  new object[] { "Foo" });

            logger.DidNotReceive().Log(Arg.Any<LogLevel>(), Arg.Any<Exception>(), Arg.Any<string>());
        }

        [TestCaseSource(typeof(LoggingExtensionTestCases), "GetTestCases")]
        public void Should_log_if_debug_is_enabled(LogLevelTester logLevelTester)
        {
            var logger = Substitute.For<ILogger>();
            logger.LevelEnabled(logLevelTester.LogLevel).Returns(true);

            var expectedException = new Exception();
            logLevelTester.LogFormatWithExceptionAction(logger, expectedException, "{0}", new object[] { "Foo" });

            logger.Received().Log(logLevelTester.LogLevel, expectedException, "Foo");
        }

        [TestCaseSource(typeof(LoggingExtensionTestCases), "GetTestCases")]
        public void Should_not_blow_up_if_resolving_an_argument_fails(LogLevelTester logLevelTester)
        {
            var faileOverLogger = Substitute.For<ILogger>();
            LoggerProvider.Create(Arg.Any<string>()).Returns(faileOverLogger);

            var logger = Substitute.For<ILogger>();
            logger.LevelEnabled(logLevelTester.LogLevel).Returns(true);

            var expectedException = new Exception();
            logLevelTester.LogFormatWithExceptionAction(logger, expectedException, "Foo{0}", new object[] { new TestObject() });

            logger.Received().Log(logLevelTester.LogLevel, expectedException, "Foo<Failed to get argument>");
            faileOverLogger.Received().Log(LogLevel.Warn, Arg.Any<NotImplementedException>(), "Failed to extract log message argument");
        }

        [TestCaseSource(typeof(LoggingExtensionTestCases), "GetTestCases")]
        public void Should_not_blow_up_if_format_fails(LogLevelTester logLevelTester)
        {
            var faileOverLogger = Substitute.For<ILogger>();
            LoggerProvider.Create(Arg.Any<string>()).Returns(faileOverLogger);

            var logger = Substitute.For<ILogger>();
            logger.LevelEnabled(logLevelTester.LogLevel).Returns(true);
            
            var expectedException = new Exception();
            logLevelTester.LogFormatWithExceptionAction(logger, expectedException, "Foo{1}", new object[] { "Bar" });

            logger.Received().Log(logLevelTester.LogLevel, expectedException, "Log message failed when formating, exception retained. See log warning message for details.");

            faileOverLogger.Received().Log(LogLevel.Warn, Arg.Any<FormatException>(), "Failed to generate log message");

        }

        class TestObject
        {
            public override string ToString()
            {
                throw new NotImplementedException();
            }
        }

        public ILoggerProvider LoggerProvider { get; set; }
    }
}