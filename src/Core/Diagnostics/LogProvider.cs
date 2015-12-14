using System;

namespace CrossCutting.Diagnostics
{
    public static class LogProvider
    {
        private static ILoggerProvider loggerProvider;

        public static ILoggerProvider LoggerProvider { get { return loggerProvider; } }

        static LogProvider()
        {
            Reset();
        }

        public static void Reset()
        {
            loggerProvider = new NullLoggerProvider();
        }

        public static void SetLoggingProvider<TLoggerProvider>() where TLoggerProvider : ILoggerProvider, new()
        {
            SetLoggingProvider(new TLoggerProvider());
        }

        public static void SetLoggingProvider(ILoggerProvider newLoggerProvider)
        {
            if (newLoggerProvider != null)
            {
                loggerProvider = newLoggerProvider;
            }
        }

        public static ILogger LoggerFor<T>()
        {
            return LoggerFor(typeof(T));
        }

        public static ILogger LoggerFor(Type type)
        {
            return LoggerFor(type.ToString());
        }

        public static ILogger LoggerFor(string type)
        {
            return loggerProvider.Create(type);
        }
    }
}