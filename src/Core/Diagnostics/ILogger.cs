using System;

namespace CrossCutting.Diagnostics
{
    public interface ILogger
    {
        void Log(LogLevel level, string message);
        void Log(LogLevel level, Exception exception, string message);
        bool LevelEnabled(LogLevel level);
    }
}