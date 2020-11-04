using System;

namespace ConsoleClient.Models.Dto
{
    [Serializable]
    internal class ImagePartDto
    {
        public int BufferSize { get; set; }

        public byte[] PartOfImage  { get; set; }
    }
}
