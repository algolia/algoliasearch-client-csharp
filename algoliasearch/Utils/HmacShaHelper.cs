using System;
using System.Security.Cryptography;
using System.Text;

namespace Algolia.Search.Utils;

/// <summary>
/// Helper to compute HMAC SHA 256
/// </summary>
internal static class HmacShaHelper
{
  private static readonly ASCIIEncoding Encoding = new();

  internal static string GetHash(string key, string text)
  {
    var keyBytes = Encoding.GetBytes(key);
    var textBytes = Encoding.GetBytes(text);

    using var hmac = new HMACSHA256(keyBytes);
    var hmBytes = hmac.ComputeHash(textBytes);
    return ToHexString(hmBytes);
  }

  private static string ToHexString(byte[] array)
  {
    var hex = new StringBuilder(array.Length * 2);
    foreach (var b in array)
    {
      hex.AppendFormat("{0:x2}", b);
    }

    return hex.ToString();
  }

  internal static string Base64Encode(string plainText)
  {
    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
    return Convert.ToBase64String(plainTextBytes);
  }
}
