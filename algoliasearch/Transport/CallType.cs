using System;

namespace Algolia.Search.Transport;

/// <summary>
/// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/enumeration-types
/// Binary enums beware when adding new values
/// </summary>
[Flags]
public enum CallType
{
  /// <summary>
  /// Read Call
  /// </summary>
  Read = 1,

  /// <summary>
  /// Write Call
  /// </summary>
  Write = 2,
}
