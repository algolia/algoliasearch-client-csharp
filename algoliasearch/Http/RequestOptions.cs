using System;
using System.Collections.Generic;

namespace Algolia.Search.Http
{
  /// <summary>
  /// Request option you can add to your queries
  /// </summary>
  public class RequestOptions
  {
    /// <summary>
    /// Custom headers will override default headers if they exist
    /// </summary>
    public IDictionary<string, string> Headers { get; set; }

    /// <summary>
    /// Add custom queries parameters
    /// </summary>
    public IDictionary<string, Object> QueryParameters { get; set; }

    /// <summary>
    /// Request timeout in seconds
    /// </summary>
    public TimeSpan? Timeout { get; set; }

    /// <summary>
    /// Constructs a new instance of <see cref="RequestOptions"/>
    /// </summary>
    public RequestOptions()
    {
      QueryParameters = new Dictionary<string, Object>();
      Headers = new Dictionary<string, string>();
    }
  }

  /// <summary>
  /// A container for generalized request inputs. This type allows consumers to extend the request functionality
  /// by abstracting away from the default (built-in) request framework (e.g. RestSharp).
  /// </summary>
  internal class InternalRequestOptions
  {
    public InternalRequestOptions(RequestOptions options = null)
    {
      QueryParameters = new Dictionary<string, string>();
      foreach (var t in options?.QueryParameters ?? new Dictionary<string, Object>())
      {
        QueryParameters.Add(t.Key, ClientUtils.ParameterToString(t.Value));
      }

      HeaderParameters = new Dictionary<string, string>();
      foreach (var t in options?.Headers ?? new Dictionary<string, string>())
      {
        HeaderParameters.Add(t.Key.ToLowerInvariant(), ClientUtils.ParameterToString(t.Value));
      }

      CustomPathParameters = new Dictionary<string, string>();
      PathParameters = new Dictionary<string, string>();
      Timeout = options?.Timeout;
    }

    public void AddQueryParameter(string key, object value)
    {
      if (value == null) return;

      if (!QueryParameters.ContainsKey(key))
      {
        QueryParameters.Add(key, ClientUtils.ParameterToString(value));
      }
    }

    public void AddCustomQueryParameters(IDictionary<string, object> values)
    {
      if (values == null) return;

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
    public Object Data { get; set; }

    /// <summary>
    /// Request timeout
    /// </summary>
    public TimeSpan? Timeout { get; set; }

    /// <summary>
    /// Enforce the Read Transporter
    /// </summary>
    public bool? UseReadTransporter { get; set; }
  }
}
