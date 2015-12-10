using System;
using NSubstitute;
using NUnit.Framework;

namespace CrossCutting.Diagnostics
{
    class Logging_with_functionalized_arguments
    {
        [TestCaseSource(typeof(LoggingExtensionTestCases), "GetTestCases")]
        public void Should_log_formated_message(LogLevelTester logLevelTester)
        {
            var logger = Substitute.For<ILogger>();
            logger.LevelEnabled(logLevelTester.LogLevel).Returns(true);

            logLevelTester.LogFormatFuncAction(logger, "Foo {0}", new Func<object>[] { () => (object)"bar" });

            logger.Received().Log(logLevelTester.LogLevel, "Foo bar");
        }


        [TestCaseSource(typeof(LoggingExtensionTestCases), "GetTestCases")]
        public void Should_not_blow_up_if_format_fails(LogLevelTester logLevelTester)
        {
            var faileOverLogger = Substitute.For<ILogger>();
            var loggerProvider = Substitute.For<ILoggerProvider>();
            loggerProvider.Create(Arg.Any<string>()).Returns(faileOverLogger);
            
            LoggerProviderFactories.SetLoggingProvider(loggerProvider);
            
            var logger = Substitute.For<ILogger>();
            logger.LevelEnabled(logLevelTester.LogLevel).Returns(true);

            logLevelTester.LogFormatFuncAction(logger, "Foo{1}", new Func<object>[] { () => new object() });

            faileOverLogger.Received().Log(LogLevel.Warn, Arg.Any<FormatException>(), "Failed to log message");
        }

        [TestCaseSource(typeof(LoggingExtensionTestCases), "GetTestCases")]
        public void Should_not_blow_up_if_resolving_an_argument_fails(LogLevelTester logLevelTester)
        {
            var faileOverLogger = Substitute.For<ILogger>();
            var loggerProvider = Substitute.For<ILoggerProvider>();
            loggerProvider.Create(Arg.Any<string>()).Returns(faileOverLogger);
            LoggerProviderFactories.SetLoggingProvider(loggerProvider);

            var logger = Substitute.For<ILogger>();
            logger.LevelEnabled(logLevelTester.LogLevel).Returns(true);

            logLevelTester.LogFormatFuncAction(logger, "Foo{0}", new Func<object>[] { () => { throw new Exception(); } });

            logger.Received().Log(logLevelTester.LogLevel, "Foo<Failed to get argument>");
            faileOverLogger.Received().Log(LogLevel.Warn, Arg.Any<Exception>(), "Failed to extract log message argument");
        }

        [TestCaseSource(typeof(LoggingExtensionTestCases), "GetTestCases")]
        public void Should_not_resolve_arguments_if_debug_is_disabled(LogLevelTester logLevelTester)
        {
            var logger = Substitute.For<ILogger>();

            logger.LevelEnabled(logLevelTester.LogLevel).Returns(false);
            var @interface = Substitute.For<IInterface>();

            logLevelTester.LogFormatFuncAction(logger, "Foo{0}", new Func<object>[] { () => @interface.GetValue() });
            
            @interface.DidNotReceive().GetValue();
            logger.DidNotReceive().Log(Arg.Any<LogLevel>(), Arg.Any<Exception>(), Arg.Any<string>());
        }

        internal interface IInterface
        {
            string GetValue();
        }
    }
}