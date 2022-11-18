using ChatEngine.Configuration;

namespace YChat.Configuration
{
    internal class YChatConfig : IYChatConfig, IEquatable<YChatConfig?>
    {
        public bool ShowHelp { get; set; }
        public bool ShowVersion { get; set; }

        public IClientConfig ClientConfig { get; set; }

        public YChatConfig()
        {
            ClientConfig = new ClientConfig();
        }

        public bool Equals(YChatConfig? other)
        {
            return other is not null &&
                   ShowHelp == other.ShowHelp &&
                   ShowVersion == other.ShowVersion &&
                   EqualityComparer<IClientConfig>.Default.Equals(ClientConfig, other.ClientConfig);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as YChatConfig);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ShowHelp, ShowVersion, ClientConfig);
        }

        public static bool operator ==(YChatConfig? left, YChatConfig? right)
        {
            return EqualityComparer<YChatConfig>.Default.Equals(left, right);
        }

        public static bool operator !=(YChatConfig? left, YChatConfig? right)
        {
            return !(left == right);
        }
    }
}
