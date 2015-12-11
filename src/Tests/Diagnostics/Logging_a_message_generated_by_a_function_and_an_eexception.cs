using System;
using NSubstitute;
using NUnit.Framework;

namespace CrossCutting.Diagnostics
{
    class Logging_a_message_generated_by_a_function_and_an_eexception : IRequireStubedLoggerProvider
    {
        [TearDown]
        public void TearDown()
        {
            LogProvider.Reset();
        }

        [TestCaseSource(typeof(LoggingExtensionTestCases), "GetTestCases")]
        public void Should_not_resolve_message_if_log_level_is_disabled(LogLevelTester logLevelTester)
        {
            var @interface = Substitute.For<IInterface>();
            var logger = Substitute.For<ILogger>();
            logger.LevelEnabled(logLevelTester.LogLevel).Returns(false);

            logLevelTester.LogFunctionWithExceptionAction(logger, new Exception(), () => @interface.GetValue());

            logger.DidNotReceive().Log(Arg.Any<LogLevel>(), Arg.Any<string>());
        }

        [TestCaseSource(typeof(LoggingExtensionTestCases), "GetTestCases")]
        public void Should_log_message_if_log_level_is_enabled(LogLevelTester logLevelTester)
        {
            var logger = Substitute.For<ILogger>();
            logger.LevelEnabled(logLevelTester.LogLevel).Returns(true);

            var expectedException = new Exception();
            logLevelTester.LogFunctionWithExceptionAction(logger, expectedException, () => "Foo");

            logger.Received().Log(logLevelTester.LogLevel, expectedException, "Foo");
        }

        [TestCaseSource(typeof(LoggingExtensionTestCases), "GetTestCases")]
        public void Should_not_fail_if_building_log_message_throws_an_exception(LogLevelTester logLevelTester)
        {
            var faileOverLogger = Substitute.For<ILogger>();
            LoggerProvider.Create(Arg.Any<string>()).Returns(faileOverLogger);
            
            var logger = Substitute.For<ILogger>();
            logger.LevelEnabled(logLevelTester.LogLevel).Returns(true);
            var expectedException = new Exception();
            logLevelTester.LogFunctionWithExceptionAction(logger, expectedException, () => { throw expectedException; });

            logger.Received().Log(logLevelTester.LogLevel, expectedException, "Log message failed when formating, exception retained. See log warning message for details.");
            faileOverLogger.Received().Log(LogLevel.Warn, expectedException, "Failed to generate log message");
        }
        
        internal interface IInterface
        {
            string GetValue();
        }

        public ILoggerProvider LoggerProvider { get; set; }
    }
}