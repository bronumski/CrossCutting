using System;
using System.Linq;

namespace CrossCutting.Diagnostics
{
    public static partial class LoggingExtensions
    {
        public static void Error(this ILogger logger, Func<string> messageBuilder)
        {
            logger.SafeLogIfEnabled(LogLevel.Error, messageBuilder);
        }
        public static void Error(this ILogger logger, Exception exception, Func<string> messageBuilder)
        {
            logger.SafeLogIfEnabled(LogLevel.Error, exception, messageBuilder);
        }

        public static void Error(this ILogger logger, string message)
        {
            logger.SafeLogIfEnabled(LogLevel.Error, () => message);
        }

        public static void Error(this ILogger logger, Exception exception, string message)
        {
            logger.SafeLogIfEnabled(LogLevel.Error, exception, () => message);
        }
        public static void ErrorFormat(this ILogger logger, string format, params object[] args)
        {
            logger.SafeLogIfEnabled(LogLevel.Error, () => RenderLogMessage(format, args.Select(x => new Func<object>(() => x))));
        }

        public static void ErrorFormat(this ILogger logger, Exception exception, string format, params object[] args)
        {
            logger.SafeLogIfEnabled(LogLevel.Error, exception, () => RenderLogMessage(format, args.Select(x => new Func<object>(() => x))));
        }

        public static void ErrorFormat(this ILogger logger, Exception exception, string format, params Func<object>[] args)
        {
            logger.SafeLogIfEnabled(LogLevel.Error, exception, () => RenderLogMessage(format, args));
        }

        public static void ErrorFormat(this ILogger logger, string format, params Func<object>[] args)
        {
            logger.SafeLogIfEnabled(LogLevel.Error, () => RenderLogMessage(format, args));
        }
    }
}