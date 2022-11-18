namespace ChatEngine.Configuration
{
    public interface INetConfig
    {
        int Port { get; set; }
        string Host { get; set; }
    }
}