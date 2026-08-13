using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Algolia.Search.Utils;

/// <summary>
/// Mints the Request-ID tracing header sent by the clients that support it.
/// </summary>
internal static class RequestIdHelper
{
  private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

  private const int Length = 11;

  // Shared across requests; GetBytes is thread-safe.
  private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

  /// <summary>
  /// Returns a fresh 11-character base62 identifier for the Request-ID header.
  /// </summary>
  internal static string Generate()
  {
    var bytes = new byte[Length];
    Rng.GetBytes(bytes);

    var id = new char[Length];
    for (var i = 0; i < Length; i++)
    {
      id[i] = Alphabet[bytes[i] % Alphabet.Length];
    }

    return new string(id);
  }

  private const string QueryParameter = "x-algolia-request-id";

  /// <summary>
  /// Whether the query parameters already carry an x-algolia-request-id entry, whatever its casing.
  /// </summary>
  internal static bool HasRequestIdQueryParameter<TValue>(
    IDictionary<string, TValue> queryParameters
  )
  {
    if (queryParameters == null)
    {
      return false;
    }

    foreach (var parameter in queryParameters)
    {
      if (parameter.Key.Equals(QueryParameter, StringComparison.OrdinalIgnoreCase))
      {
        return true;
      }
    }

    return false;
  }

  /// <summary>
  /// Whether the headers already carry a Request-ID entry, whatever its casing.
  /// </summary>
  internal static bool HasRequestId(IDictionary<string, string> headers)
  {
    if (headers == null)
    {
      return false;
    }

    foreach (var header in headers)
    {
      if (header.Key.Equals(Defaults.RequestIdHeader, StringComparison.OrdinalIgnoreCase))
      {
        return true;
      }
    }

    return false;
  }
}
