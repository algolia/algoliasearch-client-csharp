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
using System.Threading;
using System.Threading.Tasks;
using Algolia.Search.Http;
using Algolia.Search.Models.Common;
using Algolia.Search.Models.Dictionary;
using Algolia.Search.Models.Search;

namespace Algolia.Search.Clients
{
    /// <summary>
    /// Search Client Dictionary interface
    /// </summary>

    public interface ISearchClientDictionary
    {
        /// <summary>
        /// Save dictionary entries.
        /// </summary>
        /// <param name="dictionary">Target dictionary.</param>
        /// <param name="dictionaryEntries">Dictionary entries to be saved. entries from the dictionary.</param>
        /// <param name="requestOptions">Configure request locally with RequestOptions.</param>
        /// <returns></returns>
        DictionaryResponse SaveDictionaryEntries(Dictionary dictionary, List<DictionaryEntry> dictionaryEntries, RequestOptions requestOptions = null);

        /// <summary>
        /// Save dictionary entries.
        /// </summary>
        /// <param name="dictionary">Target dictionary.</param>
        /// <param name="dictionaryEntries">Dictionary entries to be saved. entries from the dictionary.</param>
        /// <param name="requestOptions">Configure request locally with RequestOptions.</param>
        /// <param name="ct">Cancelation token.</param>
        /// <returns></returns>
        Task<DictionaryResponse> SaveDictionaryEntriesAsync(Dictionary dictionary, List<DictionaryEntry> dictionaryEntries, RequestOptions requestOptions = null, CancellationToken ct = default);

        /// <summary>
        /// Replace dictionary entries.
        /// </summary>
        /// <param name="dictionary">Target dictionary.</param>
        /// <param name="dictionaryEntries">Dictionary entries to be saved. entries from the dictionary.</param>
        /// <param name="requestOptions">Configure request locally with RequestOptions.</param>
        /// <returns></returns>
        DictionaryResponse ReplaceDictionaryEntries(Dictionary dictionary, List<DictionaryEntry> dictionaryEntries, RequestOptions requestOptions = null);

        /// <summary>
        /// Replace dictionary entries.
        /// </summary>
        /// <param name="dictionary">Target dictionary.</param>
        /// <param name="dictionaryEntries">Dictionary entries to be saved. entries from the dictionary.</param>
        /// <param name="requestOptions">Configure request locally with RequestOptions.</param>
        /// <param name="ct">Cancelation token.</param>
        /// <returns></returns>
        Task<DictionaryResponse> ReplaceDictionaryEntriesAsync(Dictionary dictionary, List<DictionaryEntry> dictionaryEntries, RequestOptions requestOptions = null, CancellationToken ct = default);

        /// <summary>
        /// Delete dictionary entries.
        /// </summary>
        /// <param name="dictionary">Target dictionary.</param>
        /// <param name="ObjectIDs">List of entries' IDs to delete</param>
        /// <param name="requestOptions">Configure request locally with RequestOptions.</param>
        /// <returns></returns>
        DictionaryResponse DeleteDictionaryEntries(Dictionary dictionary, List<String> ObjectIDs, RequestOptions requestOptions = null);

        /// <summary>
        /// Delete dictionary entries.
        /// </summary>
        /// <param name="dictionary">Target dictionary.</param>
        /// <param name="ObjectIDs">List of entries' IDs to delete</param>
        /// <param name="requestOptions">Configure request locally with RequestOptions.</param>
        /// <param name="ct">Cancelation token.</param>
        /// <returns></returns>
        Task<DictionaryResponse> DeleteDictionaryEntriesAsync(Dictionary dictionary, List<String> ObjectIDs, RequestOptions requestOptions = null, CancellationToken ct = default);

        /// <summary>
        /// Clear all dictionary entries.
        /// </summary>
        /// <param name="dictionary">Target dictionary.</param>
        /// <param name="requestOptions">Configure request locally with RequestOptions.</param>
        /// <returns></returns>
        DictionaryResponse ClearDictionaryEntries(Dictionary dictionary, RequestOptions requestOptions = null);

        /// <summary>
        /// Clear all dictionary entries.
        /// </summary>
        /// <param name="dictionary">Target dictionary.</param>
        /// <param name="requestOptions">Configure request locally with RequestOptions.</param>
        /// <param name="ct">Cancelation token.</param>
        /// <returns></returns>
        Task<DictionaryResponse> ClearDictionaryEntriesAsync(Dictionary dictionary, RequestOptions requestOptions = null, CancellationToken ct = default);

