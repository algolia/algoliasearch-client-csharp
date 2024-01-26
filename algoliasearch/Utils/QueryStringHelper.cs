using System;
using System.Collections.Generic;
using System.Linq;

namespace Algolia.Search.Utils;

internal static class QueryStringHelper
{
  public static string ToQueryString(this IDictionary<string, string> dic)
  {
    if (dic == null)
    {
      throw new ArgumentNullException(nameof(dic));
    }

    return string.Join("&",
      dic.Select(kvp =>
        string.Format($"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}")));
  }
}
