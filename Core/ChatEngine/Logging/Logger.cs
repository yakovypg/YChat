namespace ChatEngine.Logging
{
    public class Logger : ILogger, IEquatable<Logger?>
    {
        private readonly TextWriter _out;

        public bool MessageLoggingEnabled { get; set; } = true;
        public bool InfoLoggingEnabled { get; set; } = true;
        public bool ErrorLoggingEnabled { get; set; } = true;

        public Logger(TextWriter writer)
        {
            _out = writer;
        }

        public void Write(string? text)
        {
            _out.Write(text);
        }

        public void WriteLine(string? text)
        {
            _out.WriteLine(text);
        }

        public virtual void LogMessage(string? message)
        {
            if (MessageLoggingEnabled)
                WriteLine(message);
        }

        public virtual void LogInfo(string? info)
        {
            if (InfoLoggingEnabled)
                WriteLine(info);
        }

        public virtual void LogError(string? error)
        {
            if (ErrorLoggingEnabled)
                WriteLine(error);
        }

        public bool Equals(Logger? other)
        {
            return other is not null &&
                   EqualityComparer<TextWriter>.Default.Equals(_out, other._out) &&
                   MessageLoggingEnabled == other.MessageLoggingEnabled &&
                   InfoLoggingEnabled == other.InfoLoggingEnabled &&
                   ErrorLoggingEnabled == other.ErrorLoggingEnabled;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Logger);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_out, MessageLoggingEnabled, InfoLoggingEnabled, ErrorLoggingEnabled);
        }

        public static bool operator ==(Logger? left, Logger? right)
        {
            return EqualityComparer<Logger>.Default.Equals(left, right);
        }

        public static bool operator !=(Logger? left, Logger? right)
        {
            return !(left == right);
        }
    }
}
