/*
* Copyright (c) 2021 Algolia
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
using System;
using System.Threading.Tasks;
using Algolia.Search.Clients;
using Algolia.Search.Models.Recommend;
using NUnit.Framework;

namespace Algolia.Search.Test.EndToEnd.Index
{
    [TestFixture]
    [Parallelizable]
    public class RecommendTest
    {
        [OneTimeSetUp]
        public void Init()
        {
        }

        [Test]
        public void TestRecommend()
        {
            RecommendClient recommendClient = new RecommendClient("", "");

            var recos = recommendClient.GetRecommendations<RecommendedProduct>(new List<RecommendOptions> {
              new RecommendOptions {
                IndexName = "",
                ObjectID = "",
                // MaxRecommendations = 3,
                Model = "bought-together",
              }
            });
            foreach (var result in recos.Results) {
              foreach (var hit in result.Hits) {
                Console.WriteLine(hit.Score);
                Console.WriteLine(hit.Name);
                Console.WriteLine(hit.ObjectID);
              }
            }
        }

        [Test]
        public async Task TestRecommendAsync()
        {
            RecommendClient recommendClient = new RecommendClient("", "");

            var recos = await recommendClient.GetRecommendationsAsync<RecommendedProduct>(new List<RecommendOptions> {
              new RecommendOptions {
                IndexName = "",
                ObjectID = "",
                // MaxRecommendations = 3,
                Model = "bought-together",
              }
            });
            foreach (var result in recos.Results) {
              foreach (var hit in result.Hits) {
                Console.WriteLine(hit.Score);
                Console.WriteLine(hit.Name);
                Console.WriteLine(hit.ObjectID);
              }
            }
        }

        public class Product
        {
            public string ObjectID { get; set; }
            public string Name { get; set; }
            public string Category { get; set; }
            public float Price { get; set; }
        }

        public class RecommendedProduct : Product, IRecommendHit {
          public float Score { get; set; }
        }
    }
}
