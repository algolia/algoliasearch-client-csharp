using System;
using System.Collections.Generic;
using System.Linq;

namespace Algolia.Search.Utils
{
  /// <summary>
  /// Extension method to shuffle List
  /// https://stackoverflow.com/questions/5807128/an-extension-method-on-ienumerable-needed-for-shuffling
  /// </summary>
  internal static class ShuffleHelper
  {
    internal static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
      return source.Shuffle(new Random());
    }

    internal static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));
      if (rng == null)
        throw new ArgumentNullException(nameof(rng));

      return source.ShuffleIterator(rng);
    }

    private static IEnumerable<T> ShuffleIterator<T>(
        this IEnumerable<T> source, Random rng)
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
