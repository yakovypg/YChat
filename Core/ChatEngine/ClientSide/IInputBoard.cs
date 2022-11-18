namespace ChatEngine.ClientSide
{
    public interface IInputBoard
    {
        void Clear();
        Task<string?> ReadLineAsync();
    }
}
