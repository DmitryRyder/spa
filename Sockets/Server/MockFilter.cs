namespace Server
{
    internal class MockFilter
    {
        public byte[] Implementation(byte[] rgbValues)
        {
            for (int counter = 2; counter < rgbValues.Length; counter += 3)
                rgbValues[counter] = 255;

            return rgbValues;
        }
    }
}
