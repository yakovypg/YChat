namespace ChatEngine.Messaging
{
    public interface IMessageInfo
    {
        bool IsMessage { get; }
        bool IsCommand { get; }

        string? Message { get; }
        MessageCommand? Command { get; }
    }
}