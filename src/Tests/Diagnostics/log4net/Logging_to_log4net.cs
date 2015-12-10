using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FluentAssertions;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using log4net.Util;
using NSubstitute;
using NUnit.Framework;

namespace CrossCutting.Diagnostics.log4net
{
    class Logging_to_log4net
    {
        private IAppender testAppender;

        [SetUp]
        public void SetUp()
        {
            var hierarchy = (Hierarchy)LogManager.GetRepository();

            var patternLayout = new PatternLayout
            {
                ConversionPattern = "%date [%thread] %-5level %logger - %message%newline"
            };
            patternLayout.ActivateOptions();

            testAppender = Substitute.For<IAppender>();
            
            hierarchy.Root.AddAppender(testAppender);

            hierarchy.Root.Level = Level.All;
            hierarchy.Configured = true;
        }

        [TestCaseSource("LogLevels")]
        public void Should_get_log4net_logging_level(LogLevel logLevel)
        {
            var log4NetLog = Substitute.For<ILog>();
            log4NetLog.Logger.IsEnabledFor(Arg.Any<Level>()).Returns(true);

            var log4NetLogger = new Log4NetLogger(log4NetLog);

            log4NetLogger.LevelEnabled(logLevel).Should().BeTrue();
        }

        [TestCaseSource("LogLevels")]
        public void Should_log_message(LogLevel logLevel)
        {
            var dateTime = DateTime.UtcNow;

            var log4NetLogger = new Log4NetLoggerProvider().Create("Foo Logger");

            log4NetLogger.Log(logLevel, "Foo");

            testAppender.Received().DoAppend(Arg.Is<LoggingEvent>(loggingEvent => ValidateLoggingEvent(loggingEvent, logLevel, dateTime, null)));
        }

        [TestCaseSource("LogLevels")]
        public void Should_log_message_with_an_exception(LogLevel logLevel)
        {
            var dateTime = DateTime.UtcNow;
            var log4NetLogger = new Log4NetLoggerProvider().Create("Foo Logger");

            var expectedException = new Exception();
            log4NetLogger.Log(logLevel, expectedException, "Foo");

            testAppender.Received().DoAppend(Arg.Is<LoggingEvent>(loggingEvent => ValidateLoggingEvent(loggingEvent, logLevel, dateTime, expectedException)));
        }

        private bool ValidateLoggingEvent(LoggingEvent loggingEvent, LogLevel logLevel, DateTime dateTime, Exception exception)
        {
            return
                loggingEvent.LoggerName == "Foo Logger" &&
                loggingEvent.Level == MapLogLevel(logLevel) &&
                loggingEvent.RenderedMessage == "Foo" &&
                loggingEvent.TimeStamp >= dateTime &&
                loggingEvent.ExceptionObject == exception &&
                loggingEvent.ThreadName == Thread.CurrentThread.Name;
        }

        private IEnumerable<ITestCaseData> LogLevels()
        {
            return (from LogLevel logLevel in Enum.GetValues(typeof(LogLevel)) select new TestCaseData(logLevel).SetName(logLevel.ToString()));
        }

        private Level MapLogLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    return Level.Debug;
                case LogLevel.Info:
                    return Level.Info;
                case LogLevel.Warn:
                    return Level.Warn;
                case LogLevel.Error:
                    return Level.Error;
                case LogLevel.Fatal:
                    return Level.Fatal;
                default:
                    return Level.Off;
            }
        }
    }
}