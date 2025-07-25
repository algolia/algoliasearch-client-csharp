//
// Code generated by OpenAPI Generator (https://openapi-generator.tech), manual changes will be lost - read more on https://github.com/algolia/api-clients-automation. DO NOT EDIT.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Algolia.Search.Serializer;

namespace Algolia.Search.Models.Composition;

/// <summary>
/// Params
/// </summary>
public partial class Params
{
  /// <summary>
  /// Initializes a new instance of the Params class.
  /// </summary>
  public Params() { }

  /// <summary>
  /// Search query.
  /// </summary>
  /// <value>Search query.</value>
  [JsonPropertyName("query")]
  public string Query { get; set; }

  /// <summary>
  /// Filter expression to only include items that match the filter criteria in the response.  You can use these filter expressions:  - **Numeric filters.** `<facet> <op> <number>`, where `<op>` is one of `<`, `<=`, `=`, `!=`, `>`, `>=`. - **Ranges.** `<facet>:<lower> TO <upper>` where `<lower>` and `<upper>` are the lower and upper limits of the range (inclusive). - **Facet filters.** `<facet>:<value>` where `<facet>` is a facet attribute (case-sensitive) and `<value>` a facet value. - **Tag filters.** `_tags:<value>` or just `<value>` (case-sensitive). - **Boolean filters.** `<facet>: true | false`.  You can combine filters with `AND`, `OR`, and `NOT` operators with the following restrictions:  - You can only combine filters of the same type with `OR`.   **Not supported:** `facet:value OR num > 3`. - You can't use `NOT` with combinations of filters.   **Not supported:** `NOT(facet:value OR facet:value)` - You can't combine conjunctions (`AND`) with `OR`.   **Not supported:** `facet:value OR (facet:value AND facet:value)`  Use quotes around your filters, if the facet attribute name or facet value has spaces, keywords (`OR`, `AND`, `NOT`), or quotes. If a facet attribute is an array, the filter matches if it matches at least one element of the array.  For more information, see [Filters](https://www.algolia.com/doc/guides/managing-results/refine-results/filtering/).
  /// </summary>
  /// <value>Filter expression to only include items that match the filter criteria in the response.  You can use these filter expressions:  - **Numeric filters.** `<facet> <op> <number>`, where `<op>` is one of `<`, `<=`, `=`, `!=`, `>`, `>=`. - **Ranges.** `<facet>:<lower> TO <upper>` where `<lower>` and `<upper>` are the lower and upper limits of the range (inclusive). - **Facet filters.** `<facet>:<value>` where `<facet>` is a facet attribute (case-sensitive) and `<value>` a facet value. - **Tag filters.** `_tags:<value>` or just `<value>` (case-sensitive). - **Boolean filters.** `<facet>: true | false`.  You can combine filters with `AND`, `OR`, and `NOT` operators with the following restrictions:  - You can only combine filters of the same type with `OR`.   **Not supported:** `facet:value OR num > 3`. - You can't use `NOT` with combinations of filters.   **Not supported:** `NOT(facet:value OR facet:value)` - You can't combine conjunctions (`AND`) with `OR`.   **Not supported:** `facet:value OR (facet:value AND facet:value)`  Use quotes around your filters, if the facet attribute name or facet value has spaces, keywords (`OR`, `AND`, `NOT`), or quotes. If a facet attribute is an array, the filter matches if it matches at least one element of the array.  For more information, see [Filters](https://www.algolia.com/doc/guides/managing-results/refine-results/filtering/). </value>
  [JsonPropertyName("filters")]
  public string Filters { get; set; }

  /// <summary>
  /// Page of search results to retrieve.
  /// </summary>
  /// <value>Page of search results to retrieve.</value>
  [JsonPropertyName("page")]
  public int? Page { get; set; }

  /// <summary>
  /// Whether the run response should include detailed ranking information.
  /// </summary>
  /// <value>Whether the run response should include detailed ranking information.</value>
  [JsonPropertyName("getRankingInfo")]
  public bool? GetRankingInfo { get; set; }

