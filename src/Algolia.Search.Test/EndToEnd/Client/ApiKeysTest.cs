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

using Algolia.Search.Models.ApiKeys;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Algolia.Search.Test.EndToEnd.Client
{
    [TestFixture]
    [Parallelizable]
    public class ApiKeysTest
    {
        private string _apiKey;

        [Test]
        public async Task TestApiKeys()
        {
            ApiKey apiKeyToSend = new ApiKey
            {
                Acl = new List<string> { "search" },
                Description = "A description",
                Indexes = new List<string> { "indexes" },
                MaxHitsPerQuery = 1000,
                MaxQueriesPerIPPerHour = 1000,
                QueryParameters = "typoTolerance=strict",
                Referers = new List<string> { "referer" },
                Validity = 600
            };

            AddApiKeyResponse addKeyResponse = await BaseTest.SearchClient.AddApiKeyAsync(apiKeyToSend);
            _apiKey = addKeyResponse.Key;
            apiKeyToSend.Value = _apiKey;
            addKeyResponse.Wait();

            var addedKey = await BaseTest.SearchClient.GetApiKeyAsync(_apiKey);

            Assert.IsTrue(TestHelper.AreObjectsEqual(apiKeyToSend, addedKey, "CreatedAt", "Validity",
                "GetApiKeyDelegate", "Key"));

            ListApiKeysResponse allKeys = await BaseTest.SearchClient.ListApiKeysAsync();
            Assert.IsTrue(allKeys.Keys.Exists(x => x.Value.Equals(_apiKey)));

            apiKeyToSend.MaxHitsPerQuery = 42;
            var updateKey = await BaseTest.SearchClient.UpdateApiKeyAsync(apiKeyToSend);
            updateKey.Wait();

            var getUpdatedKey = await BaseTest.SearchClient.GetApiKeyAsync(_apiKey);

            Assert.IsTrue(getUpdatedKey.MaxHitsPerQuery == 42);

            var deleteApiKey = await BaseTest.SearchClient.DeleteApiKeyAsync(_apiKey);
            deleteApiKey.Wait();

            var restoreAPIKey = await BaseTest.SearchClient.RestoreApiKeyAsync(_apiKey);
            restoreAPIKey.Wait();

            await BaseTest.SearchClient.GetApiKeyAsync(_apiKey);

            await BaseTest.SearchClient.DeleteApiKeyAsync(_apiKey);
        }
    }
}