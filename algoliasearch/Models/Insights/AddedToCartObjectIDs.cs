//
// Code generated by OpenAPI Generator (https://openapi-generator.tech), manual changes will be lost - read more on https://github.com/algolia/api-clients-automation. DO NOT EDIT.
//
using System;
using System.Text;
using System.Linq;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using Algolia.Search.Serializer;
using System.Text.Json;

namespace Algolia.Search.Models.Insights;

/// <summary>
/// Use this event to track when users add items to their shopping cart unrelated to a previous Algolia request. For example, if you don't use Algolia to build your category pages, use this event.  To track add-to-cart events related to Algolia requests, use the \"Added to cart object IDs after search\" event. 
/// </summary>
public partial class AddedToCartObjectIDs
{

  /// <summary>
  /// Gets or Sets EventType
  /// </summary>
  [JsonPropertyName("eventType")]
  public ConversionEvent? EventType { get; set; }

  /// <summary>
  /// Gets or Sets EventSubtype
  /// </summary>
  [JsonPropertyName("eventSubtype")]
  public AddToCartEvent? EventSubtype { get; set; }
  /// <summary>
  /// Initializes a new instance of the AddedToCartObjectIDs class.
  /// </summary>
  [JsonConstructor]
  public AddedToCartObjectIDs() { }
  /// <summary>
  /// Initializes a new instance of the AddedToCartObjectIDs class.
  /// </summary>
  /// <param name="eventName">The name of the event, up to 64 ASCII characters.  Consider naming events consistently—for example, by adopting Segment&#39;s [object-action](https://segment.com/academy/collecting-data/naming-conventions-for-clean-data/#the-object-action-framework) framework.  (required).</param>
  /// <param name="eventType">eventType (required).</param>
  /// <param name="eventSubtype">eventSubtype (required).</param>
  /// <param name="index">The name of an Algolia index. (required).</param>
  /// <param name="objectIDs">The object IDs of the records that are part of the event. (required).</param>
  /// <param name="userToken">An anonymous or pseudonymous user identifier.  &gt; **Note**: Never include personally identifiable information in user tokens.  (required).</param>
  public AddedToCartObjectIDs(string eventName, ConversionEvent? eventType, AddToCartEvent? eventSubtype, string index, List<string> objectIDs, string userToken)
  {
    EventName = eventName ?? throw new ArgumentNullException(nameof(eventName));
    EventType = eventType;
    EventSubtype = eventSubtype;
    Index = index ?? throw new ArgumentNullException(nameof(index));
    ObjectIDs = objectIDs ?? throw new ArgumentNullException(nameof(objectIDs));
    UserToken = userToken ?? throw new ArgumentNullException(nameof(userToken));
  }

  /// <summary>
  /// The name of the event, up to 64 ASCII characters.  Consider naming events consistently—for example, by adopting Segment's [object-action](https://segment.com/academy/collecting-data/naming-conventions-for-clean-data/#the-object-action-framework) framework. 
  /// </summary>
  /// <value>The name of the event, up to 64 ASCII characters.  Consider naming events consistently—for example, by adopting Segment's [object-action](https://segment.com/academy/collecting-data/naming-conventions-for-clean-data/#the-object-action-framework) framework. </value>
  [JsonPropertyName("eventName")]
  public string EventName { get; set; }

  /// <summary>
  /// The name of an Algolia index.
  /// </summary>
  /// <value>The name of an Algolia index.</value>
  [JsonPropertyName("index")]
  public string Index { get; set; }

  /// <summary>
  /// The object IDs of the records that are part of the event.
  /// </summary>
  /// <value>The object IDs of the records that are part of the event.</value>
  [JsonPropertyName("objectIDs")]
  public List<string> ObjectIDs { get; set; }

  /// <summary>
  /// An anonymous or pseudonymous user identifier.  > **Note**: Never include personally identifiable information in user tokens. 
  /// </summary>
  /// <value>An anonymous or pseudonymous user identifier.  > **Note**: Never include personally identifiable information in user tokens. </value>
  [JsonPropertyName("userToken")]
  public string UserToken { get; set; }

  /// <summary>
  /// An identifier for authenticated users.  > **Note**: Never include personally identifiable information in user tokens. 
  /// </summary>
  /// <value>An identifier for authenticated users.  > **Note**: Never include personally identifiable information in user tokens. </value>
  [JsonPropertyName("authenticatedUserToken")]
  public string AuthenticatedUserToken { get; set; }

  /// <summary>
  /// Three-letter [currency code](https://www.iso.org/iso-4217-currency-codes.html).
  /// </summary>
  /// <value>Three-letter [currency code](https://www.iso.org/iso-4217-currency-codes.html).</value>
  [JsonPropertyName("currency")]
  public string Currency { get; set; }

