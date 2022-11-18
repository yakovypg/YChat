using System.Net;
using System.Net.Sockets;

namespace ChatEngine.Sockets
{
    internal interface IServerTcpListener
    {
        int Port { get; }
        IPAddress Ip { get; }

        void Start();
        public void Start(int backlog);
        public void Stop();

        public Task<Socket> AcceptSocketAsync();
        public ValueTask<Socket> AcceptSocketAsync(CancellationToken cancellationToken);

        public Task<TcpClient> AcceptTcpClientAsync();
        public ValueTask<TcpClient> AcceptTcpClientAsync(CancellationToken cancellationToken);
    }
}
