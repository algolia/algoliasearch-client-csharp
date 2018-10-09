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
using Algolia.Search.Transport;
using System;

namespace Algolia.Search.Clients
{
    public class SearchClient : ISearchClient
    {
        private readonly IRequesterWrapper _requesterWrapper;

        /// <summary>
        /// Initialize a client with default settings
        /// </summary>
        public SearchClient()
        {
            _requesterWrapper = new RequesterWrapper();
        }

        /// <summary>
        /// Create a new search client for the given appID
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="apiKey"></param>
        public SearchClient(string applicationId, string apiKey)
        {
            if (string.IsNullOrEmpty(applicationId))
            {
                throw new ArgumentNullException(nameof(applicationId), "Application ID is required");
            }

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException(nameof(apiKey), "An API key is required");
            }

            _requesterWrapper = new RequesterWrapper(new AlgoliaConfig {ApiKey = apiKey, AppId = applicationId});
        }

        /// <summary>
        /// Initialize a client with custom config
        /// </summary>
        /// <param name="config"></param>
        public SearchClient(AlgoliaConfig config)
        {
            _requesterWrapper = new RequesterWrapper(config);
        }

        /// <summary>
        /// Initialize the client with custom config and custom Requester
        /// </summary>
        /// <param name="config"></param>
        /// <param name="httpRequester"></param>
        public SearchClient(AlgoliaConfig config, IHttpRequester httpRequester)
        {
            _requesterWrapper = new RequesterWrapper(config, httpRequester);
        }

        /// <summary>
        /// Initialize an index for the given client
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public Index InitIndex(string indexName)
        {
            return string.IsNullOrEmpty(indexName)
                ? throw new ArgumentNullException(nameof(indexName), "Index name is required") : new Index(_requesterWrapper, indexName);
        }
    }
}
