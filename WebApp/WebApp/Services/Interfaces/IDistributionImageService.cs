using Models.CommonDto;
using System.Collections.Generic;
using System.Drawing;
using WebApp.Models;

namespace WebApp.Services
{
    public interface IDistributionImageService
    {
        void SendParallelData(List<CustomSocket> sockets);

        List<ImagePartDto> CreateParallelData();

        void ConcatImage(List<CustomSocket> sockets);

        string SaveResultImage();

        void SetImageAndParts(Bitmap image, int parts);
    }
}
