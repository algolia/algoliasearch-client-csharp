using System;

namespace Algolia.Search.Exceptions
{
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
    /// Algolia's Execption
    /// </summary>
    public AlgoliaApiException()
    {
    }

    /// <summary>
    /// Create a new Algolia's api exception.
    /// </summary>
    /// <param name="message">The exception details.</param>
    public AlgoliaApiException(string message) : base(message)
    {
    }

    /// <summary>
    /// Algolia API Exception
    /// </summary>
    /// <param name="message"></param>
    /// <param name="inner"></param>
    public AlgoliaApiException(string message, Exception inner)
        : base(message, inner)
    {
    }

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
}
