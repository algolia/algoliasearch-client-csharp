/*
* Copyright (c) 2018 Algolia
* http://www.algolia.com/
* Based on the first version developed by Christopher Maneu under the same license:
*  https://github.com/cmaneu/algoliasearch-client-csharp
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

using Algolia.Search.Http;
using Algolia.Search.Iterators;
using Algolia.Search.Models.Requests;
using Algolia.Search.Models.Responses;
using Algolia.Search.Models.Settings;
using Algolia.Search.Utils;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Clients
{
    /// <summary>
    /// Client to perfom cross indices operations
    /// </summary>
    public class AccountClient : IAccountClient
    {
        /// <summary>
        /// The method copy settings, synonyms, rules and objects from the source index to the destination index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceIndex"></param>
        /// <param name="destinationIndex"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public MultiResponse CopyIndex<T>(ISearchIndex sourceIndex, ISearchIndex destinationIndex, RequestOptions requestOptions = null) where T : class =>
                     AsyncHelper.RunSync(() => CopyIndexAsync<T>(sourceIndex, destinationIndex, requestOptions));

        /// <summary>
        /// The method copy settings, synonyms, rules and objects from the source index to the destination index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceIndex"></param>
        /// <param name="destinationIndex"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<MultiResponse> CopyIndexAsync<T>(ISearchIndex sourceIndex, ISearchIndex destinationIndex, RequestOptions requestOptions = null,
                            CancellationToken ct = default(CancellationToken)) where T : class
        {
            if (sourceIndex.Config.AppId.Equals(destinationIndex.Config.AppId))
            {
                throw new AlgoliaException("Source and Destination indices should not be on the same application.");
            }

            IndexSettings destinationSettings = await destinationIndex.GetSettingsAsync(ct: ct).ConfigureAwait(false);

            if (destinationSettings != null)
            {
                throw new AlgoliaException("Destination index already exists. Please delete it before copying index across applications.");
            }

            MultiResponse ret = new MultiResponse { Responses = new List<IAlgoliaWaitableResponse>() };

            // Save settings
            IndexSettings sourceSettings = await sourceIndex.GetSettingsAsync(ct: ct).ConfigureAwait(false);
            SetSettingsResponse destinationSettingsResp = await destinationIndex.SetSettingsAsync(sourceSettings, requestOptions, ct).ConfigureAwait(false);
            ret.Responses.Add(destinationSettingsResp);

            // Save synonyms
            SynonymsIterator sourceSynonyms = new SynonymsIterator(sourceIndex);
            SaveSynonymResponse destinationSynonymResponse = await destinationIndex.SaveSynonymsAsync(sourceSynonyms, requestOptions: requestOptions, ct: ct).ConfigureAwait(false);
            ret.Responses.Add(destinationSynonymResponse);

            // Save rules
            RulesIterator sourceRules = new RulesIterator(sourceIndex);
            BatchResponse destinationRuleResponse = await destinationIndex.SaveRulesAsync(sourceRules, requestOptions: requestOptions, ct: ct).ConfigureAwait(false);
            ret.Responses.Add(destinationRuleResponse);

            // Save objects (batched)
            IndexIterator<T> indexIterator = sourceIndex.Browse<T>(new BrowseIndexQuery());
            BatchIndexingResponse saveObject = await destinationIndex.AddObjectsAysnc(indexIterator, requestOptions, ct).ConfigureAwait(false);
            ret.Responses.Add(saveObject);

            return ret;
        }
    }
}