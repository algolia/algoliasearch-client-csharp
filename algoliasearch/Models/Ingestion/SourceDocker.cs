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

namespace Algolia.Search.Models.Ingestion;

/// <summary>
/// SourceDocker
/// </summary>
public partial class SourceDocker
{

  /// <summary>
  /// Gets or Sets ImageType
  /// </summary>
  [JsonPropertyName("imageType")]
  public DockerImageType? ImageType { get; set; }

  /// <summary>
  /// Gets or Sets Registry
  /// </summary>
  [JsonPropertyName("registry")]
  public DockerRegistry? Registry { get; set; }
  /// <summary>
  /// Initializes a new instance of the SourceDocker class.
  /// </summary>
  [JsonConstructor]
  public SourceDocker() { }
  /// <summary>
  /// Initializes a new instance of the SourceDocker class.
  /// </summary>
  /// <param name="imageType">imageType (required).</param>
  /// <param name="registry">registry (required).</param>
  /// <param name="image">The name of the image to pull. (required).</param>
  /// <param name="varConfiguration">The configuration of the spec. (required).</param>
  public SourceDocker(DockerImageType? imageType, DockerRegistry? registry, string image, object varConfiguration)
  {
    ImageType = imageType;
    Registry = registry;
    Image = image ?? throw new ArgumentNullException(nameof(image));
    VarConfiguration = varConfiguration ?? throw new ArgumentNullException(nameof(varConfiguration));
  }

  /// <summary>
  /// The name of the image to pull.
  /// </summary>
  /// <value>The name of the image to pull.</value>
  [JsonPropertyName("image")]
  public string Image { get; set; }

  /// <summary>
  /// The version of the image, defaults to `latest`.
  /// </summary>
  /// <value>The version of the image, defaults to `latest`.</value>
  [JsonPropertyName("version")]
  public string VarVersion { get; set; }

  /// <summary>
  /// The configuration of the spec.
  /// </summary>
  /// <value>The configuration of the spec.</value>
  [JsonPropertyName("configuration")]
  public object VarConfiguration { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class SourceDocker {\n");
    sb.Append("  ImageType: ").Append(ImageType).Append("\n");
    sb.Append("  Registry: ").Append(Registry).Append("\n");
    sb.Append("  Image: ").Append(Image).Append("\n");
    sb.Append("  VarVersion: ").Append(VarVersion).Append("\n");
    sb.Append("  VarConfiguration: ").Append(VarConfiguration).Append("\n");
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
    if (obj is not SourceDocker input)
    {
      return false;
    }

    return
        (ImageType == input.ImageType || ImageType.Equals(input.ImageType)) &&
        (Registry == input.Registry || Registry.Equals(input.Registry)) &&
        (Image == input.Image || (Image != null && Image.Equals(input.Image))) &&
        (VarVersion == input.VarVersion || (VarVersion != null && VarVersion.Equals(input.VarVersion))) &&
        (VarConfiguration == input.VarConfiguration || (VarConfiguration != null && VarConfiguration.Equals(input.VarConfiguration)));
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
      hashCode = (hashCode * 59) + ImageType.GetHashCode();
      hashCode = (hashCode * 59) + Registry.GetHashCode();
      if (Image != null)
      {
        hashCode = (hashCode * 59) + Image.GetHashCode();
      }
      if (VarVersion != null)
      {
        hashCode = (hashCode * 59) + VarVersion.GetHashCode();
      }
      if (VarConfiguration != null)
      {
        hashCode = (hashCode * 59) + VarConfiguration.GetHashCode();
      }
      return hashCode;
    }
  }

}