  /// <summary>
  /// Gets or Sets RelevancyStrictness
  /// </summary>
  [JsonPropertyName("relevancyStrictness")]
  public int? RelevancyStrictness { get; set; }

  /// <summary>
  /// Gets or Sets FacetFilters
  /// </summary>
  [JsonPropertyName("facetFilters")]
  public FacetFilters FacetFilters { get; set; }

  /// <summary>
  /// Gets or Sets OptionalFilters
  /// </summary>
  [JsonPropertyName("optionalFilters")]
  public OptionalFilters OptionalFilters { get; set; }

  /// <summary>
  /// Gets or Sets NumericFilters
  /// </summary>
  [JsonPropertyName("numericFilters")]
  public NumericFilters NumericFilters { get; set; }

  /// <summary>
  /// Number of hits per page.
  /// </summary>
  /// <value>Number of hits per page.</value>
  [JsonPropertyName("hitsPerPage")]
  public int? HitsPerPage { get; set; }

  /// <summary>
  /// Coordinates for the center of a circle, expressed as a comma-separated string of latitude and longitude.  Only records included within a circle around this central location are included in the results. The radius of the circle is determined by the `aroundRadius` and `minimumAroundRadius` settings. This parameter is ignored if you also specify `insidePolygon` or `insideBoundingBox`.
  /// </summary>
  /// <value>Coordinates for the center of a circle, expressed as a comma-separated string of latitude and longitude.  Only records included within a circle around this central location are included in the results. The radius of the circle is determined by the `aroundRadius` and `minimumAroundRadius` settings. This parameter is ignored if you also specify `insidePolygon` or `insideBoundingBox`. </value>
  [JsonPropertyName("aroundLatLng")]
  public string AroundLatLng { get; set; }

  /// <summary>
  /// Whether to obtain the coordinates from the request's IP address.
  /// </summary>
  /// <value>Whether to obtain the coordinates from the request's IP address.</value>
  [JsonPropertyName("aroundLatLngViaIP")]
  public bool? AroundLatLngViaIP { get; set; }

  /// <summary>
  /// Gets or Sets AroundRadius
  /// </summary>
  [JsonPropertyName("aroundRadius")]
  public AroundRadius AroundRadius { get; set; }

  /// <summary>
  /// Gets or Sets AroundPrecision
  /// </summary>
  [JsonPropertyName("aroundPrecision")]
  public AroundPrecision AroundPrecision { get; set; }

  /// <summary>
  /// Minimum radius (in meters) for a search around a location when `aroundRadius` isn't set.
  /// </summary>
  /// <value>Minimum radius (in meters) for a search around a location when `aroundRadius` isn't set.</value>
  [JsonPropertyName("minimumAroundRadius")]
  public int? MinimumAroundRadius { get; set; }

  /// <summary>
  /// Gets or Sets InsideBoundingBox
  /// </summary>
  [JsonPropertyName("insideBoundingBox")]
  public InsideBoundingBox InsideBoundingBox { get; set; }

  /// <summary>
  /// Coordinates of a polygon in which to search.  Polygons are defined by 3 to 10,000 points. Each point is represented by its latitude and longitude. Provide multiple polygons as nested arrays. For more information, see [filtering inside polygons](https://www.algolia.com/doc/guides/managing-results/refine-results/geolocation/#filtering-inside-rectangular-or-polygonal-areas). This parameter is ignored if you also specify `insideBoundingBox`.
  /// </summary>
  /// <value>Coordinates of a polygon in which to search.  Polygons are defined by 3 to 10,000 points. Each point is represented by its latitude and longitude. Provide multiple polygons as nested arrays. For more information, see [filtering inside polygons](https://www.algolia.com/doc/guides/managing-results/refine-results/geolocation/#filtering-inside-rectangular-or-polygonal-areas). This parameter is ignored if you also specify `insideBoundingBox`. </value>
  [JsonPropertyName("insidePolygon")]
  public List<List<double>> InsidePolygon { get; set; }

