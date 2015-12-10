using System;
using NSubstitute;
using NUnit.Framework;

namespace CrossCutting.Diagnostics
{
    class Logging_with_functionalized_arguments_and_exception
    {
        [TestCaseSource(typeof(LoggingExtensionTestCases), "GetTestCases")]
        public void Should_log_formated_message(LogLevelTester logLevelTester)
        {
            var logger = Substitute.For<ILogger>();
            logger.LevelEnabled(logLevelTester.LogLevel).Returns(true);

            var expectedException = new Exception();

            logLevelTester.LogFormatFuncWithExceptionAction(logger, expectedException,  "Foo {0}", new Func<object>[] { () => (object)"bar" });

            logger.Received().Log(logLevelTester.LogLevel, expectedException, "Foo bar");
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

            var expectedException = new Exception();

            logLevelTester.LogFormatFuncWithExceptionAction(logger, expectedException, "Foo{1}", new Func<object>[] { () => new object() });

            logger.Received().Log(logLevelTester.LogLevel, expectedException, "Log message failed when formating, exception retained. See log warning message for details.");

            faileOverLogger.Received().Log(LogLevel.Warn, Arg.Any<FormatException>(), "Failed to generate log message");
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

            var expectedException = new Exception();

            logLevelTester.LogFormatFuncWithExceptionAction(logger, expectedException, "Foo{0}", new Func<object>[] { () => { throw new Exception(); } });

            logger.Received().Log(logLevelTester.LogLevel, expectedException, "Foo<Failed to get argument>");
            faileOverLogger.Received().Log(LogLevel.Warn, Arg.Any<Exception>(), "Failed to extract log message argument");
        }

        [TestCaseSource(typeof(LoggingExtensionTestCases), "GetTestCases")]
        public void Should_not_resolve_arguments_if_debug_is_disabled(LogLevelTester logLevelTester)
        {
            var logger = Substitute.For<ILogger>();

            logger.LevelEnabled(logLevelTester.LogLevel).Returns(false);
            var @interface = Substitute.For<IInterface>();


            logLevelTester.LogFormatFuncWithExceptionAction(logger, new Exception(), "Foo{0}", new Func<object>[] { () => @interface.GetValue() });

            @interface.DidNotReceive().GetValue();
            logger.DidNotReceive().Log(Arg.Any<LogLevel>(), Arg.Any<Exception>(), Arg.Any<string>());
        }

        internal interface IInterface
        {
            string GetValue();
        }
    }
}