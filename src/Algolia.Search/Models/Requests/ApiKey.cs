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

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Algolia.Search.Exceptions;
using Algolia.Search.Models.Responses;
using Algolia.Search.Serializer;
using Newtonsoft.Json;

namespace Algolia.Search.Models.Requests
{
    public class ApiKey : IAlgoliaWaitableResponse
    {
        [JsonIgnore]
        public Func<string, ApiKey> GetApiKeyDelegate { get; set; }
        [JsonIgnore]
        public string Key { get; set; }
        public string Value { get; set; }
        public IEnumerable<string> Acl { get; set; }
        public long? Validity { get; set; }
        public int? MaxHitsPerQuery { get; set; }
        public int? MaxQueriesPerIPPerHour { get; set; }
        public IEnumerable<string> Indexes { get; set; }
        public IEnumerable<string> Referers { get; set; }
        public string RestrictSources { get; set; }
        public string QueryParameters { get; set; }
        public string Description { get; set; }
        [JsonConverter(typeof(DateTimeEpochSerializer))]
        public DateTime? CreatedAt { get; set; }
        [JsonIgnore]
        public bool Exist { get; set; }

        public void Wait()
        {
            while (true)
            {
                ApiKey retrievedApiKey = GetApiKeyDelegate(Key);

                // loop until the key exists on the api side
                if (retrievedApiKey.Exist)
                {
                    Value = retrievedApiKey.Value;
                    Acl = retrievedApiKey.Acl;
                    Validity = retrievedApiKey.Validity;
                    MaxHitsPerQuery = retrievedApiKey.MaxHitsPerQuery;
                    MaxQueriesPerIPPerHour = retrievedApiKey.MaxQueriesPerIPPerHour;
                    Indexes = retrievedApiKey.Indexes;
                    Referers = retrievedApiKey.Referers;
                    QueryParameters = retrievedApiKey.QueryParameters;
                    Description = retrievedApiKey.Description;
                    CreatedAt = retrievedApiKey.CreatedAt;
                    break;
                }

                Task.Delay(1000);
                continue;
            }
        }
    }
}