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

namespace Algolia.Search.Models.Search;

/// <summary>
/// Unique user ID.
/// </summary>
public partial class UserId
{
  /// <summary>
  /// Initializes a new instance of the UserId class.
  /// </summary>
  [JsonConstructor]
  public UserId() { }

  /// <summary>
  /// Initializes a new instance of the UserId class.
  /// </summary>
  /// <param name="userID">Unique identifier of the user who makes the search request. (required).</param>
  /// <param name="clusterName">Cluster to which the user is assigned. (required).</param>
  /// <param name="nbRecords">Number of records belonging to the user. (required).</param>
  /// <param name="dataSize">Data size used by the user. (required).</param>
  public UserId(string userID, string clusterName, int nbRecords, int dataSize)
  {
    UserID = userID ?? throw new ArgumentNullException(nameof(userID));
    ClusterName = clusterName ?? throw new ArgumentNullException(nameof(clusterName));
    NbRecords = nbRecords;
    DataSize = dataSize;
  }

  /// <summary>
  /// Unique identifier of the user who makes the search request.
  /// </summary>
  /// <value>Unique identifier of the user who makes the search request.</value>
  [JsonPropertyName("userID")]
  public string UserID { get; set; }

  /// <summary>
  /// Cluster to which the user is assigned.
  /// </summary>
  /// <value>Cluster to which the user is assigned.</value>
  [JsonPropertyName("clusterName")]
  public string ClusterName { get; set; }

  /// <summary>
  /// Number of records belonging to the user.
  /// </summary>
  /// <value>Number of records belonging to the user.</value>
  [JsonPropertyName("nbRecords")]
  public int NbRecords { get; set; }

  /// <summary>
  /// Data size used by the user.
  /// </summary>
  /// <value>Data size used by the user.</value>
  [JsonPropertyName("dataSize")]
  public int DataSize { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class UserId {\n");
    sb.Append("  UserID: ").Append(UserID).Append("\n");
    sb.Append("  ClusterName: ").Append(ClusterName).Append("\n");
    sb.Append("  NbRecords: ").Append(NbRecords).Append("\n");
    sb.Append("  DataSize: ").Append(DataSize).Append("\n");
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
    if (obj is not UserId input)
    {
      return false;
    }

    return (UserID == input.UserID || (UserID != null && UserID.Equals(input.UserID)))
      && (
        ClusterName == input.ClusterName
        || (ClusterName != null && ClusterName.Equals(input.ClusterName))
      )
      && (NbRecords == input.NbRecords || NbRecords.Equals(input.NbRecords))
      && (DataSize == input.DataSize || DataSize.Equals(input.DataSize));
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
      if (UserID != null)
      {
        hashCode = (hashCode * 59) + UserID.GetHashCode();
      }
      if (ClusterName != null)
      {
        hashCode = (hashCode * 59) + ClusterName.GetHashCode();
      }
      hashCode = (hashCode * 59) + NbRecords.GetHashCode();
      hashCode = (hashCode * 59) + DataSize.GetHashCode();
      return hashCode;
    }
  }
}
