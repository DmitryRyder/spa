using ConsoleClient.Extensions;
using ConsoleClient.Models;
using ConsoleClient.Models.Dto;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace ConsoleClient.Services.Implementations
{
    internal class DistributionImageService
    {
        private readonly Bitmap _image;
        private readonly Rectangle _rect;
        private readonly BitmapData _bmpData;
        private byte[] _rgbValues;
        private readonly int _parts;

        public DistributionImageService(Bitmap image, int parts)
        {
            _image = image;
            _parts = parts;
            _rect = new Rectangle(0, 0, _image.Width, _image.Height);
            _bmpData = _image.LockBits(_rect, ImageLockMode.ReadWrite, _image.PixelFormat);
            _rgbValues = new byte[Math.Abs(_bmpData.Stride) * _image.Height];
            Marshal.Copy(_bmpData.Scan0, _rgbValues, 0, Math.Abs(_bmpData.Stride) * _image.Height);
        }

        public void SaveResultImage()
        {
            _image.Save("d:\\nigga1.jpg.", ImageFormat.Jpeg);
        }

        public void ConcatImage(List<CustomSocket> sockets)
        {
            _rgbValues = new byte[Math.Abs(_bmpData.Stride) * _image.Height];

            foreach (var socket in sockets)
            {
                _rgbValues.Concat(socket.Data.PartOfImage);
            }

            Marshal.Copy(_rgbValues, 0, _bmpData.Scan0, Math.Abs(_bmpData.Stride) * _image.Height);
            _image.UnlockBits(_bmpData);
        }

        public List<ImagePartDto> CreateParallelData()
        {
            //Используем ArrayExtensions.Split() для деления массива на части и отправки на сокеты.
            var partArray = _rgbValues.Split(_parts).ToArray();
            int size = Math.Abs(_bmpData.Stride) * _image.Height / _parts;
            List<ImagePartDto> chunks = new List<ImagePartDto>();

            for (var i = 0; i < _parts; i++)
            {
                if (i + 1 == _parts && size % _parts != 0)
                {
                    chunks.Add(new ImagePartDto
                    {
                        BufferSize = size + size % _parts,
                        PartOfImage = partArray[i].ToArray()
                    });

                    continue;
                }

                chunks.Add(new ImagePartDto
                {
                    BufferSize = size,
                    PartOfImage = partArray[i].ToArray()
                });
            }

            return chunks;
        }

        public void SendParallelData(List<CustomSocket> sockets)
        {
            List<Action> actions = new List<Action>();

            foreach (var sock in sockets)
            {
                actions.Add(() =>
                {
                    sock.Send(Serialize(sock.Data));
                    sock.Data.PartOfImage = new byte[sock.Data.BufferSize];
                    sock.RecieveData();
                });
            }

            Parallel.Invoke(actions.ToArray());
        }
        
        private byte[] Serialize(object partOfImage)
        {
            if (partOfImage == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            using MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, partOfImage);

            return ms.ToArray();
        }
    }
}
