using Avalonia.Threading;
using ChatEngine.Logging;
using System;

namespace YChatDesktop.Models.Loggers
{
    internal class UILogger : IChatLogger
    {
        private readonly Dispatcher _uiThread;
        private readonly Action<string?> _appendTextAction;

        public bool MessageLoggingEnabled { get; set; } = true;
        public bool InfoLoggingEnabled { get; set; } = true;
        public bool ErrorLoggingEnabled { get; set; } = true;
        public bool PrintNickBeforeTypingMessage { get; set; } = false;

        public UILogger(Action<string?> appendTextAction, Dispatcher uiThread)
        {
            _uiThread = uiThread;
            _appendTextAction = appendTextAction;
        }

        public void Write(string? text)
        {
            _uiThread.InvokeAsync(() => _appendTextAction(text));
        }

        public void WriteLine(string? text)
        {
            Write(text + Environment.NewLine);
        }

        public void LogMessage(string? message)
        {
            if (MessageLoggingEnabled)
                WriteLine(message);
        }

        public void LogInfo(string? info)
        {
            if (InfoLoggingEnabled)
                WriteLine(info);
        }

        public void LogError(string? error)
        {
            if (ErrorLoggingEnabled)
                WriteLine(error);
        }

        public void WriteNick(string? nick)
        {
            if (PrintNickBeforeTypingMessage)
                Write($"{nick}: ");
        }
    }
}
