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
using Algolia.Search.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Algolia.Search.Test")]
namespace Algolia.Search.Transport
{
    internal class RetryStrategy : IRetryStrategy
    {
        private List<StateFulHost> _hosts;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="customHosts"></param>
        public RetryStrategy(string applicationId, IEnumerable<StateFulHost> customHosts = null)
        {
            _hosts = new List<StateFulHost>();

            if (customHosts != null && customHosts.Any())
            {
                _hosts.AddRange(customHosts);
                _hosts.ForEach(x => x.Accept = CallType.Read | CallType.Write);
            }
            else
            {
                _hosts.Add(new StateFulHost
                {
                    Url = $"{applicationId}-dsn.algolia.net",
                    Priority = 10,
                    Up = true,
                    LastUse = DateTime.UtcNow,
                    TimeOut = 5,
                    Accept = CallType.Read
                });
                _hosts.Add(new StateFulHost
                {
                    Url = $"{applicationId}.algolia.net",
                    Priority = 10,
                    Up = true,
                    LastUse = DateTime.UtcNow,
                    Accept = CallType.Write,
                    TimeOut = 30
                });

                var commonHosts = new List<StateFulHost> {
                new StateFulHost
                {
                    Url = $"{applicationId}-1.algolianet.com",
                    Priority = 0,
                    Up = true,
                    LastUse = DateTime.UtcNow,
                    Accept = CallType.Read | CallType.Write,
                    TimeOut = 5
                },
                new StateFulHost
                {
                    Url = $"{applicationId}-2.algolianet.com",
                    Priority = 0,
                    Up = true,
                    LastUse = DateTime.UtcNow,
                    Accept = CallType.Read | CallType.Write,
                    TimeOut = 5
                },
                new StateFulHost
                {
                    Url = $"{applicationId}-3.algolianet.com",
                    Priority = 0,
                    Up = true,
                    LastUse = DateTime.UtcNow,
                    Accept = CallType.Read | CallType.Write,
                    TimeOut = 5
                }}.Shuffle();

                _hosts.AddRange(commonHosts);
            }

            _hosts.Add(new StateFulHost
            {
                Url = "analytics.algolia.com",
                Up = true,
                LastUse = DateTime.UtcNow,
                TimeOut = 5,
                Accept = CallType.Analytics
            });
        }

        /// <summary>
        /// Returns the tryable host regarding the retry strategy
        /// </summary>
        /// <param name="callType"></param>
        /// <returns></returns>
        public IEnumerable<StateFulHost> GetTryableHost(CallType callType)
        {
            ResetExpiredHosts();

            if (_hosts.Any(h => h.Up && h.Accept.HasFlag(callType)))
            {
                return _hosts.Where(h => h.Up && h.Accept.HasFlag(callType));
            }
            else
            {
                foreach (var host in _hosts.Where(h => h.Accept.HasFlag(callType)))
                {
                    Reset(host);
                }
                return _hosts;
            }
        }

        /// <summary>
        /// Update host's state 
        /// </summary>
        /// <param name="tryableHost"></param>
        /// <param name="httpResponseCode"></param>
        /// <param name="isTimedOut"></param>
        /// <returns></returns>
        public RetryOutcomeType Decide(StateFulHost tryableHost, int httpResponseCode, bool isTimedOut)
        {
            if (!isTimedOut && (int)Math.Floor((decimal)httpResponseCode / 100) == 2)
            {
                tryableHost.Up = true;
                tryableHost.LastUse = DateTime.UtcNow;
                return RetryOutcomeType.Success;
            }
            else if (!isTimedOut && (((int)Math.Floor((decimal)httpResponseCode / 100) != 2) && ((int)Math.Floor((decimal)httpResponseCode / 100) != 4)))
            {
                tryableHost.Up = false;
                tryableHost.LastUse = DateTime.UtcNow;
                return RetryOutcomeType.Retry;
            }
            else if (isTimedOut)
            {
                tryableHost.Up = true;
                tryableHost.LastUse = DateTime.UtcNow;
                tryableHost.RetryCount++;
                tryableHost.TimeOut *= (tryableHost.RetryCount + 1);
                return RetryOutcomeType.Retry;
            }

            return RetryOutcomeType.Failure;
        }

        /// <summary>
        /// Reset the given host
        /// </summary>
        /// <param name="host"></param>
        private void Reset(StateFulHost host)
        {
            host.Up = true;
            host.RetryCount = 0;
            host.LastUse = DateTime.UtcNow;
        }

        /// <summary>
        /// Reset down host after 5 minutes
        /// </summary>
        private void ResetExpiredHosts()
        {
            foreach (var host in _hosts)
            {
                if (!host.Up && DateTime.UtcNow.Subtract(host.LastUse).Minutes > 5)
                {
                    Reset(host);
                }
            }
        }
    }
}