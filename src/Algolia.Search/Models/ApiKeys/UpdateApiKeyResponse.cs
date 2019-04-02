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

using Algolia.Search.Models.Common;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Algolia.Search.Models.ApiKeys
{
    /// <summary>
    /// Api's reponse for update api key
    /// </summary>
    public class UpdateApiKeyResponse : IAlgoliaWaitableResponse
    {
        [JsonIgnore] internal Func<string, ApiKey> GetApiKey { get; set; }

        /// <summary>
        /// The updated key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Date of update
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Field used in the wait the method to check that the key was updated
        /// </summary>
        internal ApiKey PendingKey { get; set; }

        /// <summary>
        /// Wait until the key is updated
        /// Can be used for debugging purposes
        /// </summary>
        public void Wait()
        {
            while (true)
            {
                var actualKey = GetApiKey(Key).ToString();

                // When the key on the server equals the key we sent we break the loop
                if (PendingKey.ToString().Equals(actualKey))
                {
                    break;
                }

                Task.Delay(1000);
                continue;
            }
        }
    }
}
