using System.Net.Sockets;

namespace ChatEngine.Sockets
{
    public class NetTcpClient : IDisposable
    {
        private AddressFamily _family;
        private NetworkStream? _dataStream;
        private Socket _clientSocket = null!;

        private bool _isActive;
        private volatile int _isDisposed;

        private bool Disposed => _isDisposed != 0;
        public bool Connected => ClientSocket?.Connected ?? false;

        public Socket ClientSocket
        {
            get => Disposed ? null! : _clientSocket;
            set
            {
                _clientSocket = value;
                _family = _clientSocket?.AddressFamily ?? AddressFamily.Unknown;

                if (_clientSocket is null)
                    InitializeClientSocket();
            }
        }

        public NetTcpClient() : this(AddressFamily.Unknown)
        {
        }

        public NetTcpClient(AddressFamily family)
        {
            if (family is not (AddressFamily.InterNetwork or AddressFamily.InterNetworkV6 or AddressFamily.Unknown))
                throw new ArgumentException("Invalid family.", nameof(family));

            _family = family;

            InitializeClientSocket();
        }

        ~NetTcpClient()
        {
            Dispose(false);
        }

        public void Connect(string hostname, int port)
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(NetTcpClient));

            ArgumentNullException.ThrowIfNull(hostname);

            if (!PortValidator.ValidatePortNumber(port))
                throw new ArgumentOutOfRangeException(nameof(port));

            ClientSocket.Connect(hostname, port);

            _family = ClientSocket.AddressFamily;
            _isActive = true;
        }

        public NetworkStream GetStream()
        {
            return Disposed
                ? throw new ObjectDisposedException(nameof(NetTcpClient))
                : !Connected
                ? throw new InvalidOperationException("Net not connected.")
                : (_dataStream ??= new NetworkStream(ClientSocket, true));
        }

        public void Close()
        {
            Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Interlocked.CompareExchange(ref _isDisposed, 1, 0) != 0)
                return;

            if (!disposing)
                return;

            if (_dataStream is not null)
                _dataStream.Dispose();
            else
                _clientSocket?.Dispose();

            GC.SuppressFinalize(this);
        }

        private void InitializeClientSocket()
        {
            if (_family != AddressFamily.Unknown)
            {
                _clientSocket = new Socket(_family, SocketType.Stream, ProtocolType.Tcp);
            }
            else
            {
                _clientSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

                if (_clientSocket.AddressFamily == AddressFamily.InterNetwork)
                    _family = AddressFamily.InterNetwork;
            }
        }
    }
}
