using ChatEngine.Logging;

namespace ChatEngine.SeverSide
{
    public interface IServer
    {
        ILogger? Logger { get; set; }

        int ActiveConnections { get; }
        IReadOnlyList<IClientConnector> Clients { get; }

        Task StartAsync();
        Task BroadcastMessageAsync(IClientConnector sender, string? message);

        bool RemoveConnection(string clientId);
        bool RemoveConnection(IClientConnector client);
    }
}
