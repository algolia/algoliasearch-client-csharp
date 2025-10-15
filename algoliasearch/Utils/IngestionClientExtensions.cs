using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Algolia.Search.Exceptions;
using Algolia.Search.Http;
using Algolia.Search.Models.Ingestion;
using Algolia.Search.Serializer;
using Algolia.Search.Utils;

namespace Algolia.Search.Clients;

public partial interface IIngestionClient
{
  /// <summary>
  /// Helper method to call ChunkedPushAsync and convert the response types.
  /// This simplifies SearchClient helpers that need to use IngestionClient.
  /// </summary>
  Task<List<WatchResponse>> ChunkedPushAsync(
    string indexName,
    IEnumerable<object> objects,
    Models.Ingestion.Action action,
    bool waitForTasks = false,
    int batchSize = 1000,
    string referenceIndexName = null,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  );

  /// <summary>
  /// Synchronous version of ChunkedPushAsync
  /// </summary>
  List<WatchResponse> ChunkedPush(
    string indexName,
    IEnumerable<object> objects,
    Models.Ingestion.Action action,
    bool waitForTasks = false,
    int batchSize = 1000,
    string referenceIndexName = null,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  );
}

public partial class IngestionClient : IIngestionClient
{
  /// <summary>
  /// Helper: Chunks the given `objects` list in subset of 1000 elements max in order to make it fit
  /// in `push` requests by leveraging the Transformation pipeline setup in the Push connector
  /// (https://www.algolia.com/doc/guides/sending-and-managing-data/send-and-update-your-data/connectors/push/).
  /// </summary>
  /// <param name="indexName">The `indexName` to push `objects` to.</param>
  /// <param name="objects">The array of `objects` to store in the given Algolia `indexName`.</param>
  /// <param name="action">The `action` to perform on the given array of `objects`.</param>
  /// <param name="waitForTasks">Whether or not we should wait until every push task has been processed. This operation may slow the total execution time of this method but is more reliable.</param>
  /// <param name="batchSize">The size of the chunk of `objects`. The number of push calls will be equal to `length(objects) / batchSize`. Defaults to 1000.</param>
  /// <param name="referenceIndexName">This is required when targeting an index that does not have a push connector setup (e.g. a tmp index), but you wish to attach another index's transformation to it (e.g. the source index name).</param>
  /// <param name="options">Add extra http header or query parameters to Algolia.</param>
  /// <param name="cancellationToken">Cancellation token to cancel the request</param>
  /// <returns>List of WatchResponse objects from the push operations</returns>
  public async Task<List<WatchResponse>> ChunkedPushAsync(
    string indexName,
    IEnumerable<object> objects,
    Algolia.Search.Models.Ingestion.Action action,
    bool waitForTasks = false,
    int batchSize = 1000,
    string referenceIndexName = null,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  )
  {
    var objectsList = objects.ToList();
    var responses = new List<WatchResponse>();
    var waitBatchSize = Math.Max(batchSize / 10, 1);
    var offset = 0;

    for (var i = 0; i < objectsList.Count; i += batchSize)
    {
      var chunk = objectsList.Skip(i).Take(batchSize);
      var records = new List<PushTaskRecords>();

      foreach (var obj in chunk)
      {
        var jsonString = JsonSerializer.Serialize(obj, JsonConfig.Options);
        var record = JsonSerializer.Deserialize<PushTaskRecords>(jsonString, JsonConfig.Options);
        records.Add(record);
      }

      var payload = new PushTaskPayload(action, records);

      var response = await PushAsync(
          indexName,
          payload,
          watch: null,
          referenceIndexName: referenceIndexName,
          options: options,
          cancellationToken: cancellationToken
        )
        .ConfigureAwait(false);

      responses.Add(response);

      if (
        waitForTasks
        && responses.Count > 0
        && (responses.Count % waitBatchSize == 0 || i + batchSize >= objectsList.Count)
      )
      {
        for (var j = offset; j < responses.Count; j++)
        {
          var resp = responses[j];
          if (string.IsNullOrEmpty(resp.EventID))
          {
            throw new AlgoliaException(
              "Received unexpected response from the push endpoint, eventID must not be null or empty"
            );
          }

          await RetryHelper
            .RetryUntil(
              async () =>
              {
                try
                {
                  return await GetEventAsync(
                      resp.RunID,
                      resp.EventID,
                      cancellationToken: cancellationToken
                    )
                    .ConfigureAwait(false);
                }
                catch (AlgoliaApiException ex) when (ex.HttpErrorCode == 404)
                {
                  return await Task.FromResult<Algolia.Search.Models.Ingestion.Event>(null);
                }
              },
              eventResponse => eventResponse != null,
              maxRetries: 50,
              ct: cancellationToken
            )
            .ConfigureAwait(false);
        }
        offset = responses.Count;
      }
    }

    return responses;
  }

  /// <summary>
  /// Synchronous version of ChunkedPushAsync
  /// </summary>
  public List<WatchResponse> ChunkedPush(
    string indexName,
    IEnumerable<object> objects,
    Algolia.Search.Models.Ingestion.Action action,
    bool waitForTasks = false,
    int batchSize = 1000,
    string referenceIndexName = null,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  ) =>
    AsyncHelper.RunSync(() =>
      ChunkedPushAsync(
        indexName,
        objects,
        action,
        waitForTasks,
        batchSize,
        referenceIndexName,
        options,
        cancellationToken
      )
    );
}
