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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Algolia.Search.Test")]
namespace Algolia.Search.Transport
{
    /// <summary>
    /// Retry strategy logic in case of error/timeout
    /// </summary>
    internal class RetryStrategy
    {
        /// <summary>
        /// Hosts that will be used by the strategy
        /// Could be default hosts or custom hosts
        /// </summary>
        private readonly List<StatefulHost> _hosts;

        /// <summary>
        /// The synchronization lock for each set RetryStrategy/Transport/Client
        /// </summary>
        /// <returns></returns>
        private readonly object _lock = new object();

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="config">Client's configuration</param>
        public RetryStrategy(AlgoliaConfig config)
        {
            _hosts = config.CustomHosts ?? config.DefaultHosts;
        }

        /// <summary>
        /// Returns the tryable host regarding the retry strategy
        /// </summary>
        /// <param name="callType">The Call type <see cref="Models.Enums.CallType"/></param>
        public IEnumerable<StatefulHost> GetTryableHost(CallType callType)
        {
            lock (_lock)
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
        }

        /// <summary>
        /// Update host's state
        /// </summary>
        /// <param name="tryableHost">A statefull host</param>
        /// <param name="httpResponseCode">An http response code</param>
        /// <param name="isTimedOut">Is the request timed out</param>
        /// <returns></returns>
        public RetryOutcomeType Decide(StatefulHost tryableHost, int httpResponseCode, bool isTimedOut)
        {
            lock (_lock)
            {
                if (!isTimedOut && IsSuccess(httpResponseCode))
                {
                    tryableHost.Up = true;
                    tryableHost.LastUse = DateTime.UtcNow;
                    return RetryOutcomeType.Success;
                }
                else if (!isTimedOut && IsRetryable(httpResponseCode))
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
                    return RetryOutcomeType.Retry;
                }

                return RetryOutcomeType.Failure;
            }
        }

        /// <summary>
        ///  Tells if the response is a success or not
        /// </summary>
        /// <param name="httpResponseCode"></param>
        /// <returns></returns>
        private bool IsSuccess(int httpResponseCode)
        {
            return (int)Math.Floor((decimal)httpResponseCode / 100) == 2;
        }

        /// <summary>
        ///  Tells if the response is retryable or not
        /// </summary>
        /// <param name="httpResponseCode"></param>
        /// <returns></returns>
        private bool IsRetryable(int httpResponseCode)
        {
            return (int)Math.Floor((decimal)httpResponseCode / 100) != 2 &&
                (int)Math.Floor((decimal)httpResponseCode / 100) != 4;
        }

        /// <summary>
        /// Reset the given host
        /// </summary>
        /// <param name="host"></param>
        private void Reset(StatefulHost host)
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
