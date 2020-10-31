using System.Net;
using System.Net.Sockets;

namespace ConsoleClient.Models
{
    internal class CustomSocket
    {
        private byte[] _buffer;

        public IPEndPoint IpPoint { get; set; }

        public Socket Socket { get; set; }

        public byte[] Data => _buffer;

        public CustomSocket(int bufferSize)
        {
            _buffer = new byte[bufferSize];
        }

        public void Connect()
        {
            Socket.Connect(IpPoint);
        }

        public void CloseConnection()
        {
            Socket.Shutdown(SocketShutdown.Both);
            Socket.Close();
        }

        public void Send(byte[] data)
        {
            Socket.Send(data);
        }

        public byte[] RecieveData()
        {
            do
            {
                Socket.Receive(_buffer, _buffer.Length, 0);
            }
            while (Socket.Available > 0);

            return _buffer;
        }
    }
}
