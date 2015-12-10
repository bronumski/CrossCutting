namespace CrossCutting.Diagnostics
{
    public class DefaultLoggerProvider : ILoggerProvider
    {
        public ILogger Create(string type)
        {
            return new DefaultLogger();
        }
    }
}