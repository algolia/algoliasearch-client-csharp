using System;

namespace Algolia.Search.Exceptions
{
  /// <summary>
  /// Algolia exception.
  /// </summary>
  public class AlgoliaException : Exception
  {
    /// <summary>
    /// Algolia's Execption
    /// </summary>
    public AlgoliaException()
    {
    }

    /// <summary>
    /// Create a new Algolia exception.
    /// </summary>
    /// <param name="message">The exception details.</param>
    public AlgoliaException(string message) : base(message)
    {
    }

    /// <summary>
    /// Algolia Exception
    /// </summary>
    /// <param name="message"></param>
    /// <param name="inner"></param>
    public AlgoliaException(string message, Exception inner)
        : base(message, inner)
    {
    }
  }
}
