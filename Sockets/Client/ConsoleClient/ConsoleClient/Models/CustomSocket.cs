using System.Net;
using System.Net.Sockets;

namespace ConsoleClient.Models
{
    internal class CustomSocket
    {
        public IPEndPoint IpPoint { get; set; }

        public Socket Socket { get; set; }

        public void Connect()
        {
            Socket.Connect(IpPoint);
        }

        public void CloseConnection()
        {
            Socket.Close();
        }
    }
}
