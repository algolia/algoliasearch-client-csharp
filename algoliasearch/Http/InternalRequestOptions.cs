using System;
using System.Collections.Generic;
using Algolia.Search.Utils;

namespace Algolia.Search.Http;

/// <summary>
/// Internal representation of RequestOptions
/// </summary>
internal class InternalRequestOptions
{
  public InternalRequestOptions(RequestOptions options = null)
  {
    QueryParameters = new Dictionary<string, string>();
    foreach (var t in options?.QueryParameters ?? new Dictionary<string, object>())
    {
      QueryParameters.Add(t.Key, QueryStringHelper.ParameterToString(t.Value));
    }

    HeaderParameters = new Dictionary<string, string>();
    foreach (var t in options?.Headers ?? new Dictionary<string, string>())
    {
      HeaderParameters.Add(t.Key.ToLowerInvariant(), QueryStringHelper.ParameterToString(t.Value));
    }

    CustomPathParameters = new Dictionary<string, string>();
    PathParameters = new Dictionary<string, string>();
    ConnectTimeout = options?.ConnectTimeout;
    ReadTimeout = options?.ReadTimeout;
    WriteTimeout = options?.WriteTimeout;
  }

  public void AddQueryParameter(string key, object value)
  {
    if (value == null)
      return;

    if (!QueryParameters.ContainsKey(key))
    {
      QueryParameters.Add(key, QueryStringHelper.ParameterToString(value));
    }
  }

  public void AddCustomQueryParameters(IDictionary<string, object> values)
  {
    if (values == null)
      return;

    foreach (var t in values)
    {
      AddQueryParameter(t.Key, t.Value);
    }
  }

  /// <summary>
  /// Parameters to be bound to path parts of the Request's URL path (For custom routes)
  /// </summary>
  public IDictionary<string, string> CustomPathParameters { get; set; }

  /// <summary>
  /// Parameters to be bound to path parts of the Request's URL
  /// </summary>
  public IDictionary<string, string> PathParameters { get; set; }

  /// <summary>
  /// Query parameters to be applied to the request.
  /// Keys may have 1 or more values associated.
  /// </summary>
  public IDictionary<string, string> QueryParameters { get; set; }

  /// <summary>
  /// Header parameters to be applied to the request.
  /// Keys may have 1 or more values associated.
  /// </summary>
  public IDictionary<string, string> HeaderParameters { get; set; }

  /// <summary>
  /// Any data associated with a request body.
  /// </summary>
  public object Data { get; set; }

  /// <summary>
  /// Request read timeout
  /// </summary>
  public TimeSpan? ReadTimeout { get; set; }

  /// <summary>
  /// Request write timeout
  /// </summary>
  public TimeSpan? WriteTimeout { get; set; }

  /// <summary>
  /// Request connect timeout
  /// </summary>
  public TimeSpan? ConnectTimeout { get; set; }

  /// <summary>
  /// Enforce the Read Transporter
  /// </summary>
  public bool? UseReadTransporter { get; set; }
}
