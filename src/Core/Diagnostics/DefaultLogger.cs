using System;

namespace CrossCutting.Diagnostics
{
    public class DefaultLogger : ILogger
    {
        public void Log(LogLevel level, string message) { }
        public void Log(LogLevel level, Exception exception, string message) { }
        public bool LevelEnabled(LogLevel level) { return false; }
    }
}