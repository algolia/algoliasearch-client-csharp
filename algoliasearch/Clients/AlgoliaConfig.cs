using System;
using System.Collections.Generic;
using System.Reflection;
using Algolia.Search.Models;
using Algolia.Search.Serializer;
using Algolia.Search.Transport;
using Algolia.Search.Utils;

namespace Algolia.Search.Clients
{
  /// <summary>
  /// Algolia's client configuration
  /// </summary>
  public abstract class AlgoliaConfig
  {
    private static readonly string ClientVersion =
      typeof(AlgoliaConfig).GetTypeInfo().Assembly.GetName().Version.ToString();

    /// <summary>
    /// Create a new Algolia's configuration for the given credentials
    /// </summary>
    /// <param name="applicationId">Your application ID</param>
    /// <param name="apiKey">Your API Key</param>
    protected AlgoliaConfig(string applicationId, string apiKey)
    {
      AppId = applicationId;
      ApiKey = apiKey;

      DefaultHeaders = new Dictionary<string, string>
      {
        { Defaults.AlgoliaApplicationHeader.ToLowerInvariant(), AppId },
        { Defaults.AlgoliaApiKeyHeader.ToLowerInvariant(), ApiKey },
        { Defaults.UserAgentHeader.ToLowerInvariant(), $"Algolia For Csharp {ClientVersion}" },
        { Defaults.Connection.ToLowerInvariant(), Defaults.KeepAlive },
        { Defaults.AcceptHeader.ToLowerInvariant(), JsonConfig.JsonContentType }
      };
    }

    /// <summary>
    /// The application ID
    /// </summary>
    /// <returns></returns>
    public string AppId { get; }

    /// <summary>
    /// The admin API Key
    /// </summary>
    /// <returns></returns>
    public string ApiKey { get; }

    /// <summary>
    /// Configurations hosts
    /// </summary>
    public List<StatefulHost> CustomHosts { get; set; }

    /// <summary>
    /// Algolia's default headers.
    /// Will be sent for every request
    /// </summary>
    public Dictionary<string, string> DefaultHeaders { get; set; }

    /// <summary>
    /// Set the read timeout for all requests
    /// </summary>
    public TimeSpan? ReadTimeout { get; set; }

    /// <summary>
    /// Set the read timeout for all requests
    /// </summary>
    public TimeSpan? WriteTimeout { get; set; }

    /// <summary>
    /// Compression for outgoing http requests  <see cref="CompressionType"/>
    /// </summary>
    public CompressionType Compression { get; set; }

    /// <summary>
    /// Configurations hosts
    /// </summary>
    protected internal List<StatefulHost> DefaultHosts { get; set; }
  }
}
