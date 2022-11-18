using ChatEngine.Models;

namespace ChatEngine.Configuration
{
    public interface IClientConfig
    {
        INetConfig NetConfig { get; }
        IChatUser ChatUser { get; }
    }
}