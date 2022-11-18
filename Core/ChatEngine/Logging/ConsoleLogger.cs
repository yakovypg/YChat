using System.Runtime.Versioning;

namespace ChatEngine.Logging
{
    public class ConsoleLogger : ChatLogger, IConsoleLogger, IEquatable<ConsoleLogger?>
    {
        public ConsoleLogger() : base(Console.Out)
        {
        }

        public override void LogInfo(string? info)
        {
            ExecuteWithTempConsoleForegroundColor(() => base.LogError(info), ConsoleColor.DarkYellow);
        }

        public override void LogError(string? error)
        {
            ExecuteWithTempConsoleForegroundColor(() => base.LogError(error), ConsoleColor.DarkRed);
        }

        [SupportedOSPlatform("windows")]
        public virtual void PrintMessageBeforeLastLine(string message)
        {
            (int left, int top) = Console.GetCursorPosition();

            Console.MoveBufferArea(0, top, left, 1, 0, top + 1);
            Console.SetCursorPosition(0, top);
            Console.WriteLine(message);
            Console.SetCursorPosition(left, top + 1);
        }

        protected static void ExecuteWithTempConsoleForegroundColor(Action action, ConsoleColor tempColor)
        {
            ConsoleColor sourceColor = Console.ForegroundColor;

            Console.ForegroundColor = tempColor;
            action?.Invoke();
            Console.ForegroundColor = sourceColor;
        }

        public bool Equals(ConsoleLogger? other)
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
            return Equals(obj as ConsoleLogger);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), MessageLoggingEnabled, InfoLoggingEnabled, ErrorLoggingEnabled, PrintNickBeforeTypingMessage);
        }

        public static bool operator ==(ConsoleLogger? left, ConsoleLogger? right)
        {
            return EqualityComparer<ConsoleLogger>.Default.Equals(left, right);
        }

        public static bool operator !=(ConsoleLogger? left, ConsoleLogger? right)
        {
            return !(left == right);
        }
    }
}
