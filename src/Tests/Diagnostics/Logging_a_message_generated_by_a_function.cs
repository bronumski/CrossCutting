using System;
using NSubstitute;
using NUnit.Framework;

namespace CrossCutting.Diagnostics
{
    class Logging_a_message_generated_by_a_function
    {
        [TestCaseSource(typeof(LoggingExtensionTestCases), "GetTestCases")]
        public void Should_not_resolve_message_if_log_level_is_disabled(LogLevelTester logLevelTester)
        {
            var @interface = Substitute.For<IInterface>();
            var logger = Substitute.For<ILogger>();
            logger.LevelEnabled(logLevelTester.LogLevel).Returns(false);

            logLevelTester.LogFunctionAction(logger, () => @interface.GetValue());

            logger.DidNotReceive().Log(Arg.Any<LogLevel>(), Arg.Any<string>());
        }

        [TestCaseSource(typeof(LoggingExtensionTestCases), "GetTestCases")]
        public void Should_log_message_if_log_level_is_enabled(LogLevelTester logLevelTester)
        {
            var logger = Substitute.For<ILogger>();
            logger.LevelEnabled(logLevelTester.LogLevel).Returns(true);

            logLevelTester.LogFunctionAction(logger, () => "Foo");

            logger.Received().Log(logLevelTester.LogLevel,"Foo");
        }

        [TestCaseSource(typeof(LoggingExtensionTestCases), "GetTestCases")]
        public void Should_not_fail_if_building_log_message_throws_an_exception(LogLevelTester logLevelTester)
        {
            var faileOverLogger = Substitute.For<ILogger>();
            var loggerProvider = Substitute.For<ILoggerProvider>();
            loggerProvider.Create(Arg.Any<string>()).Returns(faileOverLogger);
            
            LoggerProviderFactories.SetLoggingProvider(loggerProvider);
            
            var logger = Substitute.For<ILogger>();
            logger.LevelEnabled(logLevelTester.LogLevel).Returns(true);
            var expectedException = new Exception();
            logLevelTester.LogFunctionAction(logger, () => { throw expectedException; });

            faileOverLogger.Received().Log(LogLevel.Warn, expectedException, "Failed to log message");
        }
        
        internal interface IInterface
        {
            string GetValue();
        }
    }
}