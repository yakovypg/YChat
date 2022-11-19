using System.Net;
using System.Net.Sockets;

namespace ChatEngine.Sockets
{
    internal class ServerTcpListener : IServerTcpListener, IDisposable
    {
        private readonly IPEndPoint _serverSocketEndPoint;
        private Socket? _serverSocket;
        private bool _isActive;

        public int Port { get; }
        public IPAddress Ip { get; }

        public ServerTcpListener(IPAddress localAddress, int port)
        {
            if (!PortValidator.ValidatePortNumber(port))
                throw new ArgumentOutOfRangeException(nameof(port));

            _serverSocketEndPoint = new IPEndPoint(localAddress, port);
            _serverSocket = new Socket(_serverSocketEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            Ip = localAddress;
            Port = port;
        }

        public void Start()
        {
            Start((int)SocketOptionName.MaxConnections);
        }

        public void Start(int backlog)
        {
            if (backlog < 0)
                throw new ArgumentOutOfRangeException(nameof(backlog));

            if (_isActive)
                return;

            CreateNewSocketIfNeeded();
            _serverSocket!.Bind(_serverSocketEndPoint);

            try
            {
                _serverSocket.Listen(backlog);
            }
            catch (SocketException)
            {
                Stop();
                throw;
            }

            _isActive = true;
        }

        public void Stop()
        {
            _serverSocket?.Dispose();

            _isActive = false;
            _serverSocket = null;
        }

        public void Dispose()
        {
            Stop();
        }

        public Task<Socket> AcceptSocketAsync()
        {
            return AcceptSocketAsync(CancellationToken.None).AsTask();
        }

        public ValueTask<Socket> AcceptSocketAsync(CancellationToken cancellationToken)
        {
            return _isActive
                ? _serverSocket!.AcceptAsync(cancellationToken)
                : throw new InvalidOperationException("Net stopped.");
        }

        public Task<NetTcpClient> AcceptTcpClientAsync()
        {
            return AcceptTcpClientAsync(CancellationToken.None).AsTask();
        }

        public ValueTask<NetTcpClient> AcceptTcpClientAsync(CancellationToken cancellationToken)
        {
            return WaitAndWrap(AcceptSocketAsync(cancellationToken));
        }

        private void CreateNewSocketIfNeeded()
        {
            _serverSocket ??= new Socket(_serverSocketEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        private static async ValueTask<NetTcpClient> WaitAndWrap(ValueTask<Socket> task)
        {
            Socket socket = await task.ConfigureAwait(false);
            NetTcpClient client = new() { ClientSocket = socket };

            return client;
        }
    }
}
