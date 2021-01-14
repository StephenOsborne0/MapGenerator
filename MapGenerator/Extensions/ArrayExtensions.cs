using System.Collections.Generic;

namespace MapGenerator.Extensions
{
    public static class ArrayExtensions
    {
        public static List<T> ToList<T>(this T[,] array) where T : class
        {
            var list = new List<T>();

            for (int y = 0; y < array.GetLength(1); y++)
            for (int x = 0; x < array.GetLength(0); x++)
                    list.Add(array[x, y]);

            return list;
        }
    }
}
