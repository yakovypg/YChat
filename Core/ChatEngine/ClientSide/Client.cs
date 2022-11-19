using ChatEngine.Configuration;
using ChatEngine.Logging;
using ChatEngine.Messaging;
using ChatEngine.Sockets;
using System.Net.Sockets;
using System.Text.Json;

namespace ChatEngine.ClientSide
{
    public class Client
    {
        private readonly IClientConfig _config;
        private readonly IInputBoard _inputBoard;

        public delegate void ConnectionErrorHandler(Exception ex);
        public event ConnectionErrorHandler? ConnectionError;

        public delegate void EndOfMessageReceivingHandler();
        public event EndOfMessageReceivingHandler? EndOfMessageReceiving;

        public IChatLogger? Logger { get; set; }

        public Client(IClientConfig config, IInputBoard inputBoard, IChatLogger? logger = null)
        {
            _config = config;
            _inputBoard = inputBoard;

            Logger = logger;
        }

        public async Task ConnentToServerAsync()
        {
            using var client = new NetTcpClient(AddressFamily.InterNetwork);

            StreamReader? reader = null;
            StreamWriter? writer = null;

            int port = _config.NetConfig.Port;
            string host = _config.NetConfig.Host;
            string nick = _config.ChatUser.Nick;

            try
            {
                client.Connect(host, port);

                reader = new StreamReader(client.GetStream());
                writer = new StreamWriter(client.GetStream());

                if (writer is null || reader is null)
                    return;

                Logger?.LogInfo($"Welcome, {nick}!");

                _ = Task.Run(() => ReceiveMessageAsync(reader));
                await SendMessageAsync(writer);
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex.Message);
                ConnectionError?.Invoke(ex);
            }
            finally
            {
                writer?.Close();
                reader?.Close();
            }
        }

        protected async Task SendMessageAsync(StreamWriter writer)
        {
            string userInfo = JsonSerializer.Serialize(_config.ChatUser);

            await writer.WriteLineAsync(userInfo);
            await writer.FlushAsync();

            while (true)
            {
                Logger?.WriteNick(_config.ChatUser.Nick);

                string? message = await _inputBoard.ReadLineAsync();

                await writer.WriteLineAsync(message);
                await writer.FlushAsync();

                if (new MessageInfo(message).Command == MessageCommand.Exit)
                    break;
            }
        }

        protected async Task ReceiveMessageAsync(StreamReader reader)
        {
            while (true)
            {
                try
                {
                    string? messageData = await reader.ReadLineAsync();
                    var messageInfo = new MessageInfo(messageData);

                    if (messageInfo.Command is not null)
                    {
                        ActionAfterExecutingMessageCommand action = ExecuteCommand(messageInfo.Command.Value);

                        if (action == ActionAfterExecutingMessageCommand.CloseConnection)
                            break;
                    }
                    else
                    {
                        PrintMessage(messageInfo.Message ?? string.Empty);
                    }
                }
                catch { break; }
            }

            EndOfMessageReceiving?.Invoke();
        }

        protected virtual void PrintMessage(string message)
        {
            if (Logger is ConsoleLogger consoleLogger && OperatingSystem.IsWindows())
                consoleLogger.PrintMessageBeforeLastLine(message);
            else
                Logger?.LogMessage(message);
        }

        private ActionAfterExecutingMessageCommand ExecuteCommand(MessageCommand command)
        {
            switch (command)
            {
                case MessageCommand.Exit:
                    return ActionAfterExecutingMessageCommand.CloseConnection;

                case MessageCommand.Clear:
                    _inputBoard.Clear();
                    Logger?.WriteNick(_config.ChatUser.Nick);
                    return ActionAfterExecutingMessageCommand.Continue;

                default:
                    return ActionAfterExecutingMessageCommand.Continue;
            }
        }
    }
}
