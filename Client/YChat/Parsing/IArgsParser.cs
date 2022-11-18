using YChat.Configuration;

namespace YChat.Parsing
{
    internal interface IArgsParser
    {
        Action<TextWriter> OptionDescriptionsWriter { get; }

        bool TryParse(string[] args, out IYChatConfig config);
        bool TryParse(string[] args, out IYChatConfig config, out Exception? exception);
    }
}