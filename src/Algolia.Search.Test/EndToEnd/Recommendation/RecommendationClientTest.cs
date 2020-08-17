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

using System.Collections.Generic;
using Algolia.Search.Exceptions;
using Algolia.Search.Models.Recommendation;
using Algolia.Search.Serializer;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Algolia.Search.Test.EndToEnd.Recommendation
{
    [TestFixture]
    [Parallelizable]
    public class RecommendationClientTest
    {
        [Test]
        [Parallelizable]
        public void TestRecommendationClient()
        {
            var request = new SetStrategyRequest(
                new List<EventsScoring>
                {
                    new EventsScoring("Add to cart", "conversion", 50),
                    new EventsScoring("Purchase", "conversion", 100),
                },
                new List<FacetsScoring>
                {
                    new FacetsScoring("brand", 100),
                    new FacetsScoring("categories", 10),
                },
                0
            );
            
            // The personalization API is now limiting the number of setPersonalizationStrategy()` successful calls
            // to 15 per day. If the 429 error is returned, the response is considered a "success".
            try
            {
                BaseTest.RecommendationClient.SetPersonalizationStrategy(request);
            }
            catch (AlgoliaApiException e) when (e.HttpErrorCode != 429)
            {
                Assert.Fail($"RecommendationClient.SetPersonalizationStrategy failure: HttpErrorCode: {e.HttpErrorCode}, HttpMessage {e.Message}");   
            }
            Assert.DoesNotThrow(() => BaseTest.RecommendationClient.GetPersonalizationStrategy());
        }

        [Test]
        [Parallelizable]
        public void TestSetStrategyPayload()
        {
            var events = new List<EventsScoring>
            {
                new EventsScoring("buy", "conversion", 10), new EventsScoring("add to cart", "conversion", 20)
            };

            var facets = new List<FacetsScoring> { new FacetsScoring("brand", 10), new FacetsScoring("category", 20) };

            var validStrategy = new SetStrategyRequest(events, facets, 75);

            var payload = JsonConvert.SerializeObject(validStrategy, JsonConfig.AlgoliaJsonSerializerSettings);

            Assert.AreEqual(payload,
                "{" +
                "\"eventsScoring\":[{\"eventName\":\"buy\",\"eventType\":\"conversion\",\"score\":10},{\"eventName\":\"add to cart\",\"eventType\":\"conversion\",\"score\":20}]," +
                "\"facetsScoring\":[{\"facetName\":\"brand\",\"score\":10},{\"facetName\":\"category\",\"score\":20}]," +
                "\"personalizationImpact\":75}");
        }
    }
}
