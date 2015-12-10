namespace CrossCutting.Diagnostics
{
    public class ConsoleLoggerProvider : ILoggerProvider
    {
        public ILogger Create(string type)
        {
            return new ConsoleLogger();
        }
    }
}