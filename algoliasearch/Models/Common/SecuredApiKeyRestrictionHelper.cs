using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Algolia.Search.Http;
using Algolia.Search.Utils;

namespace Algolia.Search.Models.Common;

/// <summary>
/// Secured Api Key restrictions
/// </summary>
public partial class SecuredApiKeyRestriction
{

  /// <summary>
  /// Transforms the restriction into a query string
  /// </summary>
  /// <returns></returns>
  public string ToQueryString()
  {
    string restrictionQuery = null;
    if (Query != null)
    {
      restrictionQuery = ToQueryString(Query);
    }

    var restrictions = ToQueryString(this, nameof(Query));
    var array = new[] { restrictionQuery, restrictions };

    return string.Join("&", array.Where(s => !string.IsNullOrEmpty(s)));
  }

  /// <summary>
  /// Transform a poco (only class of primitive objects) to a query string
  /// </summary>
  /// <param name="value"></param>
  /// <param name="ignoreList"></param>
  /// <typeparam name="T"></typeparam>
  /// <returns></returns>
  private static string ToQueryString<T>(T value, params string[] ignoreList)
  {
    var properties = typeof(T).GetTypeInfo()
      .DeclaredProperties.Where(p =>
        p.GetValue(value, null) != null && !ignoreList.Contains(p.Name) &&
        p.GetCustomAttribute<JsonPropertyNameAttribute>() != null)
      .Select(p => new
      {
        propsName = p.GetCustomAttribute<JsonPropertyNameAttribute>().Name,
        value = ClientUtils.ParameterToString(p.GetValue(value, null))
      }).ToDictionary(p => p.propsName, p => p.value);

    return properties.ToQueryString();
  }

}
