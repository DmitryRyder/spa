using ConsoleClient.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace ConsoleClient.Implementations
{
    internal class ScaleService
    {
        private readonly int _numberOfsockets;
        private readonly string _beginAdress;
        private readonly int _beginPort;
        public List<CustomSocket> Sockets { get; set; }

        public ScaleService(int numberOfsockets, string beginAdress = "127.0.0.1", int beginPort = 8001)
        {
            _numberOfsockets = numberOfsockets;
            _beginAdress = beginAdress;
            _beginPort = beginPort;
        }

        public void CloseConnections()
        {
            Sockets.ForEach(i => i.CloseConnection());
        }

        public void Connect()
        {
            Sockets.ForEach(i => i.Connect());
        }

        public void CreateScale()
        {
            Sockets = new List<CustomSocket>();

            for (var i = 0; i < _numberOfsockets; i++)
            {
                var ip = _beginAdress.Replace(_beginAdress.Split('.').LastOrDefault(), i.ToString());
                var port = _beginPort + i;
                Sockets.Add(new CustomSocket 
                { 
                    IpPoint = new IPEndPoint(IPAddress.Parse(ip), port),
                    Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                });
            }
        }
    }
}
