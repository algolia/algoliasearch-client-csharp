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
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Algolia.Search.Exceptions;
using Algolia.Search.Http;
using Algolia.Search.Models.ApiKeys;
using Algolia.Search.Models.Batch;
using Algolia.Search.Models.Common;
using Algolia.Search.Models.Dictionary;
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Mcm;
using Algolia.Search.Models.Personalization;
using Algolia.Search.Models.Search;
using Algolia.Search.Transport;
using Algolia.Search.Utils;

namespace Algolia.Search.Clients
{
    /// <summary>
    /// Algolia search client implementation of <see cref="ISearchClientDictionary"/>
    /// </summary>

    public class SearchClientDictionary : ISearchClientDictionary
    {
        private readonly HttpTransport _transport;

        /// <inheritdoc />
        public DictionaryResponse SaveDictionaryEntries(Dictionary dictionary, List<DictionaryEntry> dictionaryEntries,
            RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => SaveDictionaryEntriesAsync(dictionary, dictionaryEntries, requestOptions));

        /// <inheritdoc />
        public async Task<DictionaryResponse> SaveDictionaryEntriesAsync(Dictionary dictionary, List<DictionaryEntry> dictionaryEntries, RequestOptions requestOptions = null, CancellationToken ct = default)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("a dictionary is required");
            }

            if (dictionaryEntries == null)
            {
                throw new ArgumentNullException("Dictionary entries are required");
            }

            var request = DictionaryRequest;

            DictionaryResponse response = await _transport
                .ExecuteRequestAsync<DictionaryResponse, DictionaryRequest>(HttpMethod.Post,
                    $"/1/dictionaries/{dictionary}/batch", CallType.Write, request, requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitTask = t => WaitAppTask(t);
            return response;
        }

        /// <inheritdoc />
        public DictionaryResponse ReplaceDictionaryEntries(Dictionary dictionary, List<DictionaryEntry> dictionaryEntries,
            RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => ReplaceDictionaryEntriesAsync(dictionary, dictionaryEntries, requestOptions));

        /// <inheritdoc />
        public async Task<DictionaryResponse> ReplaceDictionaryEntriesAsync(Dictionary dictionary, List<DictionaryEntry> dictionaryEntries, RequestOptions requestOptions = null, CancellationToken ct = default)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("a dictionary is required");
            }

            if (dictionaryEntries == null)
            {
                throw new ArgumentNullException("Dictionary entries are required");
            }

            var request = DictionaryRequest;

            DictionaryResponse response = await _transport
                .ExecuteRequestAsync<DictionaryResponse, DictionaryRequest>(HttpMethod.Post,
                    $"/1/dictionaries/{dictionary}/batch", CallType.Write, request, requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitTask = t => WaitAppTask(t);
            return response;
        }

        /// <inheritdoc />
        public DictionaryResponse DeleteDictionaryEntries(Dictionary dictionary, List<String> ObjectIDs,
            RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => DeleteDictionaryEntriesAsync(dictionary, ObjectIDs, requestOptions));

        /// <inheritdoc />
        public async Task<DictionaryResponse> DeleteDictionaryEntriesAsync(Dictionary dictionary, List<String> ObjectIDs, RequestOptions requestOptions = null, CancellationToken ct = default)
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

            var request = DictionaryRequest;

            DictionaryResponse response = await _transport
                .ExecuteRequestAsync<DictionaryResponse, DictionaryRequest>(HttpMethod.Post,
                    $"/1/dictionaries/{dictionary}/batch", CallType.Write, request, requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitTask = t => WaitAppTask(t);
            return response;
        }

        /// <inheritdoc />
        public DictionaryResponse ClearDictionaryEntries(Dictionary dictionary, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => ClearDictionaryEntriesAsync(dictionary, requestOptions));

        /// <inheritdoc />
        public async Task<DictionaryResponse> ClearDictionaryEntriesAsync(Dictionary dictionary,
            RequestOptions requestOptions = null, CancellationToken ct = default)
        {
            return await ReplaceDictionaryEntriesAsync(dictionary, new List<DictionaryEntry> { }, requestOptions);
        }

        /// <inheritdoc />
        public SearchResponse<T> SearchDictionaryEntries<T>(Dictionary dictionary, Query query, RequestOptions requestOptions = null) where T : class =>
            AsyncHelper.RunSync(() => SearchDictionaryEntriesAsync<T>(dictionary, query, requestOptions));

        /// <inheritdoc />
        public async Task<SearchResponse<T>> SearchDictionaryEntriesAsync<T>(Dictionary dictionary, Query query,
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
