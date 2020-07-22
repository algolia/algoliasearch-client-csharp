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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Algolia.Search.Exceptions;
using Algolia.Search.Models.Mcm;
using Algolia.Search.Models.Search;
using NUnit.Framework;

namespace Algolia.Search.Test.EndToEnd.Index
{
    [TestFixture]
    [Parallelizable]
    public class MultiClusterManagementTest
    {
        [Test]
        public async Task McmTest()
        {
            IEnumerable<ClustersResponse> listClusters = (await BaseTest.McmClient.ListClustersAsync()).ToList();
            Assert.That(listClusters, Has.Count.GreaterThanOrEqualTo(2));

            string userId = TestHelper.GetMcmUserId() + "-0";
            string userId1 = TestHelper.GetMcmUserId() + "-1";
            string userId2 = TestHelper.GetMcmUserId() + "-2";

            var userIDs = new List<string> { userId, userId1, userId2 };

            await BaseTest.McmClient.AssignUserIdAsync(userId, listClusters.ElementAt(0).ClusterName);
            await BaseTest.McmClient.AssignUserIdsAsync(new List<string> { userId1, userId2 },
                listClusters.ElementAt(0).ClusterName);

            foreach (var user in userIDs)
            {
                WaitUserId(user);
            }

            foreach (var user in userIDs)
            {
                SearchResponse<UserIdResponse> searchResponse =
                    await BaseTest.McmClient.SearchUserIDsAsync(new SearchUserIdsRequest
                    {
                        Query = user,
                        Cluster = listClusters.ElementAt(0).ClusterName,
                        HitsPerPage = 1,
                    });

                Assert.That(searchResponse.Hits, Has.Exactly(1).Items);
                Assert.That(searchResponse.Hits.First().UserID, Is.EqualTo(user));
            }


            ListUserIdsResponse listUserIds = await BaseTest.McmClient.ListUserIdsAsync();

            foreach (var user in userIDs)
            {
                Assert.True(listUserIds.UserIds.Exists(x => x.UserID.Equals(user)));
            }

            TopUserIdResponse topUserIds = await BaseTest.McmClient.GetTopUserIdAsync();
            Assert.That(topUserIds.TopUsers, Is.Not.Empty);

            foreach (var user in userIDs)
            {
                RemoveUserId(user);
            }

            var hasPendingMappings = await BaseTest.McmClient.HasPendingMappingsAsync(true);

            Assert.That(hasPendingMappings, Is.Not.Null);
        }

        private void WaitUserId(string userId)
        {
            try
            {
                BaseTest.McmClient.GetUserId(userId);
            }
            catch (AlgoliaApiException)
            {
                Task.Delay(1000);
                // Loop until we have found the userID
                WaitUserId(userId);
            }
        }

        private void RemoveUserId(string userId)
        {
            try
            {
                BaseTest.McmClient.RemoveUserId(userId);
            }
            catch (AlgoliaApiException)
            {
                // Loop until we don't have Error 400: "Another mapping operation is already running for this userID"
                Task.Delay(1000);
                RemoveUserId(userId);
            }
        }
    }
}
