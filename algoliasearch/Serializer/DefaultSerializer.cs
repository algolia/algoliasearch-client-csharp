using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Algolia.Search.Exceptions;
using Algolia.Search.Models.Common;
using Microsoft.Extensions.Logging;

namespace Algolia.Search.Serializer;

internal class DefaultJsonSerializer(ILoggerFactory logger) : ISerializer
{
  private readonly ILogger<DefaultJsonSerializer> _logger =
    logger.CreateLogger<DefaultJsonSerializer>();

  /// <summary>
  /// Serialize the object into a JSON string.
  /// </summary>
  /// <param name="data">Object to be serialized.</param>
  /// <returns>A JSON string.</returns>
  public string Serialize(object data)
  {
    var sw = Stopwatch.StartNew();
    try
    {
      string result;
      if (data is not AbstractSchema schema)
      {
        result = JsonSerializer.Serialize(data, JsonConfig.Options);
      }
      else
      {
        // the object to be serialized is a oneOf/anyOf schema
        result = schema.ToJson();
      }

      _logger.LogDebug("Request body serialized in {Duration}ms", sw.ElapsedMilliseconds);

      return result;
    }
    catch (Exception ex)
    {
      var dataType = data?.GetType()?.ToString() ?? "unknown";

      if (_logger.IsEnabled(LogLevel.Error))
      {
        _logger.LogError(ex, "Serialization error for type {Type}", dataType);
      }

      throw new AlgoliaException($"Error while serializing object of type {dataType}", ex);
    }
    finally
    {
      sw.Stop();
    }
  }

  public async Task<T> Deserialize<T>(Stream response)
  {
    var result = (T)await Deserialize(response, typeof(T)).ConfigureAwait(false);
    return result;
  }

  /// <summary>
  /// Deserialize the JSON string into a proper object.
  /// </summary>
  /// <param name="response">The HTTP response.</param>
  /// <param name="type">Object type.</param>
  /// <returns>Object representation of the JSON string.</returns>
  private async Task<object> Deserialize(Stream response, Type type)
  {
    var sw = Stopwatch.StartNew();
    try
    {
      using var reader = new StreamReader(response);
      var readToEndAsync = await reader.ReadToEndAsync().ConfigureAwait(false);

      var result = string.IsNullOrEmpty(readToEndAsync)
        ? null
        : JsonSerializer.Deserialize(readToEndAsync, type, JsonConfig.Options);

      _logger.LogDebug("Response body deserialized in {Duration}ms", sw.ElapsedMilliseconds);

      return result;
    }
    catch (Exception ex)
    {
      if (_logger.IsEnabled(LogLevel.Error))
      {
        _logger.LogError(ex, "Failed to deserialize response of type {Type}", type);
      }

      throw new AlgoliaException($"Error while deserializing response of type {type}", ex);
    }
    finally
    {
      sw.Stop();
    }
  }
}
