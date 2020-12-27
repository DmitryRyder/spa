namespace Server
{
    internal class MockFilter
    {
        public byte[] Implementation(byte[] rgbValues, int widthImage)
        {
            for (int i = 0; i < rgbValues.Length - 3 * widthImage; i += 3)
            {
                int sumR = 0;
                int sumG = 0;
                int sumB = 0;
                int transition = i;
                for (int j = 1; j < 4; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        sumR += rgbValues[transition + k * 3];
                        sumG += rgbValues[transition + 1 + k * 3];
                        sumB += rgbValues[transition + 2 + k * 3];
                    }
                    transition += widthImage;
                }
                rgbValues[i + widthImage + 1] = (byte)(sumR / 9);
                rgbValues[i + widthImage + 2] = (byte)(sumG / 9);
                rgbValues[i + widthImage + 3] = (byte)(sumB / 9);
            }

            return rgbValues;
        }
    }
}
