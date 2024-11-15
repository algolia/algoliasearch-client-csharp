/*
* Ingestion API
*
* The Ingestion API lets you connect third-party services and platforms with Algolia and schedule tasks to ingest your data. The Ingestion API powers the no-code [data connectors](https://dashboard.algolia.com/connectors).  ## Base URLs  The base URLs for requests to the Ingestion API are:  - `https://data.us.algolia.com` - `https://data.eu.algolia.com`  Use the URL that matches your [analytics region](https://dashboard.algolia.com/account/infrastructure/analytics).  **All requests must use HTTPS.**  ## Authentication  To authenticate your API requests, add these headers:  - `x-algolia-application-id`. Your Algolia application ID. - `x-algolia-api-key`. An API key with the necessary permissions to make the request.   The required access control list (ACL) to make a request is listed in each endpoint's reference.  You can find your application ID and API key in the [Algolia dashboard](https://dashboard.algolia.com/account).  ## Request format  Request bodies must be JSON objects.  ## Response status and errors  Response bodies are JSON objects.  Successful responses return a `2xx` status. Client errors return a `4xx` status. Server errors are indicated by a `5xx` status. Error responses have a `message` property with more information.  ## Version  The current version of the Ingestion API is version 1, as indicated by the `/1/` in each endpoint's URL. 
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
/// Ingestion client configuration
/// </summary>
public sealed class IngestionConfig : AlgoliaConfig
{
  /// <summary>
  /// The configuration of the Ingestion client
  /// A client should have it's own configuration ie on configuration per client instance
  /// </summary>
  /// <param name="appId">Your application ID</param>
  /// <param name="apiKey">Your API Key</param>
  /// <param name="region">Targeted region </param>
  public IngestionConfig(string appId, string apiKey, string region) : base(appId, apiKey, "Ingestion", "7.9.1")
  {
    DefaultHosts = GetDefaultHosts(region);
    Compression = CompressionType.None;
  }
  private static List<StatefulHost> GetDefaultHosts(string region)
  {
    var regions = new List<string> { "eu", "us" };
    if (region == null || !regions.Contains(region))
    {
      throw new ArgumentException($"`region` is required and must be one of the following: {string.Join(", ", regions)}");
    }

    var selectedRegion = "data.{region}.algolia.com".Replace("{region}", region);

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
