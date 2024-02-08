using System;

namespace Algolia.Search.Exceptions;

/// <summary>
/// Exception thrown when an host in unreachable
/// </summary>
public class AlgoliaUnreachableHostException : Exception
{
  /// <summary>
  /// Create a new AlgoliaUnreachableHostException.
  /// </summary>
  /// <param name="message">The exception details.</param>
  public AlgoliaUnreachableHostException(string message) : base(message)
  {
  }
}
