using ConsoleClient.Models.Dto;
using System;
using System.Net;
using System.Net.Sockets;

namespace ConsoleClient.Models
{
    internal class CustomSocket
    {
        public IPEndPoint IpPoint { get; set; }

        public Socket Socket { get; set; }

        public ImagePartDto Data { get; set; }

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

        public void RecieveData()
        {
            do
            {
                Socket.Receive(Data.PartOfImage, Data.BufferSize, 0);
            }
            while (Socket.Available > 0);
        }
    }
}
