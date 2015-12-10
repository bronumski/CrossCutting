using System;
using log4net;
using log4net.Core;

namespace CrossCutting.Diagnostics
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog log4NetLog;

        public Log4NetLogger(ILog log4NetLog)
        {
            this.log4NetLog = log4NetLog;
        }

        public void Log(LogLevel level, string message)
        {
            log4NetLog.Logger.Log(new LoggingEvent(new LoggingEventData
            {
                LoggerName = log4NetLog.Logger.Name,
                Level = MapLogLevel(level),
                Message = message,
                TimeStamp = DateTime.UtcNow
            }));
        }

        public void Log(LogLevel level, Exception exception, string message)
        {
            log4NetLog.Logger.Log(new LoggingEvent(null, null, log4NetLog.Logger.Name, MapLogLevel(level), message, exception));
        }

        public bool LevelEnabled(LogLevel level)
        {
            return log4NetLog.Logger.IsEnabledFor(MapLogLevel(level));
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