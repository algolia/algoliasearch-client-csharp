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

using Algolia.Search.Clients;
using Algolia.Search.Exceptions;
using Algolia.Search.Models.ApiKeys;
using Algolia.Search.Models.Search;
using Algolia.Search.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Algolia.Search.Test.EndToEnd.ApiKeys
{
    [TestFixture]
    [Parallelizable]
    public class SecuredApiKeyTest
    {
        private SearchIndex _index1;
        private SearchIndex _index2;
        private string _index1Name;
        private string _index2Name;

        [OneTimeSetUp]
        public void Init()
        {
            _index1Name = TestHelper.GetTestIndexName("secured_api_keys");
            _index2Name = TestHelper.GetTestIndexName("secured_api_keys_dev");
            _index1 = BaseTest.SearchClient.InitIndex(_index1Name);
            _index2 = BaseTest.SearchClient.InitIndex(_index2Name);
        }

        [Test]
        public async Task TestApiKey()
        {
            var addOne = await _index1.SaveObjectAsync(new SecuredApiKeyStub { ObjectID = "one" });
            var addTwo = await _index2.SaveObjectAsync(new SecuredApiKeyStub { ObjectID = "one" });

            addOne.Wait();
            addTwo.Wait();

            SecuredApiKeyRestriction restriction = new SecuredApiKeyRestriction
            {
                ValidUntil = DateTime.UtcNow.AddMinutes(10).ToUnixTimeSeconds(),
                RestrictIndices = new List<string> { _index1Name }
            };

            string key = BaseTest.SearchClient.GenerateSecuredApiKeys(TestHelper.SearchKey1, restriction);

            SearchClient clientWithRestriciton = new SearchClient(TestHelper.ApplicationId1, key);
            SearchIndex index1WithoutRestriction = clientWithRestriciton.InitIndex(_index1Name);
            SearchIndex index2WithRestriction = clientWithRestriciton.InitIndex(_index2Name);

            await index1WithoutRestriction.SearchAsync<SecuredApiKeyStub>(new Query());
            AlgoliaApiException ex = Assert.ThrowsAsync<AlgoliaApiException>(() =>
                index2WithRestriction.SearchAsync<SecuredApiKeyStub>(new Query()));

            Assert.That(ex.Message.Contains("Index not allowed with this API key"));
            Assert.That(ex.HttpErrorCode == 403);
        }

        public class SecuredApiKeyStub
        {
            public string ObjectID { get; set; }
        }
    }
}