  /// <summary>
  /// Languages for language-specific query processing steps such as plurals, stop-word removal, and word-detection dictionaries This setting sets a default list of languages used by the `removeStopWords` and `ignorePlurals` settings. This setting also sets a dictionary for word detection in the logogram-based [CJK](https://www.algolia.com/doc/guides/managing-results/optimize-search-results/handling-natural-languages-nlp/in-depth/normalization/#normalization-for-logogram-based-languages-cjk) languages. To support this, you must place the CJK language **first** **You should always specify a query language.** If you don't specify an indexing language, the search engine uses all [supported languages](https://www.algolia.com/doc/guides/managing-results/optimize-search-results/handling-natural-languages-nlp/in-depth/supported-languages/), or the languages you specified with the `ignorePlurals` or `removeStopWords` parameters. This can lead to unexpected search results. For more information, see [Language-specific configuration](https://www.algolia.com/doc/guides/managing-results/optimize-search-results/handling-natural-languages-nlp/in-depth/language-specific-configurations/).
  /// </summary>
  /// <value>Languages for language-specific query processing steps such as plurals, stop-word removal, and word-detection dictionaries This setting sets a default list of languages used by the `removeStopWords` and `ignorePlurals` settings. This setting also sets a dictionary for word detection in the logogram-based [CJK](https://www.algolia.com/doc/guides/managing-results/optimize-search-results/handling-natural-languages-nlp/in-depth/normalization/#normalization-for-logogram-based-languages-cjk) languages. To support this, you must place the CJK language **first** **You should always specify a query language.** If you don't specify an indexing language, the search engine uses all [supported languages](https://www.algolia.com/doc/guides/managing-results/optimize-search-results/handling-natural-languages-nlp/in-depth/supported-languages/), or the languages you specified with the `ignorePlurals` or `removeStopWords` parameters. This can lead to unexpected search results. For more information, see [Language-specific configuration](https://www.algolia.com/doc/guides/managing-results/optimize-search-results/handling-natural-languages-nlp/in-depth/language-specific-configurations/). </value>
  [JsonPropertyName("queryLanguages")]
  public List<SupportedLanguage> QueryLanguages { get; set; }

  /// <summary>
  /// ISO language codes that adjust settings that are useful for processing natural language queries (as opposed to keyword searches) - Sets `removeStopWords` and `ignorePlurals` to the list of provided languages. - Sets `removeWordsIfNoResults` to `allOptional`. - Adds a `natural_language` attribute to `ruleContexts` and `analyticsTags`.
  /// </summary>
  /// <value>ISO language codes that adjust settings that are useful for processing natural language queries (as opposed to keyword searches) - Sets `removeStopWords` and `ignorePlurals` to the list of provided languages. - Sets `removeWordsIfNoResults` to `allOptional`. - Adds a `natural_language` attribute to `ruleContexts` and `analyticsTags`. </value>
  [JsonPropertyName("naturalLanguages")]
  public List<SupportedLanguage> NaturalLanguages { get; set; }

  /// <summary>
  /// Whether to enable composition rules.
  /// </summary>
  /// <value>Whether to enable composition rules.</value>
  [JsonPropertyName("enableRules")]
  public bool? EnableRules { get; set; }

  /// <summary>
  /// Assigns a rule context to the run query [Rule contexts](https://www.algolia.com/doc/guides/managing-results/rules/rules-overview/how-to/customize-search-results-by-platform/#whats-a-context) are strings that you can use to trigger matching rules.
  /// </summary>
  /// <value>Assigns a rule context to the run query [Rule contexts](https://www.algolia.com/doc/guides/managing-results/rules/rules-overview/how-to/customize-search-results-by-platform/#whats-a-context) are strings that you can use to trigger matching rules. </value>
  [JsonPropertyName("ruleContexts")]
  public List<string> RuleContexts { get; set; }

