namespace ChatEngine.ClientSide
{
    public class ConsoleInputBoard : IInputBoard
    {
        public ConsoleInputBoard()
        {
        }

        public void Clear()
        {
            Console.Clear();
        }

        public async Task<string?> ReadLineAsync()
        {
            return await Task.Run(Console.ReadLine);
        }
    }
}
