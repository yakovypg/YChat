namespace ChatEngine.Logging
{
    public class ChatLogger : Logger, IChatLogger, IEquatable<ChatLogger?>
    {
        public bool PrintNickBeforeTypingMessage { get; set; } = true;

        public ChatLogger(TextWriter writer) : base(writer)
        {
        }

        public void WriteNick(string? nick)
        {
            if (PrintNickBeforeTypingMessage)
                Write($"{nick}: ");
        }

        public bool Equals(ChatLogger? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   MessageLoggingEnabled == other.MessageLoggingEnabled &&
                   InfoLoggingEnabled == other.InfoLoggingEnabled &&
                   ErrorLoggingEnabled == other.ErrorLoggingEnabled &&
                   PrintNickBeforeTypingMessage == other.PrintNickBeforeTypingMessage;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ChatLogger);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), MessageLoggingEnabled, InfoLoggingEnabled,
                ErrorLoggingEnabled, PrintNickBeforeTypingMessage);
        }

        public static bool operator ==(ChatLogger? left, ChatLogger? right)
        {
            return EqualityComparer<ChatLogger>.Default.Equals(left, right);
        }

        public static bool operator !=(ChatLogger? left, ChatLogger? right)
        {
            return !(left == right);
        }
    }
}
