using ChatEngine.Logging;
using ChatEngine.SeverSide;
using System.Net;
using System.Text;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

int port = SharedConfig.NetConfig.DEFAULT_PORT;

if (args.Length == 1)
{
    if (!int.TryParse(Console.ReadLine(), out port))
    {
        Console.WriteLine("Error: incorrect port.");
        return;
    }
}
else if (args.Length > 1)
{
    Console.WriteLine("Error: incorrect number of arguments.");
    return;
}

var logger = new ConsoleLogger();
var server = new Server(IPAddress.Any, port, logger);

await server.StartAsync();
