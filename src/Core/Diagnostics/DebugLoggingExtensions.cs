using System;
using System.Linq;

namespace CrossCutting.Diagnostics
{
    public static partial class LoggingExtensions
    {
        public static void Debug(this ILogger logger, Func<string> messageBuilder)
        {
            logger.SafeLogIfEnabled(LogLevel.Debug, messageBuilder);
        }
        public static void Debug(this ILogger logger, Exception exception, Func<string> messageBuilder)
        {
            logger.SafeLogIfEnabled(LogLevel.Debug, exception, messageBuilder);
        }

        public static void Debug(this ILogger logger, string message)
        {
            logger.SafeLogIfEnabled(LogLevel.Debug, () => message);
        }

        public static void Debug(this ILogger logger, Exception exception, string message)
        {
            logger.SafeLogIfEnabled(LogLevel.Debug, exception, () => message);
        }
        public static void DebugFormat(this ILogger logger, string format, params object[] args)
        {
            logger.SafeLogIfEnabled(LogLevel.Debug, () => RenderLogMessage(format, args.Select(x => new Func<object>(() => x))));
        }

        public static void DebugFormat(this ILogger logger, Exception exception, string format, params object[] args)
        {
            logger.SafeLogIfEnabled(LogLevel.Debug, exception, () => RenderLogMessage(format, args.Select(x => new Func<object>(() => x))));
        }

        public static void DebugFormat(this ILogger logger, Exception exception, string format, params Func<object>[] args)
        {
            logger.SafeLogIfEnabled(LogLevel.Debug, exception, () => RenderLogMessage(format, args));
        }

        public static void DebugFormat(this ILogger logger, string format, params Func<object>[] args)
        {
            logger.SafeLogIfEnabled(LogLevel.Debug, () => RenderLogMessage(format, args));
        }
    }
}