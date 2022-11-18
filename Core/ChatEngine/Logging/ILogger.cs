namespace ChatEngine.Logging
{
    public interface ILogger
    {
        bool MessageLoggingEnabled { get; set; }
        bool InfoLoggingEnabled { get; set; }
        bool ErrorLoggingEnabled { get; set; }

        void Write(string? text);
        void WriteLine(string? text);

        void LogMessage(string? message);
        void LogInfo(string? info);
        void LogError(string? error);
    }
}
