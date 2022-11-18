namespace ChatEngine.Configuration
{
    public class NetConfig : INetConfig, IEquatable<NetConfig?>
    {
        public int Port { get; set; }
        public string Host { get; set; }

        public NetConfig(int port, string host)
        {
            Port = port;
            Host = host;
        }

        public bool Equals(NetConfig? other)
        {
            return other is not null &&
                   Port == other.Port &&
                   Host == other.Host;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as NetConfig);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Port, Host);
        }

        public static bool operator ==(NetConfig? left, NetConfig? right)
        {
            return EqualityComparer<NetConfig>.Default.Equals(left, right);
        }

        public static bool operator !=(NetConfig? left, NetConfig? right)
        {
            return !(left == right);
        }
    }
}
