using System;
using System.Linq;

namespace CrossCutting.Diagnostics
{
    public static partial class LoggingExtensions
    {
        public static void Fatal(this ILogger logger, Func<string> messageBuilder)
        {
            logger.SafeLogIfEnabled(LogLevel.Fatal, messageBuilder);
        }
        public static void Fatal(this ILogger logger, Exception exception, Func<string> messageBuilder)
        {
            logger.SafeLogIfEnabled(LogLevel.Fatal, exception, messageBuilder);
        }

        public static void Fatal(this ILogger logger, string message)
        {
            logger.SafeLogIfEnabled(LogLevel.Fatal, () => message);
        }

        public static void Fatal(this ILogger logger, Exception exception, string message)
        {
            logger.SafeLogIfEnabled(LogLevel.Fatal, exception, () => message);
        }
        public static void FatalFormat(this ILogger logger, string format, params object[] args)
        {
            logger.SafeLogIfEnabled(LogLevel.Fatal, () => RenderLogMessage(format, args.Select(x => new Func<object>(() => x))));
        }

        public static void FatalFormat(this ILogger logger, Exception exception, string format, params object[] args)
        {
            logger.SafeLogIfEnabled(LogLevel.Fatal, exception, () => RenderLogMessage(format, args.Select(x => new Func<object>(() => x))));
        }

        public static void FatalFormat(this ILogger logger, Exception exception, string format, params Func<object>[] args)
        {
            logger.SafeLogIfEnabled(LogLevel.Fatal, exception, () => RenderLogMessage(format, args));
        }

        public static void FatalFormat(this ILogger logger, string format, params Func<object>[] args)
        {
            logger.SafeLogIfEnabled(LogLevel.Fatal, () => RenderLogMessage(format, args));
        }
    }
}