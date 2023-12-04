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
using FileParameter = Algolia.Search.Insights.Client.FileParameter;
using OpenAPIDateConverter = Algolia.Search.Insights.Client.OpenAPIDateConverter;

namespace Algolia.Search.Insights.Models
{
  /// <summary>
  /// Use this event to track when users convert on items unrelated to a previous Algolia request. For example, if you don&#39;t use Algolia to build your category pages, use this event.  To track conversion events related to Algolia requests, use the \&quot;Converted object IDs after search\&quot; event. 
  /// </summary>
  [DataContract(Name = "ConvertedObjectIDs")]
  public partial class ConvertedObjectIDs : IEquatable<ConvertedObjectIDs>, IValidatableObject
  {

    /// <summary>
    /// Gets or Sets EventType
    /// </summary>
    [DataMember(Name = "eventType", IsRequired = true, EmitDefaultValue = true)]
    public ConversionEvent EventType { get; set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="ConvertedObjectIDs" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected ConvertedObjectIDs() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="ConvertedObjectIDs" /> class.
    /// </summary>
    /// <param name="eventName">Can contain up to 64 ASCII characters.   Consider naming events consistently—for example, by adopting Segment&#39;s [object-action](https://segment.com/academy/collecting-data/naming-conventions-for-clean-data/#the-object-action-framework) framework.  (required).</param>
    /// <param name="eventType">eventType (required).</param>
    /// <param name="index">Name of the Algolia index. (required).</param>
    /// <param name="objectIDs">List of object identifiers for items of an Algolia index. (required).</param>
    /// <param name="userToken">Anonymous or pseudonymous user identifier.   &gt; **Note**: Never include personally identifiable information in user tokens.  (required).</param>
    /// <param name="timestamp">Time of the event in milliseconds in [Unix epoch time](https://wikipedia.org/wiki/Unix_time). By default, the Insights API uses the time it receives an event as its timestamp. .</param>
    /// <param name="authenticatedUserToken">User token for authenticated users..</param>
    public ConvertedObjectIDs(string eventName = default(string), ConversionEvent eventType = default(ConversionEvent), string index = default(string), List<string> objectIDs = default(List<string>), string userToken = default(string), long timestamp = default(long), string authenticatedUserToken = default(string))
    {
      // to ensure "eventName" is required (not null)
      if (eventName == null)
      {
        throw new ArgumentNullException("eventName is a required property for ConvertedObjectIDs and cannot be null");
      }
      this.EventName = eventName;
      this.EventType = eventType;
      // to ensure "index" is required (not null)
      if (index == null)
      {
        throw new ArgumentNullException("index is a required property for ConvertedObjectIDs and cannot be null");
      }
      this.Index = index;
      // to ensure "objectIDs" is required (not null)
      if (objectIDs == null)
      {
        throw new ArgumentNullException("objectIDs is a required property for ConvertedObjectIDs and cannot be null");
      }
      this.ObjectIDs = objectIDs;
      // to ensure "userToken" is required (not null)
      if (userToken == null)
      {
        throw new ArgumentNullException("userToken is a required property for ConvertedObjectIDs and cannot be null");
      }
      this.UserToken = userToken;
      this.Timestamp = timestamp;
      this.AuthenticatedUserToken = authenticatedUserToken;
    }

    /// <summary>
    /// Can contain up to 64 ASCII characters.   Consider naming events consistently—for example, by adopting Segment&#39;s [object-action](https://segment.com/academy/collecting-data/naming-conventions-for-clean-data/#the-object-action-framework) framework. 
    /// </summary>
    /// <value>Can contain up to 64 ASCII characters.   Consider naming events consistently—for example, by adopting Segment&#39;s [object-action](https://segment.com/academy/collecting-data/naming-conventions-for-clean-data/#the-object-action-framework) framework. </value>
    [DataMember(Name = "eventName", IsRequired = true, EmitDefaultValue = true)]
    public string EventName { get; set; }

    /// <summary>
    /// Name of the Algolia index.
    /// </summary>
    /// <value>Name of the Algolia index.</value>
    [DataMember(Name = "index", IsRequired = true, EmitDefaultValue = true)]
    public string Index { get; set; }

    /// <summary>
    /// List of object identifiers for items of an Algolia index.
    /// </summary>
    /// <value>List of object identifiers for items of an Algolia index.</value>
    [DataMember(Name = "objectIDs", IsRequired = true, EmitDefaultValue = true)]
    public List<string> ObjectIDs { get; set; }

    /// <summary>
    /// Anonymous or pseudonymous user identifier.   &gt; **Note**: Never include personally identifiable information in user tokens. 
    /// </summary>
    /// <value>Anonymous or pseudonymous user identifier.   &gt; **Note**: Never include personally identifiable information in user tokens. </value>
    [DataMember(Name = "userToken", IsRequired = true, EmitDefaultValue = true)]
    public string UserToken { get; set; }

    /// <summary>
    /// Time of the event in milliseconds in [Unix epoch time](https://wikipedia.org/wiki/Unix_time). By default, the Insights API uses the time it receives an event as its timestamp. 
    /// </summary>
    /// <value>Time of the event in milliseconds in [Unix epoch time](https://wikipedia.org/wiki/Unix_time). By default, the Insights API uses the time it receives an event as its timestamp. </value>
    [DataMember(Name = "timestamp", EmitDefaultValue = false)]
    public long Timestamp { get; set; }

    /// <summary>
    /// User token for authenticated users.
    /// </summary>
    /// <value>User token for authenticated users.</value>
    [DataMember(Name = "authenticatedUserToken", EmitDefaultValue = false)]
    public string AuthenticatedUserToken { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("class ConvertedObjectIDs {\n");
      sb.Append("  EventName: ").Append(EventName).Append("\n");
      sb.Append("  EventType: ").Append(EventType).Append("\n");
      sb.Append("  Index: ").Append(Index).Append("\n");
      sb.Append("  ObjectIDs: ").Append(ObjectIDs).Append("\n");
      sb.Append("  UserToken: ").Append(UserToken).Append("\n");
      sb.Append("  Timestamp: ").Append(Timestamp).Append("\n");
      sb.Append("  AuthenticatedUserToken: ").Append(AuthenticatedUserToken).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }

    /// <summary>
    /// Returns the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public virtual string ToJson()
    {
      return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
    }

    /// <summary>
    /// Returns true if objects are equal
    /// </summary>
    /// <param name="input">Object to be compared</param>
    /// <returns>Boolean</returns>
    public override bool Equals(object input)
    {
      return this.Equals(input as ConvertedObjectIDs);
    }

    /// <summary>
    /// Returns true if ConvertedObjectIDs instances are equal
    /// </summary>
    /// <param name="input">Instance of ConvertedObjectIDs to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(ConvertedObjectIDs input)
    {
      if (input == null)
      {
        return false;
      }
      return
          (
              this.EventName == input.EventName ||
              (this.EventName != null &&
              this.EventName.Equals(input.EventName))
          ) &&
          (
              this.EventType == input.EventType ||
              this.EventType.Equals(input.EventType)
          ) &&
          (
              this.Index == input.Index ||
              (this.Index != null &&
              this.Index.Equals(input.Index))
          ) &&
          (
              this.ObjectIDs == input.ObjectIDs ||
              this.ObjectIDs != null &&
              input.ObjectIDs != null &&
              this.ObjectIDs.SequenceEqual(input.ObjectIDs)
          ) &&
          (
              this.UserToken == input.UserToken ||
              (this.UserToken != null &&
              this.UserToken.Equals(input.UserToken))
          ) &&
          (
              this.Timestamp == input.Timestamp ||
              this.Timestamp.Equals(input.Timestamp)
          ) &&
          (
              this.AuthenticatedUserToken == input.AuthenticatedUserToken ||
              (this.AuthenticatedUserToken != null &&
              this.AuthenticatedUserToken.Equals(input.AuthenticatedUserToken))
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
        if (this.EventName != null)
        {
          hashCode = (hashCode * 59) + this.EventName.GetHashCode();
        }
        hashCode = (hashCode * 59) + this.EventType.GetHashCode();
        if (this.Index != null)
        {
          hashCode = (hashCode * 59) + this.Index.GetHashCode();
        }
        if (this.ObjectIDs != null)
        {
          hashCode = (hashCode * 59) + this.ObjectIDs.GetHashCode();
        }
        if (this.UserToken != null)
        {
          hashCode = (hashCode * 59) + this.UserToken.GetHashCode();
        }
        hashCode = (hashCode * 59) + this.Timestamp.GetHashCode();
        if (this.AuthenticatedUserToken != null)
        {
          hashCode = (hashCode * 59) + this.AuthenticatedUserToken.GetHashCode();
        }
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
      // EventName (string) maxLength
      if (this.EventName != null && this.EventName.Length > 64)
      {
        yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for EventName, length must be less than 64.", new[] { "EventName" });
      }

      // EventName (string) minLength
      if (this.EventName != null && this.EventName.Length < 1)
      {
        yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for EventName, length must be greater than 1.", new[] { "EventName" });
      }

      if (this.EventName != null)
      {
        // EventName (string) pattern
        Regex regexEventName = new Regex(@"[\x20-\x7E]{1,64}", RegexOptions.CultureInvariant);
        if (!regexEventName.Match(this.EventName).Success)
        {
          yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for EventName, must match a pattern of " + regexEventName, new[] { "EventName" });
        }
      }

      // UserToken (string) maxLength
      if (this.UserToken != null && this.UserToken.Length > 129)
      {
        yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for UserToken, length must be less than 129.", new[] { "UserToken" });
      }

      // UserToken (string) minLength
      if (this.UserToken != null && this.UserToken.Length < 1)
      {
        yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for UserToken, length must be greater than 1.", new[] { "UserToken" });
      }

      if (this.UserToken != null)
      {
        // UserToken (string) pattern
        Regex regexUserToken = new Regex(@"[a-zA-Z0-9_=/+-]{1,129}", RegexOptions.CultureInvariant);
        if (!regexUserToken.Match(this.UserToken).Success)
        {
          yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for UserToken, must match a pattern of " + regexUserToken, new[] { "UserToken" });
        }
      }

      yield break;
    }
  }

}
