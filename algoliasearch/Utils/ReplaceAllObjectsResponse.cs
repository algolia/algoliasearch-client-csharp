using System.Collections.Generic;
using Algolia.Search.Models.Search;

namespace Algolia.Search.Utils;

/// <summary>
/// All responses from the ReplaceAllObjects calls.
/// </summary>
public class ReplaceAllObjectsResponse
{
  /// <summary>
  /// The response of the copy operation.
  /// </summary>
  public UpdatedAtResponse CopyOperationResponse { get; set; }

  /// <summary>
  /// The response of the batch operations.
  /// </summary>
  public List<BatchResponse> BatchResponses { get; set; }

  /// <summary>
  /// The response of the move operation.
  /// </summary>
  public UpdatedAtResponse MoveOperationResponse { get; set; }
}
