namespace ChatEngine.Models
{
    public interface IChatUser
    {
        string Nick { get; set; }
        void Rename(string? newNick);
    }
}
