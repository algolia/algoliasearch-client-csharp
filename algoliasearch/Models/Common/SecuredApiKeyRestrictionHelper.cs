using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Algolia.Search.Http;
using Algolia.Search.Utils;

namespace Algolia.Search.Models.Search;

/// <summary>
/// Secured Api Key restrictions
/// </summary>
public partial class SecuredApiKeyRestrictions
{
  /// <summary>
  /// Transforms the restriction into a query string
  /// </summary>
  /// <returns></returns>
  public string ToQueryString()
  {
    var restrictions = ToQueryMap(this, nameof(SearchParams));
    if (SearchParams != null)
    {
      // merge SearchParams into restrictions
      restrictions = restrictions
        .Concat(ToQueryMap(SearchParams))
        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    return QueryStringHelper.ToQueryString(
      restrictions.OrderBy(x => x.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
    );
  }

  /// <summary>
  /// Transform a poco to a map of query parameters
  /// </summary>
  /// <param name="value"></param>
  /// <param name="ignoreList"></param>
  /// <typeparam name="T"></typeparam>
  /// <returns></returns>
  private static Dictionary<string, string> ToQueryMap<T>(T value, params string[] ignoreList)
  {
    return typeof(T)
      .GetTypeInfo()
      .DeclaredProperties.Where(p =>
        p.GetValue(value, null) != null
        && !ignoreList.Contains(p.Name)
        && p.GetCustomAttribute<JsonPropertyNameAttribute>() != null
      )
      .Select(p => new
      {
        propsName = p.GetCustomAttribute<JsonPropertyNameAttribute>().Name,
        value = QueryStringHelper.ParameterToString(p.GetValue(value, null)),
      })
      .ToDictionary(p => p.propsName, p => p.value);
  }
}
