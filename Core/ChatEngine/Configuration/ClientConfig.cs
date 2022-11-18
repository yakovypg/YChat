using ChatEngine.Models;

namespace ChatEngine.Configuration
{
    public class ClientConfig : IClientConfig, IEquatable<ClientConfig?>
    {
        public INetConfig NetConfig { get; }
        public IChatUser ChatUser { get; }

        public ClientConfig() : this(new ChatUser(),
            new NetConfig(SharedConfig.NetConfig.DEFAULT_PORT, SharedConfig.NetConfig.DEFAULT_HOST))
        {
        }

        public ClientConfig(IChatUser chatUser, INetConfig netConfig)
        {
            ChatUser = chatUser;
            NetConfig = netConfig;
        }

        public bool Equals(ClientConfig? other)
        {
            return other is not null &&
                   EqualityComparer<INetConfig>.Default.Equals(NetConfig, other.NetConfig) &&
                   EqualityComparer<IChatUser>.Default.Equals(ChatUser, other.ChatUser);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ClientConfig);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NetConfig, ChatUser);
        }

        public static bool operator ==(ClientConfig? left, ClientConfig? right)
        {
            return EqualityComparer<ClientConfig>.Default.Equals(left, right);
        }

        public static bool operator !=(ClientConfig? left, ClientConfig? right)
        {
            return !(left == right);
        }
    }
}
