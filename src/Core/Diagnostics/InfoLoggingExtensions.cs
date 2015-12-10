using System;
using System.Linq;

namespace CrossCutting.Diagnostics
{
    public static partial class LoggingExtensions
    {
        public static void Info(this ILogger logger, Func<string> messageBuilder)
        {
            logger.SafeLogIfEnabled(LogLevel.Info, messageBuilder);
        }
        public static void Info(this ILogger logger, Exception exception, Func<string> messageBuilder)
        {
            logger.SafeLogIfEnabled(LogLevel.Info, exception, messageBuilder);
        }

        public static void Info(this ILogger logger, string message)
        {
            logger.SafeLogIfEnabled(LogLevel.Info, () => message);
        }

        public static void Info(this ILogger logger, Exception exception, string message)
        {
            logger.SafeLogIfEnabled(LogLevel.Info, exception, () => message);
        }

        public static void InfoFormat(this ILogger logger, string format, params object[] args)
        {
            logger.SafeLogIfEnabled(LogLevel.Info, () => RenderLogMessage(format, args.Select(x => new Func<object>(() => x))));
        }

        public static void InfoFormat(this ILogger logger, Exception exception, string format, params object[] args)
        {
            logger.SafeLogIfEnabled(LogLevel.Info, exception, () => RenderLogMessage(format, args.Select(x => new Func<object>(() => x))));
        }

        public static void InfoFormat(this ILogger logger, Exception exception, string format, params Func<object>[] args)
        {
            logger.SafeLogIfEnabled(LogLevel.Info, exception, () => RenderLogMessage(format, args));
        }

        public static void InfoFormat(this ILogger logger, string format, params Func<object>[] args)
        {
            logger.SafeLogIfEnabled(LogLevel.Info, () => RenderLogMessage(format, args));
        }
    }
}