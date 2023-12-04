//
// Code generated by OpenAPI Generator (https://openapi-generator.tech), manual changes will be lost - read more on https://github.com/algolia/api-clients-automation. DO NOT EDIT.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using FileParameter = Algolia.Search.Search.Client.FileParameter;
using OpenAPIDateConverter = Algolia.Search.Search.Client.OpenAPIDateConverter;
using System.Reflection;

namespace Algolia.Search.Search.Models
{
  /// <summary>
  /// SearchResult
  /// </summary>
  [JsonConverter(typeof(SearchResultJsonConverter))]
  [DataContract(Name = "searchResult")]
  public partial class SearchResult : AbstractOpenAPISchema, IEquatable<SearchResult>, IValidatableObject
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchResult" /> class
    /// with the <see cref="SearchResponse" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of SearchResponse.</param>
    public SearchResult(SearchResponse actualInstance)
    {
      this.IsNullable = false;
      this.SchemaType = "oneOf";
      this.ActualInstance = actualInstance ?? throw new ArgumentException("Invalid instance found. Must not be null.");
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchResult" /> class
    /// with the <see cref="SearchForFacetValuesResponse" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of SearchForFacetValuesResponse.</param>
    public SearchResult(SearchForFacetValuesResponse actualInstance)
    {
      this.IsNullable = false;
      this.SchemaType = "oneOf";
      this.ActualInstance = actualInstance ?? throw new ArgumentException("Invalid instance found. Must not be null.");
    }


    private Object _actualInstance;

    /// <summary>
    /// Gets or Sets ActualInstance
    /// </summary>
    public override Object ActualInstance
    {
      get
      {
        return _actualInstance;
      }
      set
      {
        if (value.GetType() == typeof(SearchForFacetValuesResponse))
        {
          this._actualInstance = value;
        }
        else if (value.GetType() == typeof(SearchResponse))
        {
          this._actualInstance = value;
        }
        else
        {
          throw new ArgumentException("Invalid instance found. Must be the following types: SearchForFacetValuesResponse, SearchResponse");
        }
      }
    }

    /// <summary>
    /// Get the actual instance of `SearchResponse`. If the actual instance is not `SearchResponse`,
    /// the InvalidClassException will be thrown
    /// </summary>
    /// <returns>An instance of SearchResponse</returns>
    public SearchResponse GetterSearchResponse()
    {
      return (SearchResponse)this.ActualInstance;
    }

    /// <summary>
    /// Get the actual instance of `SearchForFacetValuesResponse`. If the actual instance is not `SearchForFacetValuesResponse`,
    /// the InvalidClassException will be thrown
    /// </summary>
    /// <returns>An instance of SearchForFacetValuesResponse</returns>
    public SearchForFacetValuesResponse GetterSearchForFacetValuesResponse()
    {
      return (SearchForFacetValuesResponse)this.ActualInstance;
    }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
      var sb = new StringBuilder();
      sb.Append("class SearchResult {\n");
      sb.Append("  ActualInstance: ").Append(this.ActualInstance).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }

    /// <summary>
    /// Returns the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public override string ToJson()
    {
      return JsonConvert.SerializeObject(this.ActualInstance, SearchResult.SerializerSettings);
    }

