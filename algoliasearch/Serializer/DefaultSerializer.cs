using System;
using System.IO;
using System.Threading.Tasks;
using Algolia.Search.Exceptions;
using Algolia.Search.Models.Common;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Algolia.Search.Serializer;

internal class DefaultJsonSerializer : ISerializer
{
  private readonly ILogger<DefaultJsonSerializer> _logger;

  public DefaultJsonSerializer(ILoggerFactory logger)
  {
    _logger = logger.CreateLogger<DefaultJsonSerializer>();
  }

  /// <summary>
  /// Serialize the object into a JSON string.
  /// </summary>
  /// <param name="data">Object to be serialized.</param>
  /// <returns>A JSON string.</returns>
  public string Serialize(object data)
  {
    if (data is AbstractSchema schema)
    {
      // the object to be serialized is an oneOf/anyOf schema
      var serialize = schema.ToJson();
      return serialize;
    }

    return JsonConvert.SerializeObject(data, JsonConfig.AlgoliaJsonSerializerSettings);
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
    try
    {
      using var reader = new StreamReader(response);
      var readToEndAsync = await reader.ReadToEndAsync().ConfigureAwait(false);
      return JsonConvert.DeserializeObject(readToEndAsync, type,
        JsonConfig.AlgoliaJsonSerializerSettings);
    }
    catch (Exception ex)
    {
      if (_logger.IsEnabled(LogLevel.Debug))
      {
        _logger.Log(LogLevel.Debug, ex, "Error while deserializing response");
      }

      throw new AlgoliaException(ex.Message);
    }
  }
}
