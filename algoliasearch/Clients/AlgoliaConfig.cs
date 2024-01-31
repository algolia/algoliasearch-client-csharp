using System;
using System.Collections.Generic;
using Algolia.Search.Http;
using Algolia.Search.Models.Common;
using Algolia.Search.Serializer;
using Algolia.Search.Transport;
using Algolia.Search.Utils;
using Microsoft.Extensions.Logging;

namespace Algolia.Search.Clients
{
  /// <summary>
  /// Algolia's client configuration
  /// </summary>
  public abstract class AlgoliaConfig
  {
    /// <summary>
    /// Create a new Algolia's configuration for the given credentials
    /// </summary>
    /// <param name="appId">Your application ID</param>
    /// <param name="apiKey">Your API Key</param>
    /// <param name="clientName">The client name</param>
    protected AlgoliaConfig(string appId, string apiKey, string clientName)
    {
      AppId = appId;
      ApiKey = apiKey;
      UserAgent = new AlgoliaUserAgent(clientName);
      DefaultHeaders = new Dictionary<string, string>
      {
        { Defaults.AlgoliaApplicationHeader.ToLowerInvariant(), AppId },
        { Defaults.AlgoliaApiKeyHeader.ToLowerInvariant(), ApiKey },
        { Defaults.UserAgentHeader.ToLowerInvariant(), "" },
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

    /// <summary>
    /// The user-agent header
    /// </summary>
    public AlgoliaUserAgent UserAgent { get; }

    /// <summary>
    /// Build the headers for the request
    /// </summary>
    /// <returns></returns>
    internal Dictionary<string, string> BuildHeaders()
    {
      DefaultHeaders[Defaults.UserAgentHeader.ToLowerInvariant()] = UserAgent.ToString();
      return DefaultHeaders;
    }
  }
}
