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

using Algolia.Search.Exceptions;
using Algolia.Search.Models.Mcm;
using Algolia.Search.Models.Search;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

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
            Assert.True(listClusters.Count() >= 2);

            string userId = TestHelper.GetMcmUserId();
            AssignUserIdResponse assignResponse =
                await BaseTest.McmClient.AssignUserIdAsync(userId, listClusters.ElementAt(0).ClusterName);
            WaitUserId(userId);

            SearchResponse<UserIdResponse> searchResponse =
                await BaseTest.McmClient.SearchUserIDsAsync(new SearchUserIdsRequest
                { Query = userId, Cluster = listClusters.ElementAt(0).ClusterName });
            Assert.True(searchResponse.NbHits == 1);

            ListUserIdsResponse listUserIds = await BaseTest.McmClient.ListUserIdsAsync();
            Assert.True(listUserIds.UserIds.Exists(x => x.UserID.Equals(userId)));

            TopUserIdResponse topUserIds = await BaseTest.McmClient.GetTopUserIdAsync();
            Assert.True(topUserIds.TopUsers.Any());

            RemoveUserId(userId);

            ListUserIdsResponse listUserIdsTwo = await BaseTest.McmClient.ListUserIdsAsync();
            var yesterday = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            IEnumerable<UserIdResponse> userIdsToRemove =
                listUserIdsTwo.UserIds.Where(x => x.UserID.Contains($"csharp-{yesterday}"));

            var delete =
                userIdsToRemove.Select(x => BaseTest.McmClient.RemoveUserIdAsync(x.UserID)).ToArray();
            await Task.WhenAll(delete);
        }

        private void WaitUserId(string userId)
        {
            while (true)
            {
                try
                {
                    BaseTest.McmClient.GetUserId(userId);
                }
                catch (AlgoliaApiException ex)
                {
                    // Loop until we have found the userID
                    if (ex.HttpErrorCode == 404)
                    {
                        Task.Delay(1000);
                        continue;
                    }

                    throw;
                }

                break;
            }
        }

        private void RemoveUserId(String userId)
        {
            RemoveUserIdResponse deleteResponse;

            while (true)
            {
                try
                {
                    deleteResponse = BaseTest.McmClient.RemoveUserId(userId);
                }
                catch (AlgoliaApiException ex)
                {
                    // Loop until we don't have Error 400: "Another mapping operation is already running for this userID"
                    if (ex.Message.Contains("Another mapping operation is already running for this userID"))
                    {
                        Task.Delay(1000);
                        continue;
                    }

                    throw;
                }

                break;
            }
        }
    }
}
