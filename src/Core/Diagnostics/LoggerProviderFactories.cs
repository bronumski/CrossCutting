namespace CrossCutting.Diagnostics
{
    public static class LoggerProviderFactories
    {
        private static ILoggerProvider loggerProvider = new DefaultLoggerProvider();

        public static ILoggerProvider LoggerProvider { get { return loggerProvider; } }

        public static void SetLoggingProvider(ILoggerProvider newLoggerProvider)
        {
            if (newLoggerProvider != null)
            {
                loggerProvider = newLoggerProvider;
            }
        }
    }
}