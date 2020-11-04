using System;
using ConsoleClient.Implementations;
using ConsoleClient.Services.Implementations;
using System.Drawing;

namespace ConsoleClient
{
    class Program
    {
        private static Bitmap _image = new Bitmap("d:\\nigga.jpg");

        static void Main()
        {
            Console.WriteLine("Укажите кол-во серверов");
            
            var countOfServers = Convert.ToInt32(Console.ReadLine());
            var imageService = new DistributionImageService(_image, countOfServers);
            var scaleService = new ScaleService(countOfServers);

            var chunksOfImage = imageService.CreateParallelData();
            scaleService.CreateScale(chunksOfImage);
            scaleService.Connect();
            imageService.SendParallelData(scaleService.Sockets);
            imageService.ConcatImage(scaleService.Sockets);
            imageService.SaveResultImage();
            scaleService.CloseConnections();
        }
    }
}