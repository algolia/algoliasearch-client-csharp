using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Algolia.Search.Serializer;

/// <summary>
///  Factory to create a JsonConverter for enums
/// </summary>
public class JsonStringEnumConverterFactory : JsonConverterFactory
{
  /// <summary>
  /// Check if the type is an enum
  /// </summary>
  /// <param name="typeToConvert"></param>
  /// <returns></returns>
  public override bool CanConvert(Type typeToConvert)
  {
    return typeToConvert.IsEnum;
  }

  /// <summary>
  /// Create a new converter
  /// </summary>
  /// <param name="typeToConvert"></param>
  /// <param name="options"></param>
  /// <returns></returns>
  public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
  {
    var type = typeof(JsonStringEnumConverter<>).MakeGenericType(typeToConvert);
    return (JsonConverter)Activator.CreateInstance(type)!;
  }
}

/// <summary>
/// Custom JsonConverter to convert enum to string, using the JsonPropertyNameAttribute if present
/// </summary>
public class JsonStringEnumConverter<TEnum> : JsonConverter<TEnum>
  where TEnum : struct, Enum
{
  private readonly Dictionary<TEnum, string> _enumToString = new();
  private readonly Dictionary<string, TEnum> _stringToEnum = new();
  private readonly Dictionary<int, TEnum> _numberToEnum = new();

  /// <summary>
  /// Constructor to create the converter
  /// </summary>
  public JsonStringEnumConverter()
  {
    var type = typeof(TEnum);
    foreach (var value in Enum.GetValues(type))
    {
      var enumMember = type.GetMember(value.ToString())[0];
      var attr = enumMember
        .GetCustomAttributes(typeof(JsonPropertyNameAttribute), false)
        .Cast<JsonPropertyNameAttribute>()
        .FirstOrDefault();

      var num = Convert.ToInt32(type.GetField("value__")?.GetValue(value));
      if (attr?.Name != null)
      {
        _enumToString.Add((TEnum)value, attr.Name);
        _stringToEnum.Add(attr.Name, (TEnum)value);
        _numberToEnum.Add(num, (TEnum)value);
      }
      else
      {
        _enumToString.Add((TEnum)value, value.ToString());
        _stringToEnum.Add(value.ToString(), (TEnum)value);
        _numberToEnum.Add(num, (TEnum)value);
      }
    }
  }

  /// <summary>
  /// Read the enum from the json
  /// </summary>
  /// <param name="reader"></param>
  /// <param name="typeToConvert"></param>
  /// <param name="options"></param>
  /// <returns></returns>
  public override TEnum Read(
    ref Utf8JsonReader reader,
    Type typeToConvert,
    JsonSerializerOptions options
  )
  {
    var type = reader.TokenType;
    switch (type)
    {
      case JsonTokenType.String:
        {
          var stringValue = reader.GetString();
          if (stringValue != null && _stringToEnum.TryGetValue(stringValue, out var enumValue))
          {
            return enumValue;
          }

          break;
        }
      case JsonTokenType.Number:
        {
          var numValue = reader.GetInt32();
          _numberToEnum.TryGetValue(numValue, out var enumValue);
          return enumValue;
        }
    }

    return default;
  }

  /// <summary>
  /// Write the enum to the json
  /// </summary>
  /// <param name="writer"></param>
  /// <param name="value"></param>
  /// <param name="options"></param>
  public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
  {
    var success = _enumToString.TryGetValue(value, out var stringValue);
    if (success)
    {
      writer.WriteStringValue(stringValue);
    }
    else
    {
      writer.WriteNullValue();
    }
  }
}