  /// <summary>
  /// Extra information about the records involved in a purchase or add-to-cart event.  If specified, it must have the same length as `objectIDs`. 
  /// </summary>
  /// <value>Extra information about the records involved in a purchase or add-to-cart event.  If specified, it must have the same length as `objectIDs`. </value>
  [JsonPropertyName("objectData")]
  public List<ObjectData> ObjectData { get; set; }

  /// <summary>
  /// The timestamp of the event in milliseconds in [Unix epoch time](https://wikipedia.org/wiki/Unix_time). By default, the Insights API uses the time it receives an event as its timestamp. 
  /// </summary>
  /// <value>The timestamp of the event in milliseconds in [Unix epoch time](https://wikipedia.org/wiki/Unix_time). By default, the Insights API uses the time it receives an event as its timestamp. </value>
  [JsonPropertyName("timestamp")]
  public long? Timestamp { get; set; }

  /// <summary>
  /// Gets or Sets Value
  /// </summary>
  [JsonPropertyName("value")]
  public Value Value { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class AddedToCartObjectIDs {\n");
    sb.Append("  EventName: ").Append(EventName).Append("\n");
    sb.Append("  EventType: ").Append(EventType).Append("\n");
    sb.Append("  EventSubtype: ").Append(EventSubtype).Append("\n");
    sb.Append("  Index: ").Append(Index).Append("\n");
    sb.Append("  ObjectIDs: ").Append(ObjectIDs).Append("\n");
    sb.Append("  UserToken: ").Append(UserToken).Append("\n");
    sb.Append("  AuthenticatedUserToken: ").Append(AuthenticatedUserToken).Append("\n");
    sb.Append("  Currency: ").Append(Currency).Append("\n");
    sb.Append("  ObjectData: ").Append(ObjectData).Append("\n");
    sb.Append("  Timestamp: ").Append(Timestamp).Append("\n");
    sb.Append("  Value: ").Append(Value).Append("\n");
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
    if (obj is not AddedToCartObjectIDs input)
    {
      return false;
    }

    return
        (EventName == input.EventName || (EventName != null && EventName.Equals(input.EventName))) &&
        (EventType == input.EventType || EventType.Equals(input.EventType)) &&
        (EventSubtype == input.EventSubtype || EventSubtype.Equals(input.EventSubtype)) &&
        (Index == input.Index || (Index != null && Index.Equals(input.Index))) &&
        (ObjectIDs == input.ObjectIDs || ObjectIDs != null && input.ObjectIDs != null && ObjectIDs.SequenceEqual(input.ObjectIDs)) &&
        (UserToken == input.UserToken || (UserToken != null && UserToken.Equals(input.UserToken))) &&
        (AuthenticatedUserToken == input.AuthenticatedUserToken || (AuthenticatedUserToken != null && AuthenticatedUserToken.Equals(input.AuthenticatedUserToken))) &&
        (Currency == input.Currency || (Currency != null && Currency.Equals(input.Currency))) &&
        (ObjectData == input.ObjectData || ObjectData != null && input.ObjectData != null && ObjectData.SequenceEqual(input.ObjectData)) &&
        (Timestamp == input.Timestamp || Timestamp.Equals(input.Timestamp)) &&
        (Value == input.Value || (Value != null && Value.Equals(input.Value)));
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
      if (EventName != null)
      {
        hashCode = (hashCode * 59) + EventName.GetHashCode();
      }
      hashCode = (hashCode * 59) + EventType.GetHashCode();
      hashCode = (hashCode * 59) + EventSubtype.GetHashCode();
      if (Index != null)
      {
        hashCode = (hashCode * 59) + Index.GetHashCode();
      }
      if (ObjectIDs != null)
      {
        hashCode = (hashCode * 59) + ObjectIDs.GetHashCode();
      }
      if (UserToken != null)
      {
        hashCode = (hashCode * 59) + UserToken.GetHashCode();
      }
      if (AuthenticatedUserToken != null)
      {
        hashCode = (hashCode * 59) + AuthenticatedUserToken.GetHashCode();
      }
      if (Currency != null)
      {
        hashCode = (hashCode * 59) + Currency.GetHashCode();
      }
      if (ObjectData != null)
      {
        hashCode = (hashCode * 59) + ObjectData.GetHashCode();
      }
      hashCode = (hashCode * 59) + Timestamp.GetHashCode();
      if (Value != null)
      {
        hashCode = (hashCode * 59) + Value.GetHashCode();
      }
      return hashCode;
    }
  }

}
