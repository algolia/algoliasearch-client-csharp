using System;

namespace Algolia.Search.Exceptions;

/// <summary>
/// Exception thrown when an error occurs in the Algolia client.
/// </summary>
public class AlgoliaException : Exception
{
  /// <summary>
  /// The Correlation-ID header of the response whose handling failed, when
  /// present. Quote it when contacting Algolia support.
  /// </summary>
  public string CorrelationId { get; internal set; }

  /// <summary>
  /// Create a new Algolia exception.
  /// </summary>
  /// <param name="message">The exception details.</param>
  public AlgoliaException(string message)
    : base(message) { }

  /// <summary>
  /// Create a new Algolia exception, with an inner exception.
  /// </summary>
  /// <param name="message"></param>
  /// <param name="inner"></param>
  public AlgoliaException(string message, Exception inner)
    : base(message, inner) { }

  /// <summary>
  /// The exception details, with the Correlation-ID appended when one is
  /// known. CorrelationId is set after construction, so the suffix cannot be
  /// baked in through the base constructor.
  /// </summary>
  public override string Message =>
    CorrelationId == null ? base.Message : $"{base.Message} (Correlation-ID: {CorrelationId})";
}
