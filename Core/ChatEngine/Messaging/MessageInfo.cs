namespace ChatEngine.Messaging
{
    public class MessageInfo : IMessageInfo, IEquatable<MessageInfo?>
    {
        private readonly string? _data;

        public bool IsMessage => Command is null;
        public bool IsCommand => Command is not null;

        public string? Message
        {
            get
            {
                if (string.IsNullOrEmpty(_data))
                    return _data ?? string.Empty;

                if (_data.StartsWith("//"))
                    return _data[1..];

                bool isCommand = _data.StartsWith('/');

                return isCommand
                    ? null
                    : _data;
            }
        }

        public MessageCommand? Command
        {
            get
            {
                if (string.IsNullOrEmpty(_data) || !_data.StartsWith('/') || _data.StartsWith("//"))
                    return null;

                string command = _data.Remove(0, 1).ToLower();

                return command switch
                {
                    "exit" => MessageCommand.Exit,
                    "clear" => MessageCommand.Clear,
                    "list" => MessageCommand.List,

                    _ => MessageCommand.None,
                };
            }
        }

        public MessageInfo(string? data)
        {
            _data = data;
        }

        public bool Equals(MessageInfo? other)
        {
            return other is not null &&
                   _data == other._data;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as MessageInfo);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_data);
        }

        public override string ToString()
        {
            return _data ?? string.Empty;
        }

        public static bool operator ==(MessageInfo? left, MessageInfo? right)
        {
            return EqualityComparer<MessageInfo>.Default.Equals(left, right);
        }

        public static bool operator !=(MessageInfo? left, MessageInfo? right)
        {
            return !(left == right);
        }
    }
}
