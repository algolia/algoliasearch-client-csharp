using System;

namespace Algolia.Search.Models.Common
{
  /// <summary>
  ///  Abstract base class for oneOf, anyOf schemas in the OpenAPI specification
  /// </summary>
  public abstract partial class AbstractSchema
  {
    /// <summary>
    /// Gets or Sets the actual instance
    /// </summary>
    public abstract Object ActualInstance { get; set; }

    /// <summary>
    /// Gets or Sets IsNullable to indicate whether the instance is nullable
    /// </summary>
    public bool IsNullable { get; protected set; }

    /// <summary>
    /// Gets or Sets the schema type, which can be either `oneOf` or `anyOf`
    /// </summary>
    public string SchemaType { get; protected set; }

    /// <summary>
    /// Converts the instance into JSON string.
    /// </summary>
    public abstract string ToJson();
  }
}
