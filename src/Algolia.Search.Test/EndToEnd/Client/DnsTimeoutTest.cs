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
using Algolia.Search.Models.Enums;
using Algolia.Search.Transport;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Algolia.Search.Test.EndToEnd.Client
{
    [TestFixture]
    [Parallelizable]
    [Ignore("Waiting for new spec for dns timeout test")]
    public class DnsTimeoutTest
    {
        private List<StatefulHost> _hosts;

        [OneTimeSetUp]
        public void Init()
        {
            string applicationId = TestHelper.ApplicationId1;
            _hosts = new List<StatefulHost>
            {
                new StatefulHost
                {
                    Url = "algolia.biz",
                    Accept = CallType.Read | CallType.Write,
                },
                new StatefulHost
                {
                    Url = $"{applicationId}-1.algolianet.com",
                    Accept = CallType.Read | CallType.Write,
                },
                new StatefulHost
                {
                    Url = $"{applicationId}-2.algolianet.com",
                    Accept = CallType.Read | CallType.Write,
                },
                new StatefulHost
                {
                    Url = $"{applicationId}-3.algolianet.com",
                    Accept = CallType.Read | CallType.Write,
                }
            };
        }

        [Test]
        public async Task TestTimeOut()
        {
            SearchConfig config = new SearchConfig(TestHelper.ApplicationId1, TestHelper.AdminKey1)
            {
                CustomHosts = _hosts
            };

            SearchClient client = new SearchClient(config);

            Stopwatch timer = new Stopwatch();
            timer.Start();

            for (int i = 0; i < 10; i++)
            {
                _ = await client.ListIndicesAsync();
            }

            timer.Stop();

            Assert.IsTrue(timer.ElapsedMilliseconds < 5000);
        }
    }
}
