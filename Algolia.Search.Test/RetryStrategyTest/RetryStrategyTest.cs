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

using Algolia.Search.Models.Enums;
using Algolia.Search.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Algolia.Search.Test.RetryStrategyTest
{
    public class RetryStrategyTest
    {
        [Theory]
        [InlineData(CallType.Read)]
        [InlineData(CallType.Write)]
        [InlineData(CallType.Analytics)]
        public void TestGetTryableHost(CallType callType)
        {
            RetryStrategy retryStrategy = new RetryStrategy("appId");
            var hosts = retryStrategy.GetTryableHost(callType);

            if (callType.HasFlag(CallType.Analytics))
            {
                Assert.True(hosts.Count() == 1);
                Assert.True(hosts.All(h => h.Up));
            }
            else
            {
                Assert.True(hosts.Count() == 4);
                Assert.True(hosts.All(h => h.Up));
            }
        }

        [Theory]
        [InlineData(CallType.Read)]
        [InlineData(CallType.Write)]
        public void TestRetryStrategyResetExpired(CallType callType)
        {
            var commonHosts = new List<StatefulHost> {
            new StatefulHost
            {
                Url = $"-1.algolianet.com",
                Priority = 0,
                Up = true,
                LastUse = DateTime.UtcNow,
                Accept = CallType.Read | CallType.Write,
            },
            new StatefulHost
            {
                Url = $"-2.algolianet.com",
                Priority = 0,
                Up = true,
                LastUse = DateTime.UtcNow,
                Accept = CallType.Read | CallType.Write,
            },
            new StatefulHost
            {
                Url = $"-3.algolianet.com",
                Priority = 0,
                Up = false,
                LastUse = DateTime.UtcNow,
                Accept = CallType.Read | CallType.Write,
            }};

            // TODO

            RetryStrategy retryStrategy = new RetryStrategy("appId", commonHosts);
            var hosts = retryStrategy.GetTryableHost(callType);
            Assert.True(hosts.Where(h => h.Up).Count() == 2);
        }

        [Theory]
        [InlineData(CallType.Read, 500)]
        [InlineData(CallType.Write, 500)]
        [InlineData(CallType.Read, 300)]
        [InlineData(CallType.Write, 300)]
        public void TestRetryStrategyRetriableFailure(CallType callType, int httpErrorCode)
        {
            RetryStrategy retryStrategy = new RetryStrategy("appId");
            var hosts = retryStrategy.GetTryableHost(callType);
            Assert.True(hosts.Where(h => h.Up).Count() == 4);

            RetryOutcomeType decision;
            decision = retryStrategy.Decide(hosts.ElementAt(0), httpErrorCode, false);
            Assert.True(decision.HasFlag(RetryOutcomeType.Retry));

            var updatedHosts = retryStrategy.GetTryableHost(callType);
            Assert.True(updatedHosts.Where(h => h.Up).Count() == 3);
        }

        [Theory]
        [InlineData(CallType.Read, 400)]
        [InlineData(CallType.Write, 400)]
        [InlineData(CallType.Read, 404)]
        [InlineData(CallType.Write, 404)]
        public void TestRetryStrategyFailureDecision(CallType callType, int httpErrorCode)
        {
            RetryStrategy retryStrategy = new RetryStrategy("appId");
            var hosts = retryStrategy.GetTryableHost(callType);

            RetryOutcomeType decision;
            decision = retryStrategy.Decide(hosts.ElementAt(0), httpErrorCode, false);

            Assert.True(decision.HasFlag(RetryOutcomeType.Failure));
        }
    }
}