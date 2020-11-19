/*
* Copyright (c) 2020 Algolia
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

using System.Collections.Generic;
using System.Net;
using Algolia.Search.Clients;
using Algolia.Search.Exceptions;
using Algolia.Search.Models.Search;
using Algolia.Search.Transport;
using NUnit.Framework;

namespace Algolia.Search.Test.RetryStrategyTest
{

    [TestFixture]
    [Parallelizable]
    public class ExceptionsTests
    {
        private string _indexName;

        [OneTimeSetUp]
        public void Init()
        {
            _indexName = TestHelper.GetTestIndexName("exception");
        }

        [Test]
        [Parallelizable]
        public void TestRetryStrategyWithWrongUrl()
        {
            List<StatefulHost> wrongUrlHosts = new List<StatefulHost>
            {
                new StatefulHost
                {
                    Url = "wrong",
                    Up = false,
                },
            };

            SearchConfig configWithCustomHosts = new SearchConfig(TestHelper.ApplicationId1, TestHelper.AdminKey1)
            {
                CustomHosts = wrongUrlHosts
            };

            SearchClient clientWithCustomConfig = new SearchClient(configWithCustomHosts);

            Assert.That(Assert.Throws<AlgoliaUnreachableHostException>(()
                => clientWithCustomConfig.InitIndex(_indexName).Search<string>(new Query(""))).Message,
                Is.Not.Empty);
        }
    }
}
