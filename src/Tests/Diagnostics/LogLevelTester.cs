using System;

namespace CrossCutting.Diagnostics
{
    class LogLevelTester
    {
        public LogLevelTester(
            LogLevel logLevel,
            Action<ILogger, string> logMessageAction,
            Action<ILogger, Exception, string> logMessageWithExceptionAction,
            Action<ILogger, string, object[]> logFormatAction,
            Action<ILogger, Exception, string, object[]> logFormatWithExceptionAction,
            Action<ILogger, string, Func<object>[]> logFormatFuncAction,
            Action<ILogger, Exception, string, Func<object>[]> logFormatFuncWithExceptionAction,
            Action<ILogger, Func<string>> logFunctionAction,
            Action<ILogger, Exception, Func<string>> logFunctionWithExceptionAction)
        {
            LogLevel = logLevel;
            LogMessageAction = logMessageAction;
            LogFormatAction = logFormatAction;
            LogFormatFuncAction = logFormatFuncAction;
            LogFormatWithExceptionAction = logFormatWithExceptionAction;
            LogFormatFuncWithExceptionAction = logFormatFuncWithExceptionAction;
            LogFunctionAction = logFunctionAction;
            LogFunctionWithExceptionAction = logFunctionWithExceptionAction;
            LogMessageWithExceptionAction = logMessageWithExceptionAction;
        }

        public LogLevel LogLevel { get; private set; }
        public Action<ILogger, string> LogMessageAction { get; private set; }
        public Action<ILogger, Exception, string> LogMessageWithExceptionAction { get; private set; }
        public Action<ILogger, string, object[]> LogFormatAction { get; private set; }
        public Action<ILogger, Exception, string, object[]> LogFormatWithExceptionAction { get; private set; }
        public Action<ILogger, string, Func<object>[]> LogFormatFuncAction { get; private set; }
        public Action<ILogger, Exception, string, Func<object>[]> LogFormatFuncWithExceptionAction { get; private set; }
        public Action<ILogger, Func<string>> LogFunctionAction { get; private set; }
        public Action<ILogger, Exception, Func<string>> LogFunctionWithExceptionAction { get; private set; }
    }
}