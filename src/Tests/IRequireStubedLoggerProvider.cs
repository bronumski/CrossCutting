using CrossCutting.Diagnostics;

namespace CrossCutting
{
    [RequireStubbedLoggerProvider]
    interface IRequireStubedLoggerProvider
    {
        ILoggerProvider LoggerProvider { set; }
    }
}