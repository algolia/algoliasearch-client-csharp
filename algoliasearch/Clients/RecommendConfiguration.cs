/*
* Recommend API
*
* The Recommend API lets you retrieve recommendations from one of Algolia's AI recommendation models that you previously trained on your data.  ## Client libraries  Use Algolia's API clients and libraries to reliably integrate Algolia's APIs with your apps. The official API clients are covered by Algolia's [Service Level Agreement](https://www.algolia.com/policies/sla/).  See: [Algolia's ecosystem](https://www.algolia.com/doc/guides/getting-started/how-algolia-works/in-depth/ecosystem/)  ## Base URLs  The base URLs for requests to the Recommend API are:  - `https://{APPLICATION_ID}.algolia.net` - `https://{APPLICATION_ID}-dsn.algolia.net`.   If your subscription includes a [Distributed Search Network](https://dashboard.algolia.com/infra),   this ensures that requests are sent to servers closest to users.  Both URLs provide high availability by distributing requests with load balancing.  **All requests must use HTTPS.**  ## Retry strategy  To guarantee a high availability, implement a retry strategy for all API requests using the URLs of your servers as fallbacks:  - `https://{APPLICATION_ID}-1.algolianet.com` - `https://{APPLICATION_ID}-2.algolianet.com` - `https://{APPLICATION_ID}-3.algolianet.com`  These URLs use a different DNS provider than the primary URLs. You should randomize this list to ensure an even load across the three servers.  All Algolia API clients implement this retry strategy.  ## Authentication  To authenticate your API requests, add these headers:  - `x-algolia-application-id`. Your Algolia application ID. - `x-algolia-api-key`. An API key with the necessary permissions to make the request.   The required access control list (ACL) to make a request is listed in each endpoint's reference.  You can find your application ID and API key in the [Algolia dashboard](https://dashboard.algolia.com/account).  ## Request format  Request bodies must be JSON objects.  ## Response status and errors  The Recommend API returns JSON responses. Since JSON doesn't guarantee any specific ordering, don't rely on the order of attributes in the API response.  Successful responses return a `2xx` status. Client errors return a `4xx` status. Server errors are indicated by a `5xx` status. Error responses have a `message` property with more information.  ## Version  The current version of the Recommend API is version 1, as indicated by the `/1/` in each endpoint's URL. 
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
/// Recommend client configuration
/// </summary>
public sealed class RecommendConfig : AlgoliaConfig
{
  /// <summary>
  /// The configuration of the Recommend client
  /// A client should have it's own configuration ie on configuration per client instance
  /// </summary>
  /// <param name="appId">Your application ID</param>
  /// <param name="apiKey">Your API Key</param>
  public RecommendConfig(string appId, string apiKey) : base(appId, apiKey, "Recommend", "7.13.3")
  {
    DefaultHosts = GetDefaultHosts(appId);
    Compression = CompressionType.None;
    ReadTimeout = TimeSpan.FromMilliseconds(5000);
    WriteTimeout = TimeSpan.FromMilliseconds(30000);
    ConnectTimeout = TimeSpan.FromMilliseconds(2000);
  }
  private static List<StatefulHost> GetDefaultHosts(string appId)
  {
    var hosts = new List<StatefulHost>
  {
    new()
    {
      Url = $"{appId}-dsn.algolia.net",
      Up = true,
      LastUse = DateTime.UtcNow,
      Accept = CallType.Read
    },
    new()
    {
      Url = $"{appId}.algolia.net", Up = true, LastUse = DateTime.UtcNow, Accept = CallType.Write,
    }
  };

    var commonHosts = new List<StatefulHost>
  {
    new()
    {
      Url = $"{appId}-1.algolianet.com",
      Up = true,
      LastUse = DateTime.UtcNow,
      Accept = CallType.Read | CallType.Write,
    },
    new()
    {
      Url = $"{appId}-2.algolianet.com",
      Up = true,
      LastUse = DateTime.UtcNow,
      Accept = CallType.Read | CallType.Write,
    },
    new()
    {
      Url = $"{appId}-3.algolianet.com",
      Up = true,
      LastUse = DateTime.UtcNow,
      Accept = CallType.Read | CallType.Write,
    }
  }.Shuffle();

    hosts.AddRange(commonHosts);
    return hosts;
  }
}
