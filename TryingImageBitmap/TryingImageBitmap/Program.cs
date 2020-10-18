using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace TryingImageBitmap
{
    class Program
    {
        private static Bitmap _image = new Bitmap("d:\\nigga.jpg");

        static async Task Main()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var filter = new Filter(3, _image);

            stopWatch.Start();
            filter.Process();
            stopWatch.Stop();
            _image.Save("d:\\nigga1.jpg.", ImageFormat.Jpeg);
            Console.WriteLine("затраченное время на синхронное выполнение: " + stopWatch.ElapsedMilliseconds);

            stopWatch.Reset();

            stopWatch.Start();
            await filter.ProcessAsync();
            stopWatch.Stop();
            _image.Save("d:\\nigga2.jpg", ImageFormat.Jpeg);
            Console.WriteLine("затраченное время на асинхронное выполнение с паралельными задачами: " + stopWatch.ElapsedMilliseconds);

            stopWatch.Reset();

            stopWatch.Start();
            filter.ProcessParallel();
            stopWatch.Stop();
            _image.Save("d:\\nigga3.jpg", ImageFormat.Jpeg);
            Console.WriteLine("затраченное время на параллельное выполнение(parallel): " + stopWatch.ElapsedMilliseconds);
            
            Console.Read();
        }
    }
}
