namespace Algolia.Search.Models.Common;

/// <summary>
///  Abstract base class for oneOf, anyOf schemas in the OpenAPI specification
/// </summary>
public abstract class AbstractSchema
{
  /// <summary>
  /// Gets or Sets the actual instance
  /// </summary>
  public abstract object ActualInstance { get; set; }

  /// <summary>
  /// Converts the instance into JSON string.
  /// </summary>
  public abstract string ToJson();
}
