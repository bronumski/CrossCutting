using System;

namespace CrossCutting.Diagnostics
{
    public class ConsoleLogger : ILogger
    {
        public void Log(LogLevel level, string message)
        {
            Console.WriteLine("{0} - {1}", level, message);
        }

        public void Log(LogLevel level, Exception exception, string message)
        {
            Console.WriteLine("{0} - {1}\n{2}", level, message, exception);
        }

        public bool LevelEnabled(LogLevel level)
        {
            return true;
        }
    }
}