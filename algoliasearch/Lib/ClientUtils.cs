//
// Code generated by OpenAPI Generator (https://openapi-generator.tech), manual changes will be lost - read more on https://github.com/algolia/api-clients-automation. DO NOT EDIT.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;

namespace Algolia.Search.Client
{
  /// <summary>
  /// Utility functions providing some benefit to API client consumers.
  /// </summary>
  public static class ClientUtils
  {
    /// <summary>
    /// Convert params to key/value pairs.
    /// Use collectionFormat to properly format lists and collections.
    /// </summary>
    /// <param name="collectionFormat">The swagger-supported collection format, one of: csv, tsv, ssv, pipes, multi</param>
    /// <param name="name">Key name.</param>
    /// <param name="value">Value object.</param>
    /// <returns>A multimap of keys with 1..n associated values.</returns>
    public static Dictionary<string, string> ParameterToDictionary(string collectionFormat, string name, object value)
    {
      var parameters = new Dictionary<string, string>();

      if (value is ICollection collection && collectionFormat == "multi")
      {
        foreach (var item in collection)
        {
          parameters.Add(name, ParameterToString(item));
        }
      }
      else if (value is IDictionary dictionary)
      {
        if (collectionFormat == "deepObject")
        {
          foreach (DictionaryEntry entry in dictionary)
          {
            parameters.Add(name + "[" + entry.Key + "]", ParameterToString(entry.Value));
          }
        }
        else
        {
          foreach (DictionaryEntry entry in dictionary)
          {
            parameters.Add(entry.Key.ToString(), ParameterToString(entry.Value));
          }
        }
      }
      else
      {
        parameters.Add(name, ParameterToString(value));
      }

      return parameters;
    }

    /// <summary>
    /// If parameter is a list, join the list with ",".
    /// Otherwise just return the string.
    /// </summary>
    /// <param name="obj">The parameter (header, path, query, form).</param>
    /// <returns>Formatted string.</returns>
    public static string ParameterToString(object obj)
    {
      if (obj is bool boolean)
        return boolean ? "true" : "false";
      if (obj is ICollection collection)
      {
        List<string> entries = new List<string>();
        foreach (var entry in collection)
          entries.Add(ParameterToString(entry));
        return string.Join(",", entries);
      }
      if (obj is Enum && HasEnumMemberAttrValue(obj))
        return GetEnumMemberAttrValue(obj);

      return Convert.ToString(obj, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Is the Enum decorated with EnumMember Attribute
    /// </summary>
    /// <param name="enumVal"></param>
    /// <returns>true if found</returns>
    private static bool HasEnumMemberAttrValue(object enumVal)
    {
      if (enumVal == null)
        throw new ArgumentNullException(nameof(enumVal));
      var enumType = enumVal.GetType();
      var memInfo = enumType.GetMember(enumVal.ToString() ?? throw new InvalidOperationException());
      var attr = memInfo.FirstOrDefault()?.GetCustomAttributes(false).OfType<EnumMemberAttribute>().FirstOrDefault();
      if (attr != null) return true;
      return false;
    }

    /// <summary>
    /// Get the EnumMember value
    /// </summary>
    /// <param name="enumVal"></param>
    /// <returns>EnumMember value as string otherwise null</returns>
    private static string GetEnumMemberAttrValue(object enumVal)
    {
      if (enumVal == null)
        throw new ArgumentNullException(nameof(enumVal));
      var enumType = enumVal.GetType();
      var memInfo = enumType.GetMember(enumVal.ToString() ?? throw new InvalidOperationException());
      var attr = memInfo.FirstOrDefault()?.GetCustomAttributes(false).OfType<EnumMemberAttribute>().FirstOrDefault();
      if (attr != null)
      {
        return attr.Value;
      }
      return null;
    }
  }
}