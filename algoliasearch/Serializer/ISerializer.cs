using System.IO;
using System.Threading.Tasks;
using Algolia.Search.Models.Common;

namespace Algolia.Search.Serializer
{
  /// <summary>
  /// Interface representing the expected behavior of the Serializer.
  /// </summary>
  internal interface ISerializer
  {
    /// <summary>
    /// Converts the value of a specified type into a JSON string.
    /// </summary>
    /// <param name="data">The value to convert and write.</param>
    string Serialize(object data);

    /// <summary>
    /// Parses the stream into an instance of a specified type.
    /// </summary>
    /// <param name="stream">The Stream containing the data to read.</param>
    /// <typeparam name="T">The type of the value to convert.</typeparam>
    /// <returns></returns>
    Task<T> Deserialize<T>(Stream stream);
  }
}
