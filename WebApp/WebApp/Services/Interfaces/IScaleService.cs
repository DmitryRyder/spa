using Models.CommonDto;
using System.Collections.Generic;
using WebApp.Models;

namespace WebApp.Services
{
    public interface IScaleService
    {
        List<CustomSocket> Sockets { get; set; }

        void CloseConnections();

        void Connect();

        void CreateScale(List<ImagePartDto> chunks);

        void SetnumberOfSocketsAndBeginPort(int numberOfsockets, int beginPort = 8001);
    }
}
