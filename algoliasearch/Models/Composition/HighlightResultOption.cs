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
/// Surround words that match the query with HTML tags for highlighting.
/// </summary>
public partial class HighlightResultOption
{
  /// <summary>
  /// Gets or Sets MatchLevel
  /// </summary>
  [JsonPropertyName("matchLevel")]
  public MatchLevel? MatchLevel { get; set; }

  /// <summary>
  /// Initializes a new instance of the HighlightResultOption class.
  /// </summary>
  [JsonConstructor]
  public HighlightResultOption() { }

  /// <summary>
  /// Initializes a new instance of the HighlightResultOption class.
  /// </summary>
  /// <param name="value">Highlighted attribute value, including HTML tags. (required).</param>
  /// <param name="matchLevel">matchLevel (required).</param>
  /// <param name="matchedWords">List of matched words from the search query. (required).</param>
  public HighlightResultOption(string value, MatchLevel? matchLevel, List<string> matchedWords)
  {
    Value = value ?? throw new ArgumentNullException(nameof(value));
    MatchLevel = matchLevel;
    MatchedWords = matchedWords ?? throw new ArgumentNullException(nameof(matchedWords));
  }

  /// <summary>
  /// Highlighted attribute value, including HTML tags.
  /// </summary>
  /// <value>Highlighted attribute value, including HTML tags.</value>
  [JsonPropertyName("value")]
  public string Value { get; set; }

  /// <summary>
  /// List of matched words from the search query.
  /// </summary>
  /// <value>List of matched words from the search query.</value>
  [JsonPropertyName("matchedWords")]
  public List<string> MatchedWords { get; set; }

  /// <summary>
  /// Whether the entire attribute value is highlighted.
  /// </summary>
  /// <value>Whether the entire attribute value is highlighted.</value>
  [JsonPropertyName("fullyHighlighted")]
  public bool? FullyHighlighted { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class HighlightResultOption {\n");
    sb.Append("  Value: ").Append(Value).Append("\n");
    sb.Append("  MatchLevel: ").Append(MatchLevel).Append("\n");
    sb.Append("  MatchedWords: ").Append(MatchedWords).Append("\n");
    sb.Append("  FullyHighlighted: ").Append(FullyHighlighted).Append("\n");
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
    if (obj is not HighlightResultOption input)
    {
      return false;
    }

    return (Value == input.Value || (Value != null && Value.Equals(input.Value)))
      && (MatchLevel == input.MatchLevel || MatchLevel.Equals(input.MatchLevel))
      && (
        MatchedWords == input.MatchedWords
        || MatchedWords != null
          && input.MatchedWords != null
          && MatchedWords.SequenceEqual(input.MatchedWords)
      )
      && (
        FullyHighlighted == input.FullyHighlighted
        || FullyHighlighted.Equals(input.FullyHighlighted)
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
      if (Value != null)
      {
        hashCode = (hashCode * 59) + Value.GetHashCode();
      }
      hashCode = (hashCode * 59) + MatchLevel.GetHashCode();
      if (MatchedWords != null)
      {
        hashCode = (hashCode * 59) + MatchedWords.GetHashCode();
      }
      hashCode = (hashCode * 59) + FullyHighlighted.GetHashCode();
      return hashCode;
    }
  }
}
