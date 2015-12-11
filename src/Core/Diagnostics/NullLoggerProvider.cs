namespace CrossCutting.Diagnostics
{
    public class NullLoggerProvider : ILoggerProvider
    {
        public ILogger Create(string type)
        {
            return new NullLogger();
        }
    }
}