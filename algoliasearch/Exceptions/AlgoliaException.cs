using System;

namespace Algolia.Search.Exceptions;

/// <summary>
/// Exception thrown when an error occurs in the Algolia client.
/// </summary>
public class AlgoliaException : Exception
{
  /// <summary>
  /// Create a new Algolia exception.
  /// </summary>
  /// <param name="message">The exception details.</param>
  public AlgoliaException(string message) : base(message)
  {
  }

  /// <summary>
  /// Create a new Algolia exception, with an inner exception.
  /// </summary>
  /// <param name="message"></param>
  /// <param name="inner"></param>
  public AlgoliaException(string message, Exception inner)
    : base(message, inner)
  {
  }
}
