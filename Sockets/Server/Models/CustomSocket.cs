using Server.Models.Dto;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace Server.Models
{
    internal class CustomSocket
    {
        private readonly MockFilter _filter;

        public CustomSocket()
        {
            _filter = new MockFilter();
        }

        public bool isFinished = false;

        private byte[] _buffer = new byte[9999999];

        public ImagePartDto Data { get; set; }

        private Socket _handler;

        public IPEndPoint IpPoint { get; set; }

        public Socket Socket { get; set; }

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

        public void Send()
        {
            _handler.Send(Data.PartOfImage);
        }

        public void RecieveData()
        {
            do
            {
                Console.WriteLine("test");
                _handler.Receive(_buffer, _buffer.Length, 0);

            }
            while (Socket.Available > 0);

            Deserialize();
            isFinished = true;
        }

        public void FilterProcess()
        {
            var result = _filter.Implementation(Data.PartOfImage);
            Data.PartOfImage = result;
        }

        private void Deserialize()
        {
            using MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(_buffer, 0, _buffer.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Data = (ImagePartDto)binForm.Deserialize(memStream);
        }
    }
}
