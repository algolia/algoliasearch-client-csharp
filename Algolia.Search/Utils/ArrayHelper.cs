using System;
using System.Collections.Generic;
using System.Linq;

namespace Algolia.Search.Utils
{
    public static class EnumerableExtensions
    {
        static Random random = new Random();
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.ShuffleIterator(random);
        }

        private static IEnumerable<T> ShuffleIterator<T>(this IEnumerable<T> source, Random rng)
        {
            var buffer = source.ToList();
            for (int i = 0; i < buffer.Count; i++)
            {
                int j = rng.Next(i, buffer.Count);
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }
    }

}
