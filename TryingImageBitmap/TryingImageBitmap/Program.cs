using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace TryingImageBitmap
{
    class Program
    {
        private static Bitmap _bmp = new Bitmap("d:\\mountains-wallpapers.bmp");

        static void Main()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ProcessImage(999);
            _bmp.Save("d:\\mountains-wallpapers2.bmp", ImageFormat.Jpeg);
            stopWatch.Stop();
            Console.WriteLine("затраченное время: " + stopWatch.ElapsedMilliseconds);
        }

        private static void ProcessImage(int parts)
        {
            Rectangle rect = new Rectangle(0, 0, _bmp.Width, _bmp.Height);
            BitmapData bmpData = _bmp.LockBits(rect, ImageLockMode.ReadWrite, _bmp.PixelFormat);

            FilterProcess(bmpData, parts);
        }

        private static void FilterProcess(BitmapData data, int parts)
        {
            List<Action> actions = new List<Action>();
            var rgbValues = new byte[Math.Abs(data.Stride) * _bmp.Height];
            var lengthOfPart = Math.Abs(data.Stride) * _bmp.Height / parts;
            Marshal.Copy(data.Scan0, rgbValues, 0, Math.Abs(data.Stride) * _bmp.Height);
            
            //Создание параллельных задач
            for (int i = 0; i < parts; i++)
            {
                var indexParam = i;
                actions.Add(() => Filter(rgbValues, indexParam, lengthOfPart));
                //Filter(rgbValues, indexParam, lengthOfPart);
            }

            //Вызов задач параллельно
            Parallel.Invoke(actions.ToArray());
            Marshal.Copy(rgbValues, 0, data.Scan0, Math.Abs(data.Stride) * _bmp.Height);

            _bmp.UnlockBits(data);
        }

        private static void Filter(byte[] rgbValues, int index, int lengthOfPart)
        {
            Console.WriteLine($"Выполняется задача {Task.CurrentId}");
            Console.WriteLine($"длина массива: {rgbValues.Length}");
            Console.WriteLine($"индекс: {index}");
            Console.WriteLine($"длина части: {lengthOfPart}");

            var counterCase = index * lengthOfPart + 2;
            var counterCase2 = (lengthOfPart * index) + lengthOfPart - 1;
            var length = rgbValues.Length;

            for (int counter = index * lengthOfPart + 2; counter < (lengthOfPart * index) + lengthOfPart-1; counter += 3)
                rgbValues[counter] = 255;
        }
    }
}
