using System.Net;

namespace ChatEngine.Sockets
{
    internal static class PortValidator
    {
        public static bool ValidatePortNumber(int port)
        {
            return port >= IPEndPoint.MinPort && port <= IPEndPoint.MaxPort;
        }
    }
}
