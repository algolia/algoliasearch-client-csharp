using System;
using System.Collections.Generic;

namespace Algolia.Search.Http;

/// <summary>
/// Request option you can add to your queries
/// </summary>
public class RequestOptionBuilder
{
  private readonly RequestOptions _options;

  /// <summary>
  /// Constructs a new instance of <see cref="RequestOptionBuilder"/>
  /// </summary>
  public RequestOptionBuilder()
  {
    _options = new RequestOptions
    {
      Headers = new Dictionary<string, string>(),
      QueryParameters = new Dictionary<string, object>()
    };
  }

  /// <summary>
  /// Add extra headers to the request
  /// </summary>
  /// <param name="key"></param>
  /// <param name="value"></param>
  /// <returns></returns>
  public RequestOptionBuilder AddExtraHeader(string key, string value)
  {
    _options.Headers.Add(key, value);
    return this;
  }

  /// <summary>
  /// Add extra query parameters to the request
  /// </summary>
  /// <param name="key"></param>
  /// <param name="value"></param>
  /// <returns></returns>
  public RequestOptionBuilder AddExtraQueryParameters(string key, object value)
  {
    _options.QueryParameters.Add(key, value);
    return this;
  }

  /// <summary>
  /// Set the request timeout
  /// </summary>
  /// <param name="timeout"></param>
  /// <returns></returns>
  public RequestOptionBuilder SetTimeout(TimeSpan timeout)
  {
    _options.Timeout = timeout;
    return this;
  }

  /// <summary>
  /// Build the request options
  /// </summary>
  /// <returns></returns>
  public RequestOptions Build()
  {
    return _options;
  }
}
