using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Diagnostics
{
    public static partial class LoggingExtensions
    {
        public static ILogger Log(this object subject)
        {
            return LogProvider.LoggerProvider.Create(subject == null ? string.Empty : subject.GetType().ToString());
        }

        private static void SafeLogIfEnabled(this ILogger logger, LogLevel logLevel, Func<string> message)
        {
            logger.SafeLogIfEnabled(logLevel, null, message);
        }

        private static void SafeLogIfEnabled(this ILogger logger, LogLevel logLevel, Exception exception, Func<string> messageBuilder)
        {
            if (!logger.LevelEnabled(logLevel)) return;

            if (exception == null)
            {
                try
                {
                    logger.Log(logLevel, messageBuilder());
                }
                catch (Exception ex)
                {
                    InternalLog("Failed to log message", ex);
                }
            }
            else
            {
                string message;
                try
                {
                    message = messageBuilder();
                }
                catch (Exception ex)
                {
                    InternalLog("Failed to generate log message", ex);
                    message =
                        "Log message failed when formating, exception retained. See log warning message for details.";

                }
                logger.Log(logLevel, exception, message);
            }

        }

        private static string RenderLogMessage(string format, IEnumerable<Func<object>> args)
        {
            object[] evaluatedArgs = args.Select(SafeGetArg).ToArray();

            return string.Format(format, evaluatedArgs);
        }

        private static object SafeGetArg(Func<object> arg)
        {
            try
            {
                return arg().ToString();
            }
            catch (Exception ex)
            {
                InternalLog("Failed to extract log message argument", ex);

                return "<Failed to get argument>";
            }
        }

        private static void InternalLog(string message, Exception ex)
        {
            LogProvider.LoggerProvider.Create(typeof (LoggingExtensions).ToString())
                .Log(LogLevel.Warn, ex, message);
        }
    }
}