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

        public Task<NetTcpClient> AcceptTcpClientAsync();
        public ValueTask<NetTcpClient> AcceptTcpClientAsync(CancellationToken cancellationToken);
    }
}
