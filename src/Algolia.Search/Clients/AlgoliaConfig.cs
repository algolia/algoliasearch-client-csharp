/*
* Copyright (c) 2018 Algolia
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

using Algolia.Search.Serializer;
using Algolia.Search.Transport;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Algolia.Search.Clients
{
    /// <summary>
    /// Algolia's client configuration
    /// </summary>
    public class AlgoliaConfig
    {
        private static readonly string _clientVersion =
            typeof(AlgoliaConfig).GetTypeInfo().Assembly.GetName().Version.ToString();

        /// <summary>
        /// Create a new Algolia's configuration for the given credentials
        /// </summary>
        /// <param name="applicationId">Your application ID</param>
        /// <param name="apiKey">Your API Key</param>
        public AlgoliaConfig(string applicationId, string apiKey)
        {
            AppId = applicationId;
            ApiKey = apiKey;

            DefaultHeaders = new Dictionary<string, string>
            {
                {"X-Algolia-Application-Id", AppId},
                {"X-Algolia-API-Key", ApiKey},
                {"User-Agent", $"C# {_clientVersion}"},
                {"Connection", "keep-alive"},
                {"Accept", JsonConfig.JsonContentType}
            };
        }

        /// <summary>
        /// Your application ID
        /// </summary>
        /// <returns></returns>
        public string AppId { get; set; }

        /// <summary>
        /// Your API Key
        /// </summary>
        /// <returns></returns>
        public string ApiKey { get; set; }

        /// <summary>
        /// To set custom hosts
        /// </summary>
        public List<StatefulHost> Hosts { get; set; }

        /// <summary>
        /// Algolia's default headers.
        /// Will be sent for every request
        /// </summary>
        public Dictionary<string, string> DefaultHeaders { get; set; }

        /// <summary>
        /// The batch size for save methods
        /// </summary>
        public int BatchSize { get; set; } = 1000;
    }
}