  /// <summary>
  /// Unique pseudonymous or anonymous user identifier.  This helps with analytics and click and conversion events. For more information, see [user token](https://www.algolia.com/doc/guides/sending-events/concepts/usertoken/).
  /// </summary>
  /// <value>Unique pseudonymous or anonymous user identifier.  This helps with analytics and click and conversion events. For more information, see [user token](https://www.algolia.com/doc/guides/sending-events/concepts/usertoken/). </value>
  [JsonPropertyName("userToken")]
  public string UserToken { get; set; }

  /// <summary>
  /// Whether to include a `queryID` attribute in the response The query ID is a unique identifier for a search query and is required for tracking [click and conversion events](https://www.algolia.com/guides/sending-events/getting-started/).
  /// </summary>
  /// <value>Whether to include a `queryID` attribute in the response The query ID is a unique identifier for a search query and is required for tracking [click and conversion events](https://www.algolia.com/guides/sending-events/getting-started/). </value>
  [JsonPropertyName("clickAnalytics")]
  public bool? ClickAnalytics { get; set; }

  /// <summary>
  /// Whether this search will be included in Analytics.
  /// </summary>
  /// <value>Whether this search will be included in Analytics.</value>
  [JsonPropertyName("analytics")]
  public bool? Analytics { get; set; }

  /// <summary>
  /// Tags to apply to the query for [segmenting analytics data](https://www.algolia.com/doc/guides/search-analytics/guides/segments/).
  /// </summary>
  /// <value>Tags to apply to the query for [segmenting analytics data](https://www.algolia.com/doc/guides/search-analytics/guides/segments/).</value>
  [JsonPropertyName("analyticsTags")]
  public List<string> AnalyticsTags { get; set; }

  /// <summary>
  /// Whether to enable index level A/B testing for this run request. If the composition mixes multiple indices, the A/B test is ignored.
  /// </summary>
  /// <value>Whether to enable index level A/B testing for this run request. If the composition mixes multiple indices, the A/B test is ignored. </value>
  [JsonPropertyName("enableABTest")]
  public bool? EnableABTest { get; set; }

