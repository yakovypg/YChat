using ChatEngine.ClientSide;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace YChatDesktop.Models
{
    internal class TextBoxInputBoard : IInputBoard
    {
        private string? _line;
        private bool _isLineAdded;

        private readonly Action _clearAction;

        public TextBoxInputBoard(Action clearAction)
        {
            _clearAction = clearAction;
        }

        public void Clear()
        {
            _clearAction.Invoke();
        }

        public Task<string?> ReadLineAsync()
        {
            return Task.Run(() =>
            {
                while (!_isLineAdded)
                    Thread.Sleep(100);

                _isLineAdded = false;
                return _line;
            });
        }

        public void AddLine(string line)
        {
            _line = line;
            _isLineAdded = true;
        }
    }
}
