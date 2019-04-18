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
using Algolia.Search.Exceptions;
using Algolia.Search.Iterators;
using Algolia.Search.Models.Common;
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Search;
using Algolia.Search.Models.Synonyms;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Algolia.Search.Test.EndToEnd.Index
{
    [TestFixture]
    [Parallelizable]
    public class SynonymsTest
    {
        private SearchIndex _index;
        private IEnumerable<SynonymTestObject> _objectsToSave;
        private string _indexName;

        [OneTimeSetUp]
        public void Init()
        {
            _indexName = TestHelper.GetTestIndexName("synonyms");
            _index = BaseTest.SearchClient.InitIndex(_indexName);

            _objectsToSave = new List<SynonymTestObject>
            {
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
            BatchIndexingResponse addObjectResponse =
                await _index.SaveObjectsAsync(_objectsToSave, autoGenerateObjectId: true);
            addObjectResponse.Wait();

            Synonym gba = new Synonym
            {
                ObjectID = "gba",
                Type = SynonymType.Synonym,
                Synonyms = new List<string> { "gba", "gameboy advance", "game boy advance" }
            };

            var regularSynonymResponse = await _index.SaveSynonymAsync(gba);

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

            regularSynonymResponse.Wait();
            saveSynonymsResponse.Wait();

            // Retrieve the 5 added synonyms with getSynonym and check they are correctly retrieved
            var gbaTask = _index.GetSynonymAsync("gba");
            var wiiTask = _index.GetSynonymAsync("wii_to_wii_u");
            var playstationPlaceholderTask = _index.GetSynonymAsync("playstation_version_placeholder");
            var ps4Task = _index.GetSynonymAsync("ps4");
            var psoneTask = _index.GetSynonymAsync("psone");

            Synonym[] tasks = await Task.WhenAll(gbaTask, wiiTask, playstationPlaceholderTask, ps4Task, psoneTask);
            Assert.True(TestHelper.AreObjectsEqual(gba, tasks[0]));
            Assert.True(TestHelper.AreObjectsEqual(wiiToWiiu, tasks[1]));
            Assert.True(TestHelper.AreObjectsEqual(playstationPlaceholder, tasks[2]));
            Assert.True(TestHelper.AreObjectsEqual(ps4, tasks[3]));
            Assert.True(TestHelper.AreObjectsEqual(psone, tasks[4]));

            // Perform a synonym search using searchSynonyms with an empty query, page 0 and hitsPerPage set to 10 and check that the returned synonyms are the same as the 5 originally saved
            SearchResponse<Synonym> searchResponse =
                await _index.SearchSynonymsAsync(new SynonymQuery { HitsPerPage = 10, Page = 0 });
            Assert.True(searchResponse.Hits.Count == 5);

            // Instantiate a new SynonymIterator using newSynonymIterator and iterate over all the synonyms and check that those collected synonyms are the same as the 5 originally saved
            List<Synonym> synonymsFromIterator = new List<Synonym>();

            foreach (var synonym in new SynonymsIterator(_index))
            {
                synonymsFromIterator.Add(synonym);
            }

            Assert.True(TestHelper.AreObjectsEqual(gba, synonymsFromIterator.Find(s => s.ObjectID.Equals("gba"))));
            Assert.True(TestHelper.AreObjectsEqual(wiiToWiiu,
                synonymsFromIterator.Find(s => s.ObjectID.Equals("wii_to_wii_u"))));
            Assert.True(TestHelper.AreObjectsEqual(playstationPlaceholder,
                synonymsFromIterator.Find(s => s.ObjectID.Equals("playstation_version_placeholder"))));
            Assert.True(TestHelper.AreObjectsEqual(ps4, synonymsFromIterator.Find(s => s.ObjectID.Equals("ps4"))));
            Assert.True(TestHelper.AreObjectsEqual(psone, synonymsFromIterator.Find(s => s.ObjectID.Equals("psone"))));

            // Delete the synonym with objectID=”gba” using deleteSynonym and wait for the task to terminate using waitTask with the returned taskID
            var deleteGbaResponse = await _index.DeleteSynonymAsync("gba");
            deleteGbaResponse.Wait();

            // Try to get the synonym with getSynonym with objectID “gba” and check that the synonym does not exist anymore (404)
            AlgoliaApiException ex = Assert.ThrowsAsync<AlgoliaApiException>(() => _index.GetSynonymAsync("gba"));
            Assert.That(ex.HttpErrorCode == 404);

            // Clear all the synonyms using clearSynonyms and wait for the task to terminate using waitTask with the returned taskID
            var clearSynonymResponse = await _index.ClearSynonymsAsync();
            clearSynonymResponse.Wait();

            // Perform a synonym search using searchSynonyms with an empty query, page 0 and hitsPerPage set to 10 and check that the number of returned synonyms is equal to 0
            SearchResponse<Synonym> searchAfterClearResponse =
                await _index.SearchSynonymsAsync(new SynonymQuery { HitsPerPage = 10, Page = 0 });
            Assert.True(searchAfterClearResponse.Hits.Count == 0);
        }

        public class SynonymTestObject
        {
            public string ObjectID { get; set; }
            public string Console { get; set; }
        }
    }
}