  /// <summary>
  /// Whether this search will use [Dynamic Re-Ranking](https://www.algolia.com/doc/guides/algolia-ai/re-ranking/) This setting only has an effect if you activated Dynamic Re-Ranking for this index in the Algolia dashboard.
  /// </summary>
  /// <value>Whether this search will use [Dynamic Re-Ranking](https://www.algolia.com/doc/guides/algolia-ai/re-ranking/) This setting only has an effect if you activated Dynamic Re-Ranking for this index in the Algolia dashboard. </value>
  [JsonPropertyName("enableReRanking")]
  public bool? EnableReRanking { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class Params {\n");
    sb.Append("  Query: ").Append(Query).Append("\n");
    sb.Append("  Filters: ").Append(Filters).Append("\n");
    sb.Append("  Page: ").Append(Page).Append("\n");
    sb.Append("  GetRankingInfo: ").Append(GetRankingInfo).Append("\n");
    sb.Append("  RelevancyStrictness: ").Append(RelevancyStrictness).Append("\n");
    sb.Append("  FacetFilters: ").Append(FacetFilters).Append("\n");
    sb.Append("  OptionalFilters: ").Append(OptionalFilters).Append("\n");
    sb.Append("  NumericFilters: ").Append(NumericFilters).Append("\n");
    sb.Append("  HitsPerPage: ").Append(HitsPerPage).Append("\n");
    sb.Append("  AroundLatLng: ").Append(AroundLatLng).Append("\n");
    sb.Append("  AroundLatLngViaIP: ").Append(AroundLatLngViaIP).Append("\n");
    sb.Append("  AroundRadius: ").Append(AroundRadius).Append("\n");
    sb.Append("  AroundPrecision: ").Append(AroundPrecision).Append("\n");
    sb.Append("  MinimumAroundRadius: ").Append(MinimumAroundRadius).Append("\n");
    sb.Append("  InsideBoundingBox: ").Append(InsideBoundingBox).Append("\n");
    sb.Append("  InsidePolygon: ").Append(InsidePolygon).Append("\n");
    sb.Append("  QueryLanguages: ").Append(QueryLanguages).Append("\n");
    sb.Append("  NaturalLanguages: ").Append(NaturalLanguages).Append("\n");
    sb.Append("  EnableRules: ").Append(EnableRules).Append("\n");
    sb.Append("  RuleContexts: ").Append(RuleContexts).Append("\n");
    sb.Append("  UserToken: ").Append(UserToken).Append("\n");
    sb.Append("  ClickAnalytics: ").Append(ClickAnalytics).Append("\n");
    sb.Append("  Analytics: ").Append(Analytics).Append("\n");
    sb.Append("  AnalyticsTags: ").Append(AnalyticsTags).Append("\n");
    sb.Append("  EnableABTest: ").Append(EnableABTest).Append("\n");
    sb.Append("  EnableReRanking: ").Append(EnableReRanking).Append("\n");
    sb.Append("}\n");
    return sb.ToString();
  }

  /// <summary>
  /// Returns the JSON string presentation of the object
  /// </summary>
  /// <returns>JSON string presentation of the object</returns>
  public virtual string ToJson()
  {
    return JsonSerializer.Serialize(this, JsonConfig.Options);
  }

  /// <summary>
  /// Returns true if objects are equal
  /// </summary>
  /// <param name="obj">Object to be compared</param>
  /// <returns>Boolean</returns>
  public override bool Equals(object obj)
  {
    if (obj is not Params input)
    {
      return false;
    }

    return (Query == input.Query || (Query != null && Query.Equals(input.Query)))
      && (Filters == input.Filters || (Filters != null && Filters.Equals(input.Filters)))
      && (Page == input.Page || Page.Equals(input.Page))
      && (GetRankingInfo == input.GetRankingInfo || GetRankingInfo.Equals(input.GetRankingInfo))
      && (
        RelevancyStrictness == input.RelevancyStrictness
        || RelevancyStrictness.Equals(input.RelevancyStrictness)
      )
      && (
        FacetFilters == input.FacetFilters
        || (FacetFilters != null && FacetFilters.Equals(input.FacetFilters))
      )
      && (
        OptionalFilters == input.OptionalFilters
        || (OptionalFilters != null && OptionalFilters.Equals(input.OptionalFilters))
      )
      && (
        NumericFilters == input.NumericFilters
        || (NumericFilters != null && NumericFilters.Equals(input.NumericFilters))
      )
      && (HitsPerPage == input.HitsPerPage || HitsPerPage.Equals(input.HitsPerPage))
      && (
        AroundLatLng == input.AroundLatLng
        || (AroundLatLng != null && AroundLatLng.Equals(input.AroundLatLng))
      )
      && (
        AroundLatLngViaIP == input.AroundLatLngViaIP
        || AroundLatLngViaIP.Equals(input.AroundLatLngViaIP)
      )
      && (
        AroundRadius == input.AroundRadius
        || (AroundRadius != null && AroundRadius.Equals(input.AroundRadius))
      )
      && (
        AroundPrecision == input.AroundPrecision
        || (AroundPrecision != null && AroundPrecision.Equals(input.AroundPrecision))
      )
      && (
        MinimumAroundRadius == input.MinimumAroundRadius
        || MinimumAroundRadius.Equals(input.MinimumAroundRadius)
      )
      && (
        InsideBoundingBox == input.InsideBoundingBox
        || (InsideBoundingBox != null && InsideBoundingBox.Equals(input.InsideBoundingBox))
      )
      && (
        InsidePolygon == input.InsidePolygon
        || InsidePolygon != null
          && input.InsidePolygon != null
          && InsidePolygon.SequenceEqual(input.InsidePolygon)
      )
      && (
        QueryLanguages == input.QueryLanguages
        || QueryLanguages != null
          && input.QueryLanguages != null
          && QueryLanguages.SequenceEqual(input.QueryLanguages)
      )
      && (
        NaturalLanguages == input.NaturalLanguages
        || NaturalLanguages != null
          && input.NaturalLanguages != null
          && NaturalLanguages.SequenceEqual(input.NaturalLanguages)
      )
      && (EnableRules == input.EnableRules || EnableRules.Equals(input.EnableRules))
      && (
        RuleContexts == input.RuleContexts
        || RuleContexts != null
          && input.RuleContexts != null
          && RuleContexts.SequenceEqual(input.RuleContexts)
      )
      && (UserToken == input.UserToken || (UserToken != null && UserToken.Equals(input.UserToken)))
      && (ClickAnalytics == input.ClickAnalytics || ClickAnalytics.Equals(input.ClickAnalytics))
      && (Analytics == input.Analytics || Analytics.Equals(input.Analytics))
      && (
        AnalyticsTags == input.AnalyticsTags
        || AnalyticsTags != null
          && input.AnalyticsTags != null
          && AnalyticsTags.SequenceEqual(input.AnalyticsTags)
      )
      && (EnableABTest == input.EnableABTest || EnableABTest.Equals(input.EnableABTest))
      && (
        EnableReRanking == input.EnableReRanking || EnableReRanking.Equals(input.EnableReRanking)
      );
  }

  /// <summary>
  /// Gets the hash code
  /// </summary>
  /// <returns>Hash code</returns>
  public override int GetHashCode()
  {
    unchecked // Overflow is fine, just wrap
    {
      int hashCode = 41;
      if (Query != null)
      {
        hashCode = (hashCode * 59) + Query.GetHashCode();
      }
      if (Filters != null)
      {
        hashCode = (hashCode * 59) + Filters.GetHashCode();
      }
      hashCode = (hashCode * 59) + Page.GetHashCode();
      hashCode = (hashCode * 59) + GetRankingInfo.GetHashCode();
      hashCode = (hashCode * 59) + RelevancyStrictness.GetHashCode();
      if (FacetFilters != null)
      {
        hashCode = (hashCode * 59) + FacetFilters.GetHashCode();
      }
      if (OptionalFilters != null)
      {
        hashCode = (hashCode * 59) + OptionalFilters.GetHashCode();
      }
      if (NumericFilters != null)
      {
        hashCode = (hashCode * 59) + NumericFilters.GetHashCode();
      }
      hashCode = (hashCode * 59) + HitsPerPage.GetHashCode();
      if (AroundLatLng != null)
      {
        hashCode = (hashCode * 59) + AroundLatLng.GetHashCode();
      }
      hashCode = (hashCode * 59) + AroundLatLngViaIP.GetHashCode();
      if (AroundRadius != null)
      {
        hashCode = (hashCode * 59) + AroundRadius.GetHashCode();
      }
      if (AroundPrecision != null)
      {
        hashCode = (hashCode * 59) + AroundPrecision.GetHashCode();
      }
      hashCode = (hashCode * 59) + MinimumAroundRadius.GetHashCode();
      if (InsideBoundingBox != null)
      {
        hashCode = (hashCode * 59) + InsideBoundingBox.GetHashCode();
      }
      if (InsidePolygon != null)
      {
        hashCode = (hashCode * 59) + InsidePolygon.GetHashCode();
      }
      if (QueryLanguages != null)
      {
        hashCode = (hashCode * 59) + QueryLanguages.GetHashCode();
      }
      if (NaturalLanguages != null)
      {
        hashCode = (hashCode * 59) + NaturalLanguages.GetHashCode();
      }
      hashCode = (hashCode * 59) + EnableRules.GetHashCode();
      if (RuleContexts != null)
      {
        hashCode = (hashCode * 59) + RuleContexts.GetHashCode();
      }
      if (UserToken != null)
      {
        hashCode = (hashCode * 59) + UserToken.GetHashCode();
      }
      hashCode = (hashCode * 59) + ClickAnalytics.GetHashCode();
      hashCode = (hashCode * 59) + Analytics.GetHashCode();
      if (AnalyticsTags != null)
      {
        hashCode = (hashCode * 59) + AnalyticsTags.GetHashCode();
      }
      hashCode = (hashCode * 59) + EnableABTest.GetHashCode();
      hashCode = (hashCode * 59) + EnableReRanking.GetHashCode();
      return hashCode;
    }
  }
}
