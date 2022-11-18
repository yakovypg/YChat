namespace ChatEngine.Logging
{
    internal interface IConsoleLogger : ILogger
    {
        void PrintMessageBeforeLastLine(string message);
    }
}
