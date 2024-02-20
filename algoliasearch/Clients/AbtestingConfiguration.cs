/*
* A/B Testing API
*
* API powering the A/B Testing feature of Algolia.
*
* The version of the OpenAPI document: 1.0.0
* Generated by: https://github.com/openapitools/openapi-generator.git
*/


using System;
using System.Collections.Generic;
using Algolia.Search.Models.Common;
using Algolia.Search.Transport;
using Algolia.Search.Utils;

namespace Algolia.Search.Clients;

/// <summary>
/// Abtesting client configuration
/// </summary>
public sealed class AbtestingConfig : AlgoliaConfig
{
  /// <summary>
  /// The configuration of the Abtesting client
  /// A client should have it's own configuration ie on configuration per client instance
  /// </summary>
  /// <param name="appId">Your application ID</param>
  /// <param name="apiKey">Your API Key</param>
  /// <param name="region">Targeted region (optional)</param>
  public AbtestingConfig(string appId, string apiKey, string region = null) : base(appId, apiKey, "Abtesting")
  {
    DefaultHosts = GetDefaultHosts(region);
    Compression = CompressionType.None;
  }
  private static List<StatefulHost> GetDefaultHosts(string region)
  {
    var regions = new List<string> { "de", "us" };
    if (region != null && !regions.Contains(region))
    {
      throw new ArgumentException($"`region` must be one of the following: {string.Join(", ", regions)}");
    }

    var selectedRegion = region == null ? "analytics.algolia.com" : "analytics.{region}.algolia.com".Replace("{region}", region);

    var hosts = new List<StatefulHost>
  {
    new()
    {
      Url = selectedRegion, Accept = CallType.Read | CallType.Write
    }
  };
    return hosts;
  }
}
