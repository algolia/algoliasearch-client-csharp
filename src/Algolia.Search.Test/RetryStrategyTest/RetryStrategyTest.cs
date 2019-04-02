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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Test.RetryStrategyTest
{
    [TestFixture]
    [Parallelizable]
    public class RetryStrategyTest
    {
        [TestCase(CallType.Read)]
        [TestCase(CallType.Write)]
        [Parallelizable]
        public void TestRetryStrategyResetExpired(CallType callType)
        {
            var commonHosts = new List<StatefulHost>
            {
                new StatefulHost
                {
                    Url = "-1.algolianet.com",
                    Up = true,
                    LastUse = DateTime.UtcNow,
                    Accept = CallType.Read | CallType.Write,
                },
                new StatefulHost
                {
                    Url = "-2.algolianet.com",
                    Up = true,
                    LastUse = DateTime.UtcNow,
                    Accept = CallType.Read | CallType.Write,
                },
                new StatefulHost
                {
                    Url = "-3.algolianet.com",
                    Up = false,
                    LastUse = DateTime.UtcNow,
                    Accept = CallType.Read | CallType.Write,
                }
            };

            SearchConfig config = new SearchConfig(TestHelper.ApplicationId1, TestHelper.AdminKey1)
            {
                CustomHosts = commonHosts
            };

            // TODO
            RetryStrategy retryStrategy = new RetryStrategy(config);
            var hosts = retryStrategy.GetTryableHost(callType);
            Assert.True(hosts.Count(h => h.Up) == 2);
        }

        [TestCase(CallType.Read, 500)]
        [TestCase(CallType.Write, 500)]
        [TestCase(CallType.Read, 300)]
        [TestCase(CallType.Write, 300)]
        [Parallelizable]
        public void TestRetryStrategyRetriableFailure(CallType callType, int httpErrorCode)
        {
            var searchConfig = new SearchConfig("appId", "apiKey");
            RetryStrategy retryStrategy = new RetryStrategy(searchConfig);

            var hosts = retryStrategy.GetTryableHost(callType);
            Assert.True(hosts.Count(h => h.Up) == 4);

            var decision = retryStrategy.Decide(hosts.ElementAt(0), httpErrorCode, false);
            Assert.True(decision.HasFlag(RetryOutcomeType.Retry));

            var updatedHosts = retryStrategy.GetTryableHost(callType);
            Assert.True(updatedHosts.Count(h => h.Up) == 3);
        }

        [TestCase(CallType.Read, 400)]
        [TestCase(CallType.Write, 400)]
        [TestCase(CallType.Read, 404)]
        [TestCase(CallType.Write, 404)]
        [Parallelizable]
        public void TestRetryStrategyFailureDecision(CallType callType, int httpErrorCode)
        {
            var searchConfig = new SearchConfig("appId", "apiKey");
            RetryStrategy retryStrategy = new RetryStrategy(searchConfig);

            var hosts = retryStrategy.GetTryableHost(callType);

            var decision = retryStrategy.Decide(hosts.ElementAt(0), httpErrorCode, false);

            Assert.True(decision.HasFlag(RetryOutcomeType.Failure));
        }

        [TestCase(CallType.Read)]
        [Parallelizable]
        public void TestRetryStrategyMultiThread(CallType callType)
        {
            var searchConfig = new SearchConfig("appId", "apiKey");
            RetryStrategy retryStrategy = new RetryStrategy(searchConfig);

            var initialHosts = retryStrategy.GetTryableHost(callType);
            Assert.True(initialHosts.Count() == 4);

            Task task1 = Task.Run(() =>
            {
                var hosts = retryStrategy.GetTryableHost(callType);
                retryStrategy.Decide(hosts.ElementAt(0), 200, false);
                Console.WriteLine(Thread.CurrentThread.Name);
            });

            Task task2 = Task.Run(() =>
            {
                var hosts = retryStrategy.GetTryableHost(callType);
                retryStrategy.Decide(hosts.ElementAt(0), 500, false);
            });

            Task.WaitAll(task1, task2);

            var updatedHosts = retryStrategy.GetTryableHost(callType);
            Assert.True(updatedHosts.Count() == 3);
        }
    }
}
