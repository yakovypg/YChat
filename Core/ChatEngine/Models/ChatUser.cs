namespace ChatEngine.Models
{
    public class ChatUser : IChatUser, IEquatable<ChatUser?>
    {
        public string Nick { get; set; }

        public ChatUser() : this(GetRandomNick())
        {
        }

        public ChatUser(string nick)
        {
            Nick = nick;
        }

        public void Rename(string? newNick)
        {
            Nick = newNick ?? string.Empty;
        }

        protected static string GetRandomNick()
        {
            const int numLength = 6;
            const string prefix = "User";

            var rand = new Random();
            int randNum = rand.Next();

            string num = randNum.ToString();
            num = num.PadLeft(numLength, '0');

            return $"{prefix}{num}";
        }

        public bool Equals(ChatUser? other)
        {
            return other is not null &&
                   Nick == other.Nick;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ChatUser);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Nick);
        }

        public override string ToString()
        {
            return Nick;
        }

        public static bool operator ==(ChatUser? left, ChatUser? right)
        {
            return EqualityComparer<ChatUser>.Default.Equals(left, right);
        }

        public static bool operator !=(ChatUser? left, ChatUser? right)
        {
            return !(left == right);
        }
    }
}
