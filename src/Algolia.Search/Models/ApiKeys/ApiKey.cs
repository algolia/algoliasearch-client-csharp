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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Algolia.Search.Models.ApiKeys
{
    /// <summary>
    /// Algolia's API Key
    /// </summary>
    public class ApiKey
    {
        /// <summary>
        /// Api Key
        /// </summary>
        [JsonIgnore]
        public string Key { get; set; }

        /// <summary>
        /// Key Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Set of permissions associated to the key.
        /// </summary>
        public IEnumerable<string> Acl { get; set; }

        /// <summary>
        ///  A Unix timestamp used to define the expiration date of the API key.
        /// </summary>
        public long? Validity { get; set; }

        /// <summary>
        /// Specify the maximum number of hits this API key can retrieve in one call.
        /// This parameter can be used to protect you from attempts at retrieving your entire index contents by massively querying the index.
        /// </summary>
        public int? MaxHitsPerQuery { get; set; }

        /// <summary>
        /// Specify the maximum number of API calls allowed from an IP address per hour. Each time an API call is performed with this key, a check is performed.
        /// </summary>
        public int? MaxQueriesPerIPPerHour { get; set; }

        /// <summary>
        /// Specify the list of targeted indices. You can target all indices starting with a prefix or ending with a suffix using the ‘*’ character.
        /// </summary>
        public IEnumerable<string> Indexes { get; set; }

        /// <summary>
        /// Specify the list of referers. You can target all referers starting with a prefix, ending with a suffix using the ‘*’ character.
        /// </summary>
        public IEnumerable<string> Referers { get; set; }

        /// <summary>
        /// IPv4 network allowed to use the generated key. This is used for more protection against API key leaking and reuse.
        /// </summary>
        public string RestrictSources { get; set; }

        /// <summary>
        /// Specify the list of query parameters. You can force the query parameters for a query using the url string format.
        /// </summary>
        public string QueryParameters { get; set; }

        /// <summary>
        /// Specify a description of the API key. Used for informative purposes only. It has impact on the functionality of the API key.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Date of creation
        /// </summary>
        [JsonConverter(typeof(DateTimeEpochConverter))]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// ToString()
        /// </summary>
        public override string ToString()
        {
            return
                $"{Value}{string.Join(",", Acl)}{MaxHitsPerQuery}{MaxQueriesPerIPPerHour}{string.Join(",", Indexes)}{string.Join(",", Referers)}{RestrictSources}{QueryParameters}{Description}";
        }
    }
}