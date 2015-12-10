namespace CrossCutting.Diagnostics
{
    public interface ILoggerProvider
    {
        ILogger Create(string type);
    }
}