    /// <summary>
    /// Converts the JSON string into an instance of SearchResult
    /// </summary>
    /// <param name="jsonString">JSON string</param>
    /// <returns>An instance of SearchResult</returns>
    public static SearchResult FromJson(string jsonString)
    {
      SearchResult newSearchResult = null;

      if (string.IsNullOrEmpty(jsonString))
      {
        return newSearchResult;
      }
      int match = 0;
      List<string> matchedTypes = new List<string>();

      try
      {
        // if it does not contains "AdditionalProperties", use SerializerSettings to deserialize
        if (typeof(SearchForFacetValuesResponse).GetProperty("AdditionalProperties") == null)
        {
          newSearchResult = new SearchResult(JsonConvert.DeserializeObject<SearchForFacetValuesResponse>(jsonString, SearchResult.SerializerSettings));
        }
        else
        {
          newSearchResult = new SearchResult(JsonConvert.DeserializeObject<SearchForFacetValuesResponse>(jsonString, SearchResult.AdditionalPropertiesSerializerSettings));
        }
        matchedTypes.Add("SearchForFacetValuesResponse");
        match++;
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine(string.Format("Failed to deserialize `{0}` into SearchForFacetValuesResponse: {1}", jsonString, exception.ToString()));
      }

      try
      {
        // if it does not contains "AdditionalProperties", use SerializerSettings to deserialize
        if (typeof(SearchResponse).GetProperty("AdditionalProperties") == null)
        {
          newSearchResult = new SearchResult(JsonConvert.DeserializeObject<SearchResponse>(jsonString, SearchResult.SerializerSettings));
        }
        else
        {
          newSearchResult = new SearchResult(JsonConvert.DeserializeObject<SearchResponse>(jsonString, SearchResult.AdditionalPropertiesSerializerSettings));
        }
        matchedTypes.Add("SearchResponse");
        match++;
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine(string.Format("Failed to deserialize `{0}` into SearchResponse: {1}", jsonString, exception.ToString()));
      }

      if (match == 0)
      {
        throw new InvalidDataException("The JSON string `" + jsonString + "` cannot be deserialized into any schema defined.");
      }
      else if (match > 1)
      {
        throw new InvalidDataException("The JSON string `" + jsonString + "` incorrectly matches more than one schema (should be exactly one match): " + String.Join(",", matchedTypes));
      }

      // deserialization is considered successful at this point if no exception has been thrown.
      return newSearchResult;
    }

    /// <summary>
    /// Returns true if objects are equal
    /// </summary>
    /// <param name="input">Object to be compared</param>
    /// <returns>Boolean</returns>
    public override bool Equals(object input)
    {
      return this.Equals(input as SearchResult);
    }

    /// <summary>
    /// Returns true if SearchResult instances are equal
    /// </summary>
    /// <param name="input">Instance of SearchResult to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(SearchResult input)
    {
      if (input == null)
        return false;

      return this.ActualInstance.Equals(input.ActualInstance);
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
        if (this.ActualInstance != null)
          hashCode = hashCode * 59 + this.ActualInstance.GetHashCode();
        return hashCode;
      }
    }

    /// <summary>
    /// To validate all properties of the instance
    /// </summary>
    /// <param name="validationContext">Validation context</param>
    /// <returns>Validation Result</returns>
    IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
    {
      yield break;
    }
  }

  /// <summary>
  /// Custom JSON converter for SearchResult
  /// </summary>
  public class SearchResultJsonConverter : JsonConverter
  {
    /// <summary>
    /// To write the JSON string
    /// </summary>
    /// <param name="writer">JSON writer</param>
    /// <param name="value">Object to be converted into a JSON string</param>
    /// <param name="serializer">JSON Serializer</param>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      writer.WriteRawValue((string)(typeof(SearchResult).GetMethod("ToJson").Invoke(value, null)));
    }

    /// <summary>
    /// To convert a JSON string into an object
    /// </summary>
    /// <param name="reader">JSON reader</param>
    /// <param name="objectType">Object type</param>
    /// <param name="existingValue">Existing value</param>
    /// <param name="serializer">JSON Serializer</param>
    /// <returns>The object converted from the JSON string</returns>
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      if (reader.TokenType != JsonToken.Null)
      {
        return SearchResult.FromJson(JObject.Load(reader).ToString(Formatting.None));
      }
      return null;
    }

    /// <summary>
    /// Check if the object can be converted
    /// </summary>
    /// <param name="objectType">Object type</param>
    /// <returns>True if the object can be converted</returns>
    public override bool CanConvert(Type objectType)
    {
      return false;
    }
  }

}
