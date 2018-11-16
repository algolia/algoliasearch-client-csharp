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

using System.Collections.Generic;
using System.Threading.Tasks;
using Algolia.Search.Clients;
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Responses;
using Algolia.Search.Models.Synonyms;
using Algolia.Search.Utils;
using NUnit.Framework;

namespace Algolia.Search.Test.EndToEnd
{
    [TestFixture]
    [Parallelizable]
    public class SynonymsTest
    {
        protected SearchIndex _index;
        private IEnumerable<SynonymTestObject> _objectsToSave;
        private string _indexName;

        [OneTimeSetUp]
        public void Init()
        {
            _indexName = TestHelper.GetTestIndexName("synonyms");
            _index = BaseTest.SearchClient.InitIndex(_indexName);

            _objectsToSave = new List<SynonymTestObject>{
                new SynonymTestObject {Console = "Sony PlayStation <PLAYSTATIONVERSION>"},
                new SynonymTestObject {Console = "Nintendo Switch"},
                new SynonymTestObject {Console = "Nintendo Wii U"},
                new SynonymTestObject {Console = "Nintendo Game Boy Advance"},
                new SynonymTestObject {Console = "Microsoft Xbox"},
                new SynonymTestObject {Console = "Microsoft Xbox 360"},
                new SynonymTestObject {Console = "Microsoft Xbox One"}
            };
        }

        [Test]
        public async Task SynonymsOperationsTest()
        {
            BatchResponse addObjectResponse = await _index.AddObjectsAysnc(_objectsToSave);
            addObjectResponse.Wait();

            Synonym regularSynonym = new Synonym
            {
                ObjectID = "gba",
                Type = SynonymType.Synonym,
                Synonyms = new List<string> { "gba", "gameboy advance", "game boy advance" }
            };

            var regularSynonymResponse = await _index.SaveSynonymAsync(regularSynonym.ObjectID, regularSynonym);
            regularSynonymResponse.Wait();

            Synonym wiiToWiiu = new Synonym
            {
                ObjectID = "wii_to_wii_u",
                Type = SynonymType.OneWaySynonym,
                Input = "wii",
                Synonyms = new List<string> { "wii u" }
            };

            Synonym playstationPlaceholder = new Synonym
            {
                ObjectID = "playstation_version_placeholder",
                Type = SynonymType.Placeholder,
                Placeholder = "<PLAYSTATIONVERSION>",
                Replacements = new List<string> { "1", "One", "2", "3", "4", "4 Pro" }
            };

            Synonym ps4 = new Synonym
            {
                ObjectID = "ps4",
                Type = SynonymType.AltCorrection1,
                Word = "ps4",
                Corrections = new List<string> { "playstation4" }
            };
            
            Synonym psone = new Synonym
            {
                ObjectID = "psone",
                Type = SynonymType.AltCorrection2,
                Word = "psone",
                Corrections = new List<string> { "playstationone" }
            };

            List<Synonym> synonyms = new List<Synonym> { wiiToWiiu, playstationPlaceholder, ps4, psone };

            var saveSynonymsResponse = await _index.SaveSynonymsAsync(synonyms);
            saveSynonymsResponse.Wait();

            SearchResponse<Synonym> searchResponse = await _index.SearchSynonymsAsync(new SynonymQuery { HitsPerPage = 10, Page = 0 });
            Assert.True(searchResponse.Hits.Count == 5);

            var deleteGbaResponse = await _index.DeleteSynonymAsync("gba");
            deleteGbaResponse.Wait();

            // Catching 404
            Assert.ThrowsAsync<AlgoliaApiException>(() => _index.GetSynonymAsync("gba"));

            var clearSynonymResponse = await _index.ClearSynonymsAsync();
            clearSynonymResponse.Wait();

            SearchResponse<Synonym> searchAfterClearResponse = await _index.SearchSynonymsAsync(new SynonymQuery { HitsPerPage = 10, Page = 0 });
            Assert.True(searchAfterClearResponse.Hits.Count == 0);
        }

        public class SynonymTestObject
        {
            public string ObjectID { get; set; }
            public string Console { get; set; }
        }
    }
}