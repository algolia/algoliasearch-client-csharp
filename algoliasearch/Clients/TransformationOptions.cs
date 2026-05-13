using System;
using System.Collections.Generic;
using Algolia.Search.Models.Common;
using Algolia.Search.Transport;

namespace Algolia.Search.Clients;

/// <summary>
/// Configuration options for the ingestion transporter used by <c>*WithTransformation</c> helpers.
/// An ingestion transporter is eagerly created using the provided region and Ingestion API defaults
/// (25 s timeouts, region-derived hosts, no compression). Only fields explicitly set here replace
/// the Ingestion API defaults. Credentials are always taken from the parent <see cref="SearchClient"/>.
/// See https://www.algolia.com/doc/libraries/sdk/methods/ingestion
/// </summary>
public class TransformationOptions
{
  /// <summary>The ingestion region ("eu" or "us"). Required.</summary>
  public string Region { get; }

  /// <summary>Override the default ingestion hosts.</summary>
  public List<StatefulHost> CustomHosts { get; set; }

  /// <summary>Override the default connect timeout (25 s).</summary>
  public TimeSpan? ConnectTimeout { get; set; }

  /// <summary>Override the default read timeout (25 s).</summary>
  public TimeSpan? ReadTimeout { get; set; }

  /// <summary>Override the default write timeout (25 s).</summary>
  public TimeSpan? WriteTimeout { get; set; }

  /// <summary>Override the default compression (none).</summary>
  public CompressionType? Compression { get; set; }

  /// <summary>Override the default headers.</summary>
  public Dictionary<string, string> DefaultHeaders { get; set; }

  /// <param name="region">The ingestion region ("eu" or "us"). Required.</param>
  /// <exception cref="ArgumentException">Thrown when region is null or whitespace.</exception>
  public TransformationOptions(string region)
  {
    if (string.IsNullOrWhiteSpace(region))
    {
      throw new ArgumentException(
        "region is required in TransformationOptions. See https://www.algolia.com/doc/libraries/sdk/methods/ingestion"
      );
    }
    Region = region;
  }
}
