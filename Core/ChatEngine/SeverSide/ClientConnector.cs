using ChatEngine.Messaging;
using ChatEngine.Models;
using ChatEngine.Sockets;
using System.Text.Json;

namespace ChatEngine.SeverSide
{
    public class ClientConnector : IClientConnector
    {
        private readonly StreamWriter _writer;
        private readonly StreamReader _reader;

        private readonly IServer _server;
        private readonly NetTcpClient _tcpClient;

        public string ClientId { get; }
        public IChatUser ChatUser { get; private set; }

        public ClientConnector(string clientId, NetTcpClient tcpClient, IServer server)
        {
            _writer = new StreamWriter(tcpClient.GetStream());
            _reader = new StreamReader(tcpClient.GetStream());

            _server = server;
            _tcpClient = tcpClient;

            ClientId = clientId;
            ChatUser = new ChatUser();
        }

        public async Task StartReceiveMessagesAsync()
        {
            bool isInfoReceived = await TryReadChatUserInfoAsync();

            if (!isInfoReceived)
            {
                _server.RemoveConnection(this);
                return;
            }

            while (true)
            {
                try
                {
                    string? messageData = await _reader.ReadLineAsync();
                    var messageInfo = new MessageInfo(messageData);

                    if (messageInfo.Command is not null)
                    {
                        ActionAfterExecutingMessageCommand action = await ExecuteCommandAsync(messageInfo.Command.Value);

                        if (action == ActionAfterExecutingMessageCommand.CloseConnection)
                            break;

                        continue;
                    }

                    string message = $"{ChatUser.Nick}: {messageInfo.Message}";

                    _server.Logger?.LogMessage(message);
                    await _server.BroadcastMessageAsync(this, message);
                }
                catch { break; }
            }

            string userLeftMessage = $"{ChatUser.Nick} has left the chat.";

            _server.Logger?.LogInfo(userLeftMessage);
            await _server.BroadcastMessageAsync(this, userLeftMessage);

            _server.RemoveConnection(this);
        }

        public Task WriteMessageAsync(string? message)
        {
            return _writer.WriteLineAsync(message);
        }

        public Task FlushWriterAsync()
        {
            return _writer.FlushAsync();
        }

        public void Close()
        {
            _writer.Close();
            _reader.Close();
            _tcpClient.Close();
        }

        private async Task<bool> TryReadChatUserInfoAsync()
        {
            try
            {
                string? chatUserJson = await _reader.ReadLineAsync();

                ChatUser = JsonSerializer.Deserialize<ChatUser>(chatUserJson ?? string.Empty)
                    ?? throw new NullReferenceException("Deserialized chat user is null.");

                string? userJoinedMessage = $"{ChatUser.Nick} has joined the chat.";

                _server.Logger?.LogInfo(userJoinedMessage);
                await _server.BroadcastMessageAsync(this, userJoinedMessage);

                return true;
            }
            catch (Exception ex)
            {
                _server.Logger?.LogError(ex.Message);
                return false;
            }
        }

        private async Task<ActionAfterExecutingMessageCommand> ExecuteCommandAsync(MessageCommand command)
        {
            string answer;
            ActionAfterExecutingMessageCommand actionAfterExecuting;

            switch (command)
            {
                case MessageCommand.Exit:
                    _server.RemoveConnection(this);

                    answer = $"/{command}";
                    actionAfterExecuting = ActionAfterExecutingMessageCommand.CloseConnection;
                    break;

                case MessageCommand.Clear:
                    answer = $"/{command}";
                    actionAfterExecuting = ActionAfterExecutingMessageCommand.Continue;
                    break;

                case MessageCommand.List:
                    var users = _server.Clients.Select(t => t.ChatUser.ToString());
                    string usersStr = string.Join('\n', users);
                    string participants = $"\nParticipants:\n{usersStr}\n";

                    answer = participants;
                    actionAfterExecuting = ActionAfterExecutingMessageCommand.Continue;
                    break;

                default:
                    answer = "The command is unknown.";
                    actionAfterExecuting = ActionAfterExecutingMessageCommand.Continue;
                    break;
            }

            await WriteMessageAsync(answer);
            await FlushWriterAsync();

            return actionAfterExecuting;
        }
    }
}
