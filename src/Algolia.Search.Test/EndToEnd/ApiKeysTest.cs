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

using Algolia.Search.Models.Requests;
using Algolia.Search.Models.Responses;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Algolia.Search.Test.EndToEnd
{
    [TestFixture]
    [Parallelizable]
    public class ApiKeysTest
    {
        private string _apiKey;

        [OneTimeTearDown]
        public void CleanApiKey()
        {
            DeleteApiKeyResponse deleteKey = BaseTest.SearchClient.DeleteApiKey(_apiKey);
        }

        [Test]
        public async Task TestApiKeys()
        {
            ApiKey apiKeyToSend = new ApiKey
            {
                Acl = new List<string> {"search"},
                Description = "A description",
                Indexes = new List<string> {"indexes"},
                MaxHitsPerQuery = 1000,
                MaxQueriesPerIPPerHour = 1000,
                QueryParameters = "typoTolerance=strict",
                Referers = new List<string> {"referer"},
                Validity = 600
            };

            AddApiKeyResponse addKey = await BaseTest.SearchClient.AddApiKeyAsync(apiKeyToSend);
            _apiKey = addKey.Key;
            apiKeyToSend.Value = _apiKey;

            ApiKey addedKey = null;
            addedKey = await BaseTest.SearchClient.GetApiKeyAsync(_apiKey);
            addedKey.Wait();

            Assert.IsTrue(TestHelper.AreObjectsEqual(apiKeyToSend, addedKey, "CreatedAt", "Validity",
                "GetApiKeyDelegate", "Key"));

            ListApiKeysResponse allKeys = await BaseTest.SearchClient.ListApiKeysAsync();
            Assert.IsTrue(allKeys.Keys.Exists(x => x.Value.Equals(_apiKey)));

            apiKeyToSend.MaxHitsPerQuery = 42;
            var updateKey = await BaseTest.SearchClient.UpdateApiKeyAsync(apiKeyToSend);

            ApiKey getUpdatedKey = null;

            // Not wait method on api side, so we have to loop until changes are made.
            while (true)
            {
                getUpdatedKey = await BaseTest.SearchClient.GetApiKeyAsync(_apiKey);
                if (getUpdatedKey.MaxHitsPerQuery == 42)
                {
                    break;
                }

                await Task.Delay(1000);
            }

            Assert.IsTrue(getUpdatedKey.MaxHitsPerQuery == 42);
        }
    }
}