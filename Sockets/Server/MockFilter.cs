using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    internal static class MockFilter
    {
        public static byte[] Implementation(byte[] rgbValues/*, int index, int lengthOfPart*/)
        {
            //Console.WriteLine($"Выполняется задача {Task.CurrentId}");
            //Console.WriteLine($"длина массива: {rgbValues.Length}");
            //Console.WriteLine($"индекс: {index}");
            //Console.WriteLine($"длина части: {lengthOfPart}");

            //var counterCase = index * lengthOfPart + 2;
            //var counterCase2 = (lengthOfPart * index) + lengthOfPart - 1;
            //var length = rgbValues.Length;

            //for (int counter = index * lengthOfPart; counter < (lengthOfPart * index) + lengthOfPart - 1; counter += 3)
            //    rgbValues[counter] = 255;

            for (int counter = 2; counter < rgbValues.Length; counter += 3)
                rgbValues[counter] = 255;

            return rgbValues;
        }
    }
}
