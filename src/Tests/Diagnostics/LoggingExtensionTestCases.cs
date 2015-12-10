using System.Collections.Generic;
using NUnit.Framework;

namespace CrossCutting.Diagnostics
{
    static class LoggingExtensionTestCases
    {
        public static IEnumerable<ITestCaseData> GetTestCases()
        {
            yield return new TestCaseData(new LogLevelTester(
                LogLevel.Debug,
                (logger, message) => logger.Debug(message),
                (logger, exception, message) => logger.Debug(exception, message),
                (logger, format, args) => logger.DebugFormat(format, args),
                (logger, exception, format, args) => logger.DebugFormat(exception, format, args),
                (logger, format, args) => logger.DebugFormat(format, args),
                (logger, exception, format, args) => logger.DebugFormat(exception, format, args),
                (logger, func) => logger.Debug(func),
                (logger, exception, func) => logger.Debug(exception, func))).SetName("Debug");

            yield return new TestCaseData(new LogLevelTester(
                LogLevel.Info,
                (logger, message) => logger.Info(message),
                (logger, exception, message) => logger.Info(exception, message),
                (logger, format, args) => logger.InfoFormat(format, args),
                (logger, exception, format, args) => logger.InfoFormat(exception, format, args),
                (logger, format, args) => logger.InfoFormat(format, args),
                (logger, exception, format, args) => logger.InfoFormat(exception, format, args),
                (logger, func) => logger.Info(func),
                (logger, exception, func) => logger.Info(exception, func))).SetName("Info");

            yield return new TestCaseData(new LogLevelTester(
                LogLevel.Warn,
                (logger, message) => logger.Warn(message),
                (logger, exception, message) => logger.Warn(exception, message),
                (logger, format, args) => logger.WarnFormat(format, args),
                (logger, exception, format, args) => logger.WarnFormat(exception, format, args),
                (logger, format, args) => logger.WarnFormat(format, args),
                (logger, exception, format, args) => logger.WarnFormat(exception, format, args),
                (logger, func) => logger.Warn(func),
                (logger, exception, func) => logger.Warn(exception, func))).SetName("Warn");
            
            yield return new TestCaseData(new LogLevelTester(
                LogLevel.Error,
                (logger, message) => logger.Error(message),
                (logger, exception, message) => logger.Error(exception, message),
                (logger, format, args) => logger.ErrorFormat(format, args),
                (logger, exception, format, args) => logger.ErrorFormat(exception, format, args),
                (logger, format, args) => logger.ErrorFormat(format, args),
                (logger, exception, format, args) => logger.ErrorFormat(exception, format, args),
                (logger, func) => logger.Error(func),
                (logger, exception, func) => logger.Error(exception, func))).SetName("Error");

            yield return new TestCaseData(new LogLevelTester(
                LogLevel.Fatal,
                (logger, message) => logger.Fatal(message),
                (logger, exception, message) => logger.Fatal(exception, message),
                (logger, format, args) => logger.FatalFormat(format, args),
                (logger, exception, format, args) => logger.FatalFormat(exception, format, args),
                (logger, format, args) => logger.FatalFormat(format, args),
                (logger, exception, format, args) => logger.FatalFormat(exception, format, args),
                (logger, func) => logger.Fatal(func),
                (logger, exception, func) => logger.Fatal(exception, func))).SetName("Fatal");

        }
    }
}