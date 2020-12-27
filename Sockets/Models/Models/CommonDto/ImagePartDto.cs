using System;

namespace Models.CommonDto
{
    [Serializable]
    public class ImagePartDto
    {
        public int BufferSize { get; set; }

        public byte[] PartOfImage { get; set; }

        public int WidthImage { get; set; }
    }
}
