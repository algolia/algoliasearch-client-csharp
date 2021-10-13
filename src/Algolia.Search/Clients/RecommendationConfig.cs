/*
* Copyright (c) 2018-2021 Algolia
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
using Algolia.Search.Models.Enums;
using Algolia.Search.Transport;

namespace Algolia.Search.Clients
{
    /// <summary>
    /// Recommendation client configuration
    /// </summary>
    [Obsolete("Deprecated, use PersonalizationConfig instead.")]
    public sealed class RecommendationConfig : AlgoliaConfig
    {
        /// <summary>
        /// The configuration of the recommendation client
        /// A client should have it's own configuration ie on configuration per client instance
        /// </summary>
        /// <param name="applicationId">Your application ID</param>
        /// <param name="apiKey">Your API Key</param>
        /// <param name="region">Region where your personalization data is stored and processed</param>
        public RecommendationConfig(string applicationId, string apiKey, string region) : base(applicationId, apiKey)
        {
            DefaultHosts = new List<StatefulHost>
            {
                new StatefulHost
                {
                    Url = $"recommendation.{region}.algolia.com", Accept = CallType.Read | CallType.Write
                }
            };

            Compression = CompressionType.NONE;
        }
    }
}
