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
  /// Ctor with error code and message
  /// </summary>
  /// <param name="message"></param>
  /// <param name="httpErrorCode"></param>
  public AlgoliaApiException(string message, int httpErrorCode)
    : base(message)
  {
    HttpErrorCode = httpErrorCode;
  }
}
