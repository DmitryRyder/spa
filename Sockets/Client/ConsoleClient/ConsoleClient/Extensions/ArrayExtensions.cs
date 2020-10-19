using System.Collections.Generic;
using System.Linq;

namespace ConsoleClient.Extensions
{
    internal static class ArrayExtensions
    {
        public static IEnumerable<IEnumerable<T>> Split<T>(this T[] array, int size)
        {
            for (var i = 0; i < array.Length; i += array.Length / size)
            {
                yield return array.Skip(i).Take(array.Length / size).ToArray();
            }
        }
    }
}
