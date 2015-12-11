using System;
using NSubstitute;
using NUnit.Framework;

namespace CrossCutting.Diagnostics
{
    class Logging_a_simple_string_formated_message
    {
        [TearDown]
        public void TearDown()
        {
            LogProvider.Reset();
        }
        
        [TestCaseSource(typeof(LoggingExtensionTestCases), "GetTestCases")]
        public void Should_not_log_if_level_is_disabled(LogLevelTester logLevelTester)
        {
            var logger = Substitute.For<ILogger>();
            logger.LevelEnabled(logLevelTester.LogLevel).Returns(false);

            logLevelTester.LogFormatAction(logger, "{0}", new object[] { "Foo" });

            logger.DidNotReceive().Log(Arg.Any<LogLevel>(), Arg.Any<string>());
        }

        [TestCaseSource(typeof(LoggingExtensionTestCases), "GetTestCases")]
        public void Should_log_if_debug_is_enabled(LogLevelTester logLevelTester)
        {
            var logger = Substitute.For<ILogger>();
            logger.LevelEnabled(logLevelTester.LogLevel).Returns(true);

            logLevelTester.LogFormatAction(logger, "{0}", new object[] { "Foo" });

            logger.Received().Log(logLevelTester.LogLevel, "Foo");
        }

        [TestCaseSource(typeof(LoggingExtensionTestCases), "GetTestCases")]
        public void Should_not_blow_up_if_resolving_an_argument_fails(LogLevelTester logLevelTester)
        {
            var faileOverLogger = Substitute.For<ILogger>();
            var loggerProvider = Substitute.For<ILoggerProvider>();
            loggerProvider.Create(Arg.Any<string>()).Returns(faileOverLogger);
            LogProvider.SetLoggingProvider(loggerProvider);

            var logger = Substitute.For<ILogger>();
            logger.LevelEnabled(logLevelTester.LogLevel).Returns(true);

            logLevelTester.LogFormatAction(logger, "Foo{0}", new object[] { new TestObject() });

            logger.Received().Log(logLevelTester.LogLevel, "Foo<Failed to get argument>");
            faileOverLogger.Received().Log(LogLevel.Warn, Arg.Any<NotImplementedException>(), "Failed to extract log message argument");
        }

        [TestCaseSource(typeof(LoggingExtensionTestCases), "GetTestCases")]
        public void Should_not_blow_up_if_format_fails(LogLevelTester logLevelTester)
        {
            var faileOverLogger = Substitute.For<ILogger>();
            var loggerProvider = Substitute.For<ILoggerProvider>();
            loggerProvider.Create(Arg.Any<string>()).Returns(faileOverLogger);

            LogProvider.SetLoggingProvider(loggerProvider);

            var logger = Substitute.For<ILogger>();
            logger.LevelEnabled(logLevelTester.LogLevel).Returns(true);

            logLevelTester.LogFormatAction(logger, "Foo{1}", new object[] { "Bar" });

            faileOverLogger.Received().Log(LogLevel.Warn, Arg.Any<FormatException>(), "Failed to log message");
        }

        class TestObject
        {
            public override string ToString()
            {
                throw new NotImplementedException();
            }
        }
    }
}