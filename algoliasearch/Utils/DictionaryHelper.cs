using System.Collections.Generic;
using System.Linq;

namespace Algolia.Search.Utils
{
  /// <summary>
  /// Helper regarding dictionary merging
  /// </summary>
  public static class DictionaryHelper
  {
    /// <summary>
    /// Merge a into b removing the duplicates from b if they exists
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    public static IDictionary<TKey, TValue> MergeWith<TKey, TValue>(this IDictionary<TKey, TValue> a,
        Dictionary<TKey, TValue> b)
    {
      if (a == null && b != null)
      {
        return b;
      }

      if (a != null && b == null)
      {
        return a;
      }

      var mergeWith = a?.Concat(b.Where(kvp => !a.ContainsKey(kvp.Key) && kvp.Value != null))
        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
      return mergeWith;
    }
  }
}
