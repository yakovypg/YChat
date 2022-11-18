using ChatEngine.Models;

namespace ChatEngine.SeverSide
{
    public interface IClientConnector
    {
        string ClientId { get; }
        IChatUser ChatUser { get; }

        Task StartReceiveMessagesAsync();
        Task WriteMessageAsync(string? message);
        Task FlushWriterAsync();

        void Close();
    }
}
