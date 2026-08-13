using System;

namespace Algolia.Search.Exceptions;

/// <summary>
/// Exception thrown when an host in unreachable
/// </summary>
public class AlgoliaUnreachableHostException : Exception
{
  /// <summary>
  /// The Correlation-ID header of the last retried attempt whose response
  /// carried one, or null when no attempt did. Quote it when contacting
  /// Algolia support.
  /// </summary>
  public string CorrelationId { get; }

  /// <summary>
  /// Create a new AlgoliaUnreachableHostException.
  /// </summary>
  /// <param name="message">The exception details.</param>
  public AlgoliaUnreachableHostException(string message)
    : this(message, null) { }

  /// <summary>
  /// Create a new AlgoliaUnreachableHostException carrying the Correlation-ID of the last retried attempt.
  /// </summary>
  /// <param name="message">The exception details.</param>
  /// <param name="correlationId">The Correlation-ID of the last retried attempt whose response carried one.</param>
  public AlgoliaUnreachableHostException(string message, string correlationId)
    : base(correlationId == null ? message : $"{message} (Correlation-ID: {correlationId})")
  {
    CorrelationId = correlationId;
  }
}
