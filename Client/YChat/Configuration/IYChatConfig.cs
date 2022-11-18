using ChatEngine.Configuration;

namespace YChat.Configuration
{
    internal interface IYChatConfig
    {
        bool ShowHelp { get; set; }
        bool ShowVersion { get; set; }

        IClientConfig ClientConfig { get; set; }
    }
}