using Models.CommonDto;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using WebApp.Models;

namespace WebApp.Services
{
    public class ScaleService : IScaleService
    {
        int _numberOfsockets;
        string _startStringAdress;
        int _beginPort;
        public List<CustomSocket> Sockets { get; set; }

        public ScaleService()
        {
        }

        public void SetnumberOfSocketsAndBeginPort(int numberOfsockets, int beginPort = 8001)
        {
            _numberOfsockets = numberOfsockets;
            _startStringAdress = "127.0.0.";
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

        public void CreateScale(List<ImagePartDto> chunks)
        {
            Sockets = new List<CustomSocket>();

            for (var i = 2; i < _numberOfsockets + 2; i++)
            {
                var address = _startStringAdress + i.ToString();
                var port = _beginPort + i;
                Sockets.Add(new CustomSocket
                {
                    IpPoint = new IPEndPoint(IPAddress.Parse(address), port),
                    Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp),
                    Data = chunks[i - 2]
                });
            }
        }
    }
}
