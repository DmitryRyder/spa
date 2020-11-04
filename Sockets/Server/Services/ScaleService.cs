using Server.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    internal class ScaleService
    {
        private readonly int _numberOfsockets;
        private readonly string _startStringAdress;
        private readonly int _beginPort;

        public List<CustomSocket> Sockets { get; set; }

        public ScaleService(int numberOfsockets, int beginPort = 8001)
        {
            _numberOfsockets = numberOfsockets;
            _startStringAdress = "127.0.0.";
            _beginPort = beginPort;
        }

        public void CloseConnections()
        {
            Sockets.ForEach(i => i.CloseConnection());
        }

        public void Listen()
        {
            Sockets.ForEach(i => i.Listen());
        }

        public void CreateScale()
        {
            Sockets = new List<CustomSocket>();

            for (var i = 2; i < _numberOfsockets + 2; i++)
            {
                var address = _startStringAdress + i.ToString();
                var port = _beginPort + i;
                Sockets.Add(new CustomSocket
                {
                    IpPoint = new IPEndPoint(IPAddress.Parse(address), port),
                    Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                });
            }
        }
    }
}
