using ConsoleClient.Extensions;
using ConsoleClient.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ConsoleClient.Services.Implementations
{
    internal class DistributionImageService
    {
        private readonly List<CustomSocket>  _sockets;
        private readonly Bitmap _image;

        public DistributionImageService(List<CustomSocket> sockets, Bitmap image)
        {
            _sockets = sockets;
            _image = image;
        }

        public void Process()
        {
            Rectangle rect = new Rectangle(0, 0, _image.Width, _image.Height);
            BitmapData bmpData = _image.LockBits(rect, ImageLockMode.ReadWrite, _image.PixelFormat);

            SendParallelData(bmpData, _sockets.Count);
        }

        private void SendParallelData(BitmapData data, int parts)
        {
            //Используем ArrayExtensions.Split() для деления массива на части и отправки на сокеты.

            List<Action> actions = new List<Action>();
            var rgbValues = new byte[Math.Abs(data.Stride) * _image.Height];
            //var lengthOfPart = Math.Abs(data.Stride) * _image.Height / parts;
            Marshal.Copy(data.Scan0, rgbValues, 0, Math.Abs(data.Stride) * _image.Height);

            var partArray = rgbValues.Split(parts).ToArray();

            for(var i = 0; i < parts; i++)
            {
                actions.Add(() =>
                {
                    _sockets[i].Send(partArray[i].ToArray());
                    _sockets[i].RecieveData();
                });
            }

            Parallel.Invoke(actions.ToArray());

            _sockets.ForEach(s =>
            {
                rgbValues = new byte[Math.Abs(data.Stride) * _image.Height];
                rgbValues.Concat(s.Data);
            });
            var result = _sockets.FirstOrDefault().RecieveData();

            ////Создание параллельных задач
            //for (int i = 0; i < parts; i++)
            //{
            //    var indexParam = i;
            //    actions.Add(() => Implementation(rgbValues, indexParam, lengthOfPart));
            //}

            ////Вызов задач параллельно
            //Parallel.Invoke(actions.ToArray());
            Marshal.Copy(result, 0, data.Scan0, Math.Abs(data.Stride) * _image.Height);

            _image.UnlockBits(data);
        }

        private void Implementation(byte[] rgbValues, int index, int lengthOfPart)
        {
            //Console.WriteLine($"Выполняется задача {Task.CurrentId}");
            //Console.WriteLine($"длина массива: {rgbValues.Length}");
            //Console.WriteLine($"индекс: {index}");
            //Console.WriteLine($"длина части: {lengthOfPart}");

            //var counterCase = index * lengthOfPart + 2;
            //var counterCase2 = (lengthOfPart * index) + lengthOfPart - 1;
            //var length = rgbValues.Length;

            for (int counter = index * lengthOfPart; counter < (lengthOfPart * index) + lengthOfPart - 1; counter += 3)
                rgbValues[counter] = 255;
        }
    }
}
