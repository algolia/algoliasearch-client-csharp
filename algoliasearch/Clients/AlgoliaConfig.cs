using System;
using System.Collections.Generic;
using System.Reflection;
using Algolia.Search.Models;
using Algolia.Search.Models.Common;
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

    // get the dotnet runtime version
    private static readonly string DotnetVersion = Environment.Version.ToString();

    /// <summary>
    /// Create a new Algolia's configuration for the given credentials
    /// </summary>
    /// <param name="appId">Your application ID</param>
    /// <param name="apiKey">Your API Key</param>
    /// <param name="client">The client name</param>
    protected AlgoliaConfig(string appId, string apiKey, string client)
    {
      AppId = appId;
      ApiKey = apiKey;

      DefaultHeaders = new Dictionary<string, string>
      {
        { Defaults.AlgoliaApplicationHeader.ToLowerInvariant(), AppId },
        { Defaults.AlgoliaApiKeyHeader.ToLowerInvariant(), ApiKey },
        { Defaults.UserAgentHeader.ToLowerInvariant(), $"Algolia for Csharp ({ClientVersion}); {client} ({ClientVersion}); Dotnet ({DotnetVersion})" },
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
    /// Set the connect timeout for all requests
    /// </summary>
    public TimeSpan? ConnectTimeout { get; set; }

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