        /// <summary>
        /// Search the dictionary entries.
        /// </summary>
        /// <param name="dictionary">Target dictionary.</param>
        /// <param name="query">The Query used to search./param>
        /// <param name="requestOptions">Configure request locally with RequestOptions.</param>
        /// <typeparam name="T">Type of the data to send/retrieve</typeparam>
        /// <returns></returns>
        SearchResponse<T> SearchDictionaryEntries<T>(Dictionary dictionary, Query query, RequestOptions requestOptions = null) where T : class;

        /// <summary>
        /// Search the dictionary entries.
        /// </summary>
        /// <param name="dictionary">Target dictionary.</param>
        /// <param name="query">The Query used to search.</param>
        /// <param name="requestOptions">Configure request locally with RequestOptions.</param>
        /// <param name="ct">Cancelation token.</param>
        /// <typeparam name="T">Type of the data to send/retrieve</typeparam>
        /// <returns></returns>
        Task<SearchResponse<T>> SearchDictionaryEntriesAsync<T>(Dictionary dictionary, Query query, RequestOptions requestOptions = null, CancellationToken ct = default) where T : class;

        /// <summary>
        /// Update dictionary settings. Only specified settings are overridden; unspecified settings are 
        /// left unchanged. Specifying `null` for a setting resets it to its default value.
        /// </summary>
        /// <param name="dictionarySettings">Settings to be applied.</param>
        /// <param name="requestOptions">Configure request locally with RequestOptions.</param>
        /// <returns></returns>
        DictionaryResponse SetDictionarySettings(DictionarySettings dictionarySettings, RequestOptions requestOptions = null);

        /// <summary>
        /// Update dictionary settings. Only specified settings are overridden; unspecified settings are 
        /// left unchanged. Specifying `null` for a setting resets it to its default value.
        /// </summary>
        /// <param name="dictionarySettings">Settings to be applied.</param>
        /// <param name="requestOptions">Configure request locally with RequestOptions.</param>
        /// <param name="ct">Cancelation token.</param>
        /// <returns></returns>
        Task<DictionaryResponse> SetDictionarySettingsAsync(DictionarySettings dictionarySettings, RequestOptions requestOptions = null, CancellationToken ct = default);

        /// <summary>
        /// Retrieve dictionaries settings.
        /// </summary>
        /// <param name="requestOptions">Configure request locally with RequestOptions.</param>
        /// <returns></returns>
        DictionarySettings GetDictionarySettings(RequestOptions requestOptions = null);

        /// <summary>
        /// Retrieve dictionaries settings.
        /// </summary>
        /// <param name="requestOptions">Configure request locally with RequestOptions.</param>
        /// <param name="ct">Cancelation token.</param>
        /// <returns></returns>
        Task<DictionarySettings> GetDictionarySettingsAsync(RequestOptions requestOptions = null, CancellationToken ct = default);

        /// <summary>
        /// Wait for a dictionary task to complete before executing the next line of code. All write
        /// operations in Algolia are asynchronous by design.
        /// </summary>
        /// <param name="taskId">taskID returned by Algolia API</param>
        /// <param name="timeToWait"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        void WaitAppTask(long taskId, int timeToWait = 100, RequestOptions requestOptions = null);

        /// <summary>
        /// Wait for a dictionary task to complete before executing the next line of code. All write
        /// operations in Algolia are asynchronous by design.
        /// </summary>
        /// <param name="taskId">taskID returned by Algolia API</param>
        /// <param name="timeToWait"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Cancelation token.</param>

        Task WaitAppTaskAsync(long taskId, int timeToWait = 100, RequestOptions requestOptions = null, CancellationToken ct = default);

        /// <summary>
        /// Get the status of the given dictionary task.
        /// </summary>
        /// <param name="taskId">taskID returned by Algolia API</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        TaskStatusResponse GetAppTask(long taskId, RequestOptions requestOptions = null);

        /// <summary>
        /// Get the status of the given dictionary task.
        /// </summary>
        /// <param name="taskId">taskID returned by Algolia API</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Cancelation token.</param>
        Task<TaskStatusResponse> GetAppTaskAsync(long taskId, RequestOptions requestOptions = null, CancellationToken ct = default);
    }
}
