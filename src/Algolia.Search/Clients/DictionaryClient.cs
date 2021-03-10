/*
* Copyright (c) 2021 Algolia
* http://www.algolia.com/
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Algolia.Search.Http;
using Algolia.Search.Models.Common;
using Algolia.Search.Models.Dictionary;
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Search;
using Algolia.Search.Transport;
using Algolia.Search.Utils;

namespace Algolia.Search.Clients
{
    /// <summary>
    /// Algolia search client implementation of <see cref="IDictionaryClient"/>
    /// </summary>

    public class DictionaryClient : IDictionaryClient
    {
        private readonly HttpTransport _transport;
        private readonly AlgoliaConfig _config;

        /// <summary>
        /// Create a new dictionary client for the given appID
        /// </summary>
        /// <param name="applicationId">Your application</param>
        /// <param name="apiKey">Your API key</param>
        public DictionaryClient(string applicationId, string apiKey) : this(
            new SearchConfig(applicationId, apiKey), new AlgoliaHttpRequester())
        {
        }

        /// <summary>
        /// Initialize a dictionary client with custom config
        /// </summary>
        /// <param name="config">Algolia configuration</param>
        public DictionaryClient(SearchConfig config) : this(config, new AlgoliaHttpRequester())
        {
        }

        /// <summary>
        /// Initialize the dictionary client with custom config and custom Requester
        /// </summary>
        /// <param name="config">Algolia Config</param>
        /// <param name="httpRequester">Your Http requester implementation of <see cref="IHttpRequester"/></param>
        public DictionaryClient(SearchConfig config, IHttpRequester httpRequester)
        {
            if (httpRequester == null)
            {
                throw new ArgumentNullException(nameof(httpRequester), "An httpRequester is required");
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config), "A config is required");
            }

            if (string.IsNullOrWhiteSpace(config.AppId))
            {
                throw new ArgumentNullException(nameof(config.AppId), "Application ID is required");
            }

            if (string.IsNullOrWhiteSpace(config.ApiKey))
            {
                throw new ArgumentNullException(nameof(config.ApiKey), "An API key is required");
            }

            _config = config;
            _transport = new HttpTransport(config, httpRequester);
        }

        /// <inheritdoc />
        public DictionaryResponse SaveDictionaryEntries(AlgoliaDictionary dictionary, List<DictionaryEntry> dictionaryEntries,
            RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => SaveDictionaryEntriesAsync(dictionary, dictionaryEntries, requestOptions));

        /// <inheritdoc />
        public async Task<DictionaryResponse> SaveDictionaryEntriesAsync(AlgoliaDictionary dictionary, List<DictionaryEntry> dictionaryEntries, RequestOptions requestOptions = null, CancellationToken ct = default)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("a dictionary is required");
            }

            if (dictionaryEntries == null)
            {
                throw new ArgumentNullException("Dictionary entries are required");
            }

            DictionaryResponse response = await _transport
                .ExecuteRequestAsync<DictionaryResponse>(HttpMethod.Post,
                    $"/1/dictionaries/{dictionary}/batch", CallType.Write, requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitTask = t => WaitAppTask(t);
            return response;
        }

        /// <inheritdoc />
        public DictionaryResponse ReplaceDictionaryEntries(AlgoliaDictionary dictionary, List<DictionaryEntry> dictionaryEntries,
            RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => ReplaceDictionaryEntriesAsync(dictionary, dictionaryEntries, requestOptions));

        /// <inheritdoc />
        public async Task<DictionaryResponse> ReplaceDictionaryEntriesAsync(AlgoliaDictionary dictionary, List<DictionaryEntry> dictionaryEntries, RequestOptions requestOptions = null, CancellationToken ct = default)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("a dictionary is required");
            }

            if (dictionaryEntries == null)
            {
                throw new ArgumentNullException("Dictionary entries are required");
            }

            DictionaryResponse response = await _transport
                .ExecuteRequestAsync<DictionaryResponse>(HttpMethod.Post,
                    $"/1/dictionaries/{dictionary}/batch", CallType.Write, requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitTask = t => WaitAppTask(t);
            return response;
        }

        /// <inheritdoc />
        public DictionaryResponse DeleteDictionaryEntries(AlgoliaDictionary dictionary, List<String> ObjectIDs,
            RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => DeleteDictionaryEntriesAsync(dictionary, ObjectIDs, requestOptions));

        /// <inheritdoc />
        public async Task<DictionaryResponse> DeleteDictionaryEntriesAsync(AlgoliaDictionary dictionary, List<String> ObjectIDs, RequestOptions requestOptions = null, CancellationToken ct = default)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("a dictionary is required");
            }

            if (ObjectIDs == null)
            {
                throw new ArgumentNullException("Dictionary entries are required");
            }

            if (!ObjectIDs.Any())
            {
                throw new ArgumentException("objectIDs can't be empty");
            }

            DictionaryResponse response = await _transport
                .ExecuteRequestAsync<DictionaryResponse>(HttpMethod.Post,
                    $"/1/dictionaries/{dictionary}/batch", CallType.Write, requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitTask = t => WaitAppTask(t);
            return response;
        }

        /// <inheritdoc />
        public DictionaryResponse ClearDictionaryEntries(AlgoliaDictionary dictionary, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => ClearDictionaryEntriesAsync(dictionary, requestOptions));

        /// <inheritdoc />
        public async Task<DictionaryResponse> ClearDictionaryEntriesAsync(AlgoliaDictionary dictionary,
            RequestOptions requestOptions = null, CancellationToken ct = default)
        {
            return await ReplaceDictionaryEntriesAsync(dictionary, new List<DictionaryEntry> { }, requestOptions);
        }

        /// <inheritdoc />
        public SearchResponse<T> SearchDictionaryEntries<T>(AlgoliaDictionary dictionary, Query query, RequestOptions requestOptions = null) where T : class =>
            AsyncHelper.RunSync(() => SearchDictionaryEntriesAsync<T>(dictionary, query, requestOptions));

        /// <inheritdoc />
        public async Task<SearchResponse<T>> SearchDictionaryEntriesAsync<T>(AlgoliaDictionary dictionary, Query query,
            RequestOptions requestOptions = null, CancellationToken ct = default) where T : class
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("a dictionary is required");
            }

            if (query == null)
            {
                throw new ArgumentNullException("A query key is required.");
            }

            return await _transport
                .ExecuteRequestAsync<SearchResponse<T>, Query>(HttpMethod.Post,
                    $"/1/dictionaries/{dictionary}/search", CallType.Read, query, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public DictionaryResponse SetDictionarySettings(DictionarySettings dictionarySettings, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => SetDictionarySettingsAsync(dictionarySettings, requestOptions));

        /// <inheritdoc />
        public async Task<DictionaryResponse> SetDictionarySettingsAsync(DictionarySettings dictionarySettings, RequestOptions requestOptions = null, CancellationToken ct = default)
        {
            if (dictionarySettings == null)
            {
                throw new ArgumentNullException("Dictionary settings are required");
            }

            DictionaryResponse response = await _transport
                .ExecuteRequestAsync<DictionaryResponse, DictionarySettings>(HttpMethod.Put,
                    $"/1/dictionaries/*/settings", CallType.Write, dictionarySettings, requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitTask = t => WaitAppTask(t);
            return response;
        }

        /// <inheritdoc />
        public DictionarySettings GetDictionarySettings(RequestOptions requestOptions = null) =>
           AsyncHelper.RunSync(() => GetDictionarySettingsAsync(requestOptions));

        /// <inheritdoc />
        public async Task<DictionarySettings> GetDictionarySettingsAsync(RequestOptions requestOptions = null, CancellationToken ct = default)
        {
            return await _transport
               .ExecuteRequestAsync<DictionarySettings>(HttpMethod.Get,
                   $"/1/dictionaries/*/settings", CallType.Write, requestOptions, ct)
               .ConfigureAwait(false);
        }


        /// <inheritdoc />
        public void WaitAppTask(long taskId, int timeToWait = 100, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => WaitAppTaskAsync(taskId, timeToWait, requestOptions));

        /// <inheritdoc />
        public async Task WaitAppTaskAsync(long taskId, int timeToWait = 100, RequestOptions requestOptions = null, CancellationToken ct = default)
        {
            while (true)
            {
                TaskStatusResponse response = await GetAppTaskAsync(taskId, requestOptions, ct).ConfigureAwait(false);

                if (response.Status.Equals("published"))
                {
                    return;
                }

                await Task.Delay(timeToWait, ct).ConfigureAwait(false);
                timeToWait *= 2;

                if (timeToWait > Defaults.MaxTimeToWait)
                {
                    timeToWait = Defaults.MaxTimeToWait;
                }
            }
        }

        /// <inheritdoc />
        public TaskStatusResponse GetAppTask(long taskId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => GetAppTaskAsync(taskId, requestOptions));

        /// <inheritdoc />
        public async Task<TaskStatusResponse> GetAppTaskAsync(long taskId, RequestOptions requestOptions = null,
            CancellationToken ct = default)
        {
            return await _transport.ExecuteRequestAsync<TaskStatusResponse>(HttpMethod.Get,
                    $"/1/task/{taskId}", CallType.Read, requestOptions, ct)
                .ConfigureAwait(false);
        }
    }
}
