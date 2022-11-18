namespace ChatEngine.Logging
{
    public interface IChatLogger : ILogger
    {
        bool PrintNickBeforeTypingMessage { get; set; }
        void WriteNick(string? nick);
    }
}
