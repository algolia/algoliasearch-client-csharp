using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Algolia.Search.Models;
using System.Net;

namespace Algolia.Search
{
    public class Analytics
    {
		private AlgoliaClient _client;

		public Analytics(AlgoliaClient client)
		{
			_client = client;
		}

		/// <summary>
		/// Add an object to this index.
		/// </summary>
		/// <param name="content">The object you want to add to the index.</param>
		/// <param name="token"></param>
		/// <returns>An object that contains an "abtestID" attribute.</returns>
		public Task<JObject> AddABTestAsync(object content, CancellationToken token = default(CancellationToken))
		{
			return _client.ExecuteRequest(AlgoliaClient.callType.Analytics, "POST", "/2/abtests", content, token, null);
		}

		/// <summary>
		/// Synchronously call <see cref="Index.AddABTestAsync"/>.
		/// </summary>
		/// <param name="content">The object you want to add to the index.</param>
		/// <returns>An object that contains an "abtestID" attribute.</returns>
		public JObject AddABTest(object content)
		{
			return AddABTestAsync(content, default(CancellationToken)).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Retrieve a list of ABTest
		/// </summary>
		/// <param name="params">limit and offset of the list to retrieve.</param>
		/// <param name="token"></param>
		/// <returns>An object that contains a list of AB Tests.</returns>
		public Task<JObject> GetABTestsAsync(Dictionary<string, object> parameters, CancellationToken token = default(CancellationToken))
		{
			if (!parameters.ContainsKey("limit"))
			{
				parameters.Add("limit", 10);
			}
			if (!parameters.ContainsKey("offset"))
			{
				parameters.Add("offset", 0);
			}

			return _client.ExecuteRequest(
				AlgoliaClient.callType.Analytics,
				"GET",
				$"/2/abtests?offest={WebUtility.UrlEncode(parameters["offset"].ToString())}&limit={WebUtility.UrlEncode(parameters["limit"].ToString())}",
				null,
				token,
				null
			);
		}

		/// <summary>
		/// Retrieve a list of ABTest
		/// </summary>
		/// <param name="params">limit and offset of the list to retrieve.</param>
		/// <param name="token"></param>
		/// <returns>An object that contains a list of AB Tests.</returns>
		public JObject GetABTests(Dictionary<string, object> parameters)
		{
			return GetABTestsAsync(parameters, default(CancellationToken)).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Retrieve an ABTest
		/// </summary>
		/// <param name="abTestID">The ID of the ABTest to retrieve.</param>
		/// <param name="token"></param>
		/// <returns>An object that contains a full AB Test.</returns>
		public Task<JObject> GetABTestAsync(int abTestID, CancellationToken token = default(CancellationToken))
		{
			return _client.ExecuteRequest(AlgoliaClient.callType.Analytics, "GET", $"/2/abtests/{abTestID}", null, token, null);
		}

		/// <summary>
		/// Retrieve an ABTest
		/// </summary>
		/// <param name="abTestID">The ID of the ABTest to retrieve.</param>
		/// <returns>An object that contains a full AB Test.</returns>
		public JObject GetABTest(int abTestID)
		{
			return GetABTestAsync(abTestID, default(CancellationToken)).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Stop (pause) an active ABTest
		/// </summary>
		/// <param name="abTestID">The ID of the ABTest to stop.</param>
		/// <param name="token"></param>
		/// <returns>An object that contains a taskID and the corresponding index name.</returns>
		public Task<JObject> StopABTestAsync(int abTestID, CancellationToken token = default(CancellationToken))
		{
			return _client.ExecuteRequest(AlgoliaClient.callType.Analytics, "POST", $"/2/abtests/{abTestID}/stop", "", token, null);
		}

		/// <summary>
		/// Stop (pause) an active ABTest
		/// </summary>
		/// <param name="abTestID">The ID of the ABTest to stop.</param>
		/// <returns>An object that contains a taskID and the corresponding index name.</returns>
		public JObject StopABTest(int abTestID)
		{
			return StopABTestAsync(abTestID, default(CancellationToken)).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Delete an ABTest
		/// </summary>
		/// <param name="abTestID">The ID of the ABTest to delete.</param>
		/// <param name="token"></param>
		/// <returns>An object that contains a taskID and the corresponding index name.</returns>
		public Task<JObject> DeleteABTestAsync(int abTestID, CancellationToken token = default(CancellationToken))
		{
			return _client.ExecuteRequest(AlgoliaClient.callType.Analytics, "DELETE", $"/2/abtests/{abTestID}", null, token, null);
		}

		/// <summary>
		/// Delete an ABTest
		/// </summary>
		/// <param name="abTestID">The ID of the ABTest to delete.</param>
		/// <param name="token"></param>
		/// <returns>An object that contains a taskID and the corresponding index name.</returns>
		public JObject DeleteABTest(int abTestID)
		{
			return DeleteABTestAsync(abTestID, default(CancellationToken)).GetAwaiter().GetResult();
		}



		/// <summary>
		/// Check to see if the asynchronous server task is complete.
		/// </summary>
		/// <param name="indexName">The index associated with the task.</param>
		/// <param name="taskID">The id of the task returned by server.</param>
		/// <param name="requestOptions"></param>
		/// <param name="timeToWait"></param>
		/// <param name="token"></param>
		async public Task WaitTaskAsync(string indexName, string taskID, RequestOptions requestOptions = null, int timeToWait = 100, CancellationToken token = default(CancellationToken))
		{
			await _client.InitIndex(indexName).WaitTaskAsync(taskID, requestOptions, timeToWait, token);
		}

		/// <summary>
		/// Synchronously call <see cref="Index.WaitTaskAsync"/>.
		/// </summary>
		/// <param name="indexName">The index associated with the task.</param>
		/// <param name="taskID">The id of the task returned by server.</param>
		/// <param name="requestOptions"></param>
		/// <param name="timeToWait"></param>
		public void WaitTask(string indexName, string taskID, RequestOptions requestOptions = null, int timeToWait = 100)
		{
			WaitTaskAsync(indexName, taskID, requestOptions, timeToWait, default(CancellationToken)).GetAwaiter().GetResult();
		}
	}
}
