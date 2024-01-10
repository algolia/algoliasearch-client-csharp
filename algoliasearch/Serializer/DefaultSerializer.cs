using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Algolia.Search.Clients;
using Algolia.Search.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Algolia.Search.Serializer
{
  internal class CustomJsonCodec
  {
    private const string _contentType = "application/json";

    private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
    {
      // OpenAPI generated types generally hide default constructors.
      ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
      ContractResolver = new DefaultContractResolver
      {
        NamingStrategy = new CamelCaseNamingStrategy
        {
          OverrideSpecifiedNames = false
        }
      }
    };

    public CustomJsonCodec()
    {

    }

    public CustomJsonCodec(JsonSerializerSettings serializerSettings)
    {
      _serializerSettings = serializerSettings;
    }

    /// <summary>
    /// Serialize the object into a JSON string.
    /// </summary>
    /// <param name="obj">Object to be serialized.</param>
    /// <returns>A JSON string.</returns>
    public string Serialize(object obj)
    {
      if (obj != null && obj is AbstractSchema)
      {
        // the object to be serialized is an oneOf/anyOf schema
        return ((AbstractSchema)obj).ToJson();
      }
      else
      {
        return JsonConvert.SerializeObject(obj, _serializerSettings);
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
    internal async Task<object> Deserialize(Stream response, Type type)
    {
      if (type == typeof(byte[])) // return byte array
      {
        using (var reader = new StreamReader(response))
        {
          return Encoding.UTF8.GetBytes(await reader.ReadToEndAsync().ConfigureAwait(false));
        }
      }

      if (type == typeof(Stream))
      {
        return response;
      }

      if (type.Name.StartsWith("System.Nullable`1[[System.DateTime")) // return a datetime object
      {
        using (var reader = new StreamReader(response))
        {
          var text = await reader.ReadToEndAsync().ConfigureAwait(false);
          return DateTime.Parse(text, null, System.Globalization.DateTimeStyles.RoundtripKind);
        }
      }

      if (type == typeof(string) || type.Name.StartsWith("System.Nullable")) // return primitive type
      {
        using (var reader = new StreamReader(response))
        {
          var text = await reader.ReadToEndAsync().ConfigureAwait(false);
          return Convert.ChangeType(text, type);
        }
      }

      // at this point, it must be a model (json)
      try
      {
        using (var reader = new StreamReader(response))
        {
          var text = await reader.ReadToEndAsync().ConfigureAwait(false);
          return JsonConvert.DeserializeObject(text, type, _serializerSettings);
        }
      }
      catch (Exception e)
      {
        throw new ApiException(500, e.Message);
      }
    }

    public string RootElement { get; set; }
    public string Namespace { get; set; }
    public string DateFormat { get; set; }

    public string ContentType
    {
      get { return _contentType; }
      set { throw new InvalidOperationException("Not allowed to set content type."); }
    }
  }

  /// <summary>
  /// Default Serializer using Newtonsoft JSON
  /// Implementation of <see cref="ISerializer"/>
  /// https://www.newtonsoft.com/json/help/html/performance.htm
  /// </summary>
  internal class DefaultSerializer : ISerializer
  {
    private static readonly UTF8Encoding DefaultEncoding = new UTF8Encoding(false);

    private static readonly int DefaultBufferSize = 1024;

    // Re-usable Serializer instance, so it keeps in memory the JsonSerializationContract
    private static readonly JsonSerializer Serializer = JsonSerializer.Create(JsonConfig.AlgoliaJsonSerializerSettings);

    // Buffer sized as recommended by Bradley Grainger, http://faithlife.codes/blog/2012/06/always-wrap-gzipstream-with-bufferedstream/
    private static readonly int GZipBufferSize = 8192;

    public void Serialize<T>(T data, Stream stream, CompressionType compressionType)
    {
      if (compressionType == CompressionType.GZIP)
      {
        using (var gzipStream = new GZipStream(stream, CompressionMode.Compress, true))
        using (var sw = new StreamWriter(gzipStream, DefaultEncoding, GZipBufferSize))
        using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
        {
          JsonSerialize(jtw);
        }
      }
      else
      {
        using (var sw = new StreamWriter(stream, DefaultEncoding, DefaultBufferSize, true))
        using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
        {
          JsonSerialize(jtw);
        }
      }

      void JsonSerialize(JsonTextWriter writer)
      {
        Serializer.Serialize(writer, data);
        writer.Flush();
      }
    }

    public T Deserialize<T>(Stream stream)
    {
      if (stream == null || stream.CanRead == false)
        return default;

      using (stream)
      using (var sr = new StreamReader(stream))
      using (var jtr = new JsonTextReader(sr))
      {
        return Serializer.Deserialize<T>(jtr);
      }
    }
  }
}
