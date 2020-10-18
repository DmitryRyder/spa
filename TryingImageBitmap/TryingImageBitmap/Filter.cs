using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace TryingImageBitmap
{
    internal class Filter
    {
        private int _parts { get; set; }

        private Bitmap _image { get; set; }

        public Filter(int parts, Bitmap image)
        {
            _parts = parts;
            _image = image;
        }

        public void Process()
        {
            Rectangle rect = new Rectangle(0, 0, _image.Width, _image.Height);
            BitmapData bmpData = _image.LockBits(rect, ImageLockMode.ReadWrite, _image.PixelFormat);

            FilterProcess(bmpData, _parts);
        }

        public async Task ProcessAsync()
        {
            Rectangle rect = new Rectangle(0, 0, _image.Width, _image.Height);
            BitmapData bmpData = _image.LockBits(rect, ImageLockMode.ReadWrite, _image.PixelFormat);

            await FilterProcessAsync(bmpData, _parts);
        }

        public void ProcessParallel()
        {
            Rectangle rect = new Rectangle(0, 0, _image.Width, _image.Height);
            BitmapData bmpData = _image.LockBits(rect, ImageLockMode.ReadWrite, _image.PixelFormat);

            FilterProcessParallel(bmpData, _parts);
        }

        private void FilterProcess(BitmapData data, int parts)
        {
            var rgbValues = new byte[Math.Abs(data.Stride) * _image.Height];
            var lengthOfPart = Math.Abs(data.Stride) * _image.Height / parts;
            Marshal.Copy(data.Scan0, rgbValues, 0, Math.Abs(data.Stride) * _image.Height);
            var testArray = rgbValues.Split(parts).ToArray();
            //Создание параллельных задач
            for (int i = 0; i < parts; i++)
            {
                var indexParam = i;
                Implementation(rgbValues, indexParam, lengthOfPart);
            }

            Marshal.Copy(rgbValues, 0, data.Scan0, Math.Abs(data.Stride) * _image.Height);

            _image.UnlockBits(data);
        }

        private void FilterProcessParallel(BitmapData data, int parts)
        {
            List<Action> actions = new List<Action>();
            var rgbValues = new byte[Math.Abs(data.Stride) * _image.Height];
            var lengthOfPart = Math.Abs(data.Stride) * _image.Height / parts;
            Marshal.Copy(data.Scan0, rgbValues, 0, Math.Abs(data.Stride) * _image.Height);

            //Создание параллельных задач
            for (int i = 0; i < parts; i++)
            {
                var indexParam = i;
                actions.Add(() => Implementation(rgbValues, indexParam, lengthOfPart));
            }

            //Вызов задач параллельно
            Parallel.Invoke(actions.ToArray());
            Marshal.Copy(rgbValues, 0, data.Scan0, Math.Abs(data.Stride) * _image.Height);

            _image.UnlockBits(data);
        }

        private async Task FilterProcessAsync(BitmapData data, int parts)
        {
            List<Task> tasks = new List<Task>();
            var rgbValues = new byte[Math.Abs(data.Stride) * _image.Height];
            var lengthOfPart = Math.Abs(data.Stride) * _image.Height / parts;
            Marshal.Copy(data.Scan0, rgbValues, 0, Math.Abs(data.Stride) * _image.Height);

            //Создание параллельных задач
            for (int i = 0; i < parts; i++)
            {
                var indexParam = i;
                Task t1 = Task.Run(() => Implementation(rgbValues, indexParam, lengthOfPart));
                tasks.Add(t1);
            }

            await Task.WhenAll(tasks);
            Marshal.Copy(rgbValues, 0, data.Scan0, Math.Abs(data.Stride) * _image.Height);

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
