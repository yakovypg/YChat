using ChatEngine.ClientSide;
using ChatEngine.Logging;
using System.Reflection;
using System.Text;
using YChat.Configuration;
using YChat.Parsing;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

var argsParser = new ArgsParser();

if (!argsParser.TryParse(args, out IYChatConfig config, out Exception? ex))
{
    Console.WriteLine(ex?.Message);
}
else if (config.ShowHelp)
{
    argsParser.OptionDescriptionsWriter.Invoke(Console.Out);
}
else if (config.ShowVersion)
{
    AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
    string? name = assemblyName.Name?.ToString();
    string? version = assemblyName.Version?.ToString();
    Console.WriteLine($"{name} {version}");
}
else
{
    var logger = new ConsoleLogger()
    {
        PrintNickBeforeTypingMessage = OperatingSystem.IsWindows()
    };

    var inputBoard = new ConsoleInputBoard();
    var client = new Client(config.ClientConfig, inputBoard, logger);

    await client.ConnentToServerAsync();
}
