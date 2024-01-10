using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using Newtonsoft.Json;

namespace Algolia.Search.Utils
{
  internal static class QueryStringHelper
  {
    /// <summary>
    /// Transfrom a poco (only class of primitive objects) to a query string
    /// </summary>
    /// <param name="value"></param>
    /// <param name="ignoreList"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static string ToQueryString<T>(T value, params string[] ignoreList)
    {
      // Flat properties
      IEnumerable<string> properties = typeof(T).GetTypeInfo()
          .DeclaredProperties.Where(p =>
              !(p.PropertyType.GetTypeInfo().IsGenericType && typeof(IEnumerable).GetTypeInfo()
                    .IsAssignableFrom(p.PropertyType.GetTypeInfo())) &&
              p.GetValue(value, null) != null && !ignoreList.Contains(p.Name) &&
              p.GetCustomAttribute<JsonPropertyAttribute>() == null)
          .Select(p =>
          {
            string encodedValue = Nullable.GetUnderlyingType(p.PropertyType) == typeof(bool)
                      ? Uri.EscapeDataString(p.GetValue(value, null).ToString().ToLower())
                      : Uri.EscapeDataString(p.GetValue(value, null).ToString());
            return p.Name + "=" + encodedValue;
          });

      // List<T> and List<List<T>> properties
      IEnumerable<string> listProperties = typeof(T).GetTypeInfo()
          .DeclaredProperties.Where(p =>
              p.PropertyType.GetTypeInfo().IsGenericType &&
              typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(p.PropertyType.GetTypeInfo()) &&
              p.GetValue(value, null) != null && !ignoreList.Contains(p.Name) &&
              p.GetCustomAttribute<JsonPropertyAttribute>() == null)
          .Select(p =>
          {
            string values;
            var genericTypeArgument = p.PropertyType.GenericTypeArguments[0];

            // In case of nested lists
            if (typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(genericTypeArgument.GetTypeInfo()) &&
                      genericTypeArgument != typeof(string))
            {
              if (typeof(IEnumerable<float>).GetTypeInfo()
                        .IsAssignableFrom(genericTypeArgument.GetTypeInfo()))
              {
                IEnumerable<IEnumerable<float>> nestedParametersLists =
                          ((IEnumerable)p.GetValue(value, null)).Cast<IEnumerable<float>>();
                // Culture set to en-US to have floating points separators with "."
                values = WrapValues(string.Join(",",
                          nestedParametersLists.Select(f =>
                              WrapValues(
                                  string.Join(",", f.Select(x => x.ToString(CultureInfo.InvariantCulture)))))));
              }
              else
              {
                IEnumerable<IEnumerable<object>> nestedParametersLists =
                          ((IEnumerable)p.GetValue(value, null)).Cast<IEnumerable<object>>();
                values = WrapValues(string.Join(",",
                          nestedParametersLists.Select(x =>
                              WrapValues(string.Join(",", x.Select(y => "\"" + y.ToString().Replace("\"", "\\\"") + "\""))))));


              }
            }
            else
            {
              // One level list
              IEnumerable<object> parameterList = ((IEnumerable)p.GetValue(value, null)).Cast<object>();
              values = string.Join(",", parameterList);
            }

            return p.Name + "=" + WebUtility.UrlEncode(values);
          });

      // Handle properties with JsonPropertyAttribute
      IEnumerable<string> propertiesWithJsonAttribute = typeof(T).GetTypeInfo()
          .DeclaredProperties.Where(p =>
              p.GetValue(value, null) != null && !ignoreList.Contains(p.Name) &&
              p.GetCustomAttribute<JsonPropertyAttribute>() != null)
          .Select(p =>
              p.GetCustomAttribute<JsonPropertyAttribute>().PropertyName + "=" +
              Uri.EscapeDataString(p.GetValue(value, null).ToString()));

      // Merge twoListBeforeSending
      var mergedProperties = propertiesWithJsonAttribute.Concat(properties).Concat(listProperties);

      return string.Join("&", mergedProperties.ToArray());
    }

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

    private static string WrapValues(string values)
    {
      return "[" + values + "]";
    }
  }
}
