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

using Newtonsoft.Json;
using System.Collections.Generic;

namespace Algolia.Search.Models.Common
{
    /// <summary>
    /// https://www.algolia.com/doc/api-reference/api-methods/get-logs/
    /// </summary>
    public class LogResponse
    {
        /// <summary>
        /// List of logs
        /// </summary>
        public IEnumerable<Log> Logs { get; set; }
    }

    /// <summary>
    /// https://www.algolia.com/doc/api-reference/api-methods/get-logs/
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Timestamp in ISO-8601 format.
        /// </summary>
        public string TimeStamp { get; set; }

        /// <summary>
        /// Rest type of the method.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Http response code.
        /// </summary>
        public string AnswerCode { get; set; }

        /// <summary>
        /// Request body. It’s truncated after 1000 characters.
        /// </summary>
        public string QueryBody { get; set; }

        /// <summary>
        /// Answer body. It’s truncated after 1000 characters.
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// Request URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Client ip of the call.
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// SHA1 ID of entry.
        /// </summary>
        public string Sha1 { get; set; }

        /// <summary>
        /// Request Headers (API Key is obfuscated).
        /// </summary>
        [JsonProperty(PropertyName = "query_headers")]
        public string QueryHeaders { get; set; }

        /// <summary>
        /// Number Of Api Calls
        /// </summary>
        [JsonProperty(PropertyName = "nb_api_calls")]
        public string NumberOfApiCalls { get; set; }

        /// <summary>
        /// Processing time for the query. This does not include network time.
        /// </summary>
        [JsonProperty(PropertyName = "processing_time_ms")]
        public string ProcessingTimeMs { get; set; }

        /// <summary>
        /// Number of hits returned for the query.
        /// </summary>
        [JsonProperty(PropertyName = "query_nb_hits")]
        public string NumberOfQueryHits { get; set; }

        /// <summary>
        /// Exhaustive flags used during the query.
        /// </summary>
        public bool? Exhaustive { get; set; }

        /// <summary>
        /// Index name of the log
        /// </summary>
        public string Index { get; set; }
    }
}