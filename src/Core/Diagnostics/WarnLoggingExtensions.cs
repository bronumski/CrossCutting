using System;
using System.Linq;

namespace CrossCutting.Diagnostics
{
    public static partial class LoggingExtensions
    {
        public static void Warn(this ILogger logger, Func<string> messageBuilder)
        {
            logger.SafeLogIfEnabled(LogLevel.Warn, messageBuilder);
        }
        public static void Warn(this ILogger logger, Exception exception, Func<string> messageBuilder)
        {
            logger.SafeLogIfEnabled(LogLevel.Warn, exception, messageBuilder);
        }

        public static void Warn(this ILogger logger, string message)
        {
            logger.SafeLogIfEnabled(LogLevel.Warn, () => message);
        }

        public static void Warn(this ILogger logger, Exception exception, string message)
        {
            logger.SafeLogIfEnabled(LogLevel.Warn, exception, () => message);
        }
        public static void WarnFormat(this ILogger logger, string format, params object[] args)
        {
            logger.SafeLogIfEnabled(LogLevel.Warn, () => RenderLogMessage(format, args.Select(x => new Func<object>(() => x))));
        }

        public static void WarnFormat(this ILogger logger, Exception exception, string format, params object[] args)
        {
            logger.SafeLogIfEnabled(LogLevel.Warn, exception, () => RenderLogMessage(format, args.Select(x => new Func<object>(() => x))));
        }

        public static void WarnFormat(this ILogger logger, Exception exception, string format, params Func<object>[] args)
        {
            logger.SafeLogIfEnabled(LogLevel.Warn, exception, () => RenderLogMessage(format, args));
        }

        public static void WarnFormat(this ILogger logger, string format, params Func<object>[] args)
        {
            logger.SafeLogIfEnabled(LogLevel.Warn, () => RenderLogMessage(format, args));
        }
    }
}