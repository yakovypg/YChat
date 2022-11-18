using Mono.Options;
using System.Text;
using YChat.Configuration;

namespace YChat.Parsing
{
    internal class ArgsParser : IArgsParser
    {
        private IYChatConfig? _config;
        private readonly OptionSet _options;

        public Action<TextWriter> OptionDescriptionsWriter => _options.WriteOptionDescriptions;

        public ArgsParser()
        {
            _options = new OptionSet()
            {
                { "h|help", "show help.", t => _config!.ShowHelp = t is not null },
                { "v|version", "show version.", t => _config!.ShowVersion = t is not null },

                { "n|nick=", "the nick.", t => _config!.ClientConfig.ChatUser.Nick = t },

                { "P|port=", "the port.", (int t) => _config!.ClientConfig.NetConfig.Port = t },
                { "H|host=", "the host.", t => _config!.ClientConfig.NetConfig.Host = t },

                { "input-encoding=", "the encoding of console input.", t => Console.InputEncoding = Encoding.GetEncoding(t) },
                { "output-encoding=", "the encoding of console output.", t => Console.OutputEncoding = Encoding.GetEncoding(t) },
            };
        }

        public bool TryParse(string[] args, out IYChatConfig config)
        {
            return TryParse(args, out config, out _);
        }

        public bool TryParse(string[] args, out IYChatConfig config, out Exception? exception)
        {
            _config = new YChatConfig();

            try
            {
                List<string> extraArgs = _options.Parse(args);

                if (extraArgs.Count > 0)
                    throw new UnknownParametersException(extraArgs.ToArray());
            }
            catch (Exception ex)
            {
                exception = ex;
                config = _config;
                return false;
            }

            exception = null;
            config = _config;
            return true;
        }
    }
}
