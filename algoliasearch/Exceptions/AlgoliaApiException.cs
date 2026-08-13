using System;

namespace Algolia.Search.Exceptions;

/// <summary>
/// Exception sent by Algolia's API
/// </summary>
public class AlgoliaApiException : Exception
{
  /// <summary>
  /// Http error code
  /// </summary>
  public int HttpErrorCode { get; set; }

  /// <summary>
  /// The Correlation-ID header of the failed response, when present.
  /// Quote it when contacting Algolia support.
  /// </summary>
  public string CorrelationId { get; }

  /// <summary>
  /// The response body as received, without the Correlation-ID suffix that
  /// Message carries, for callers that parse the error payload.
  /// </summary>
  public string ResponseBody { get; }

  /// <summary>
  /// Create a new AlgoliaAPIException
  /// </summary>
  /// <param name="message">The raw response body of the failed request.</param>
  /// <param name="httpErrorCode">The HTTP status code of the failed response.</param>
  public AlgoliaApiException(string message, int httpErrorCode)
    : this(message, httpErrorCode, null) { }

  /// <summary>
  /// Create a new AlgoliaAPIException carrying the Correlation-ID of the failed response
  /// </summary>
  /// <param name="message">The raw response body of the failed request; Message carries it with the Correlation-ID appended.</param>
  /// <param name="httpErrorCode">The HTTP status code of the failed response.</param>
  /// <param name="correlationId">The Correlation-ID header of the failed response, or null when absent.</param>
  public AlgoliaApiException(string message, int httpErrorCode, string correlationId)
    : base(correlationId == null ? message : $"{message} (Correlation-ID: {correlationId})")
  {
    HttpErrorCode = httpErrorCode;
    CorrelationId = correlationId;
    ResponseBody = message;
  }
}
