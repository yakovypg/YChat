using ChatEngine.Logging;
using ChatEngine.Sockets;
using System.Net;

namespace ChatEngine.SeverSide
{
    public class Server : IServer
    {
        private readonly List<IClientConnector> _clients;
        private readonly IServerTcpListener _listener;

        public ILogger? Logger { get; set; }

        public int ActiveConnections => _clients.Count;
        public IReadOnlyList<IClientConnector> Clients => _clients;

        public Server(ILogger? logger = null) : this(IPAddress.Any, SharedConfig.NetConfig.DEFAULT_PORT, logger)
        {
        }

        public Server(IPAddress localAddress, int port, ILogger? logger = null)
        {
            _clients = new List<IClientConnector>();
            _listener = new ServerTcpListener(localAddress, port);

            Logger = logger;
        }

        public async Task StartAsync()
        {
            try
            {
                _listener.Start();
                Logger?.LogInfo("Waiting for connections...");

                while (true)
                {
                    NetTcpClient tcpClient = await _listener.AcceptTcpClientAsync();

                    string id = GetNextClientId();
                    IClientConnector client = new ClientConnector(id, tcpClient, this);

                    _clients.Add(client);

                    _ = Task.Run(client.StartReceiveMessagesAsync);
                }
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }

        public async Task BroadcastMessageAsync(IClientConnector sender, string? message)
        {
            var receivers = _clients.Where(t => t.ClientId != sender.ClientId);

            foreach (var client in receivers)
            {
                await client.WriteMessageAsync(message);
                await client.FlushWriterAsync();
            }
        }

        public bool RemoveConnection(string clientId)
        {
            IClientConnector? client = _clients.FirstOrDefault(t => t.ClientId == clientId);
            return client is not null && RemoveConnection(client);
        }

        public bool RemoveConnection(IClientConnector client)
        {
            if (!_clients.Remove(client))
                return false;

            client.Close();
            return true;
        }

        protected virtual string GetNextClientId()
        {
            string id;

            do
            {
                id = Guid.NewGuid().ToString();
            }
            while (_clients.Any(t => t.ClientId == id));

            return id;
        }

        private void Disconnect()
        {
            foreach (var client in _clients)
            {
                client.Close();
            }

            _clients.Clear();
            _listener.Stop();

            Logger?.LogInfo("Server was stopped.");
        }
    }
}
