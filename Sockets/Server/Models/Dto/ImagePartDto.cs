using System;

namespace Server.Models.Dto
{
    [Serializable]
    internal class ImagePartDto
    {
        public int BufferSize { get; set; }

        public byte[] PartOfImage { get; set; }
    }
}
