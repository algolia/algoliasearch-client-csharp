using Algolia.Search.Models.Search;

namespace Algolia.Search.Utils;

/// <summary>
/// A tool class to help model conversion
/// </summary>
public static class ModelConverters
{
  /// <summary>
  /// Convert a GetApiKeyResponse to an ApiKey
  /// </summary>
  /// <param name="apiKey"></param>
  /// <returns></returns>
  public static ApiKey ToApiKey(this GetApiKeyResponse apiKey)
  {
    return new ApiKey
    {
      Acl = apiKey.Acl,
      Description = apiKey.Description,
      Indexes = apiKey.Indexes,
      Referers = apiKey.Referers,
      Validity = apiKey.Validity,
      QueryParameters = apiKey.QueryParameters,
      MaxHitsPerQuery = apiKey.MaxHitsPerQuery,
      MaxQueriesPerIPPerHour = apiKey.MaxQueriesPerIPPerHour
    };
  }
}
