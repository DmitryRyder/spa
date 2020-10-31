using System;
using System.Net;
using System.Net.Sockets;

namespace Server.Models
{
    internal class CustomSocket
    {
        public bool isFinished = false;

        private byte[] _buffer = new byte[200688];

        private Socket _handler;

        public IPEndPoint IpPoint { get; set; }

        public Socket Socket { get; set; }

        public byte[] Data { get => _buffer; }

        public void Listen()
        {
            Socket.Bind(IpPoint);
            Socket.Listen(10);
            _handler = Socket.Accept();
        }

        public void CloseConnection()
        {
            Socket.Shutdown(SocketShutdown.Both);
            Socket.Close();
        }

        public void Send(byte[] data)
        {
            _handler.Send(data);
        }

        public void RecieveData()
        {
            do
            {
                Console.WriteLine("test");
                _handler.Receive(_buffer, _buffer.Length, 0);
            }
            while (Socket.Available > 0);

            isFinished = true;
        }
    }
}
