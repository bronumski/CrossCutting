using log4net;

namespace CrossCutting.Diagnostics
{
    public class Log4NetLoggerProvider : ILoggerProvider
    {
        public ILogger Create(string type)
        {
            return new Log4NetLogger(LogManager.GetLogger(type));
        }
    }
}