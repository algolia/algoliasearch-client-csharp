using System;

namespace Algolia.Search.Exceptions
{
  /// <summary>
  /// Exception thrown when an host in unreachable
  /// </summary>
  public class AlgoliaUnreachableHostException : Exception
  {
    /// <summary>
    /// Algolia's Execption
    /// </summary>
    public AlgoliaUnreachableHostException()
    {
    }

    /// <summary>
    /// Create a new AlgoliaUnreachableHostException.
    /// </summary>
    /// <param name="message">The exception details.</param>
    public AlgoliaUnreachableHostException(string message) : base(message)
    {
    }

    /// <summary>
    /// AlgoliaUnreachableHostException
    /// </summary>
    /// <param name="message"></param>
    /// <param name="inner"></param>
    public AlgoliaUnreachableHostException(string message, Exception inner)
        : base(message, inner)
    {
    }
  }
}
