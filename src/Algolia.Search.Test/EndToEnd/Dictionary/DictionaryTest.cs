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

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Algolia.Search.Models.Dictionary;
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Search;
using NUnit.Framework;

namespace Algolia.Search.Test.EndToEnd.Client
{
    [TestFixture]
    [Parallelizable]
    public class DictionaryTest
    {
        [Test]
        [Parallelizable]
        public async Task TestStopwordsDictionnaries()
        {
            // Search non-existent
            String objectId = System.Guid.NewGuid().ToString();

            Query query = new Query(objectId);

            AlgoliaDictionary algoliaDictionary = new AlgoliaDictionary
            {
                Name = AlgoliaDictionaryType.Stopwords
            };

            var emptySearchDictionaryResponse = await BaseTest.DictionaryClient.SearchDictionaryEntriesAsync<Stopword>(algoliaDictionary, query);
            Assert.That(emptySearchDictionaryResponse.Hits, Is.Empty);

            // Save Entry
            Stopword stopword = new Stopword()
            {
                ObjectID = objectId,
                Language = "en",
                Word = "upper",
                State = "enabled"
            };

            var saveDictionaryResponse = await BaseTest.DictionaryClient.SaveDictionaryEntriesAsync(algoliaDictionary, new List<DictionaryEntry>() { stopword });
            saveDictionaryResponse.Wait();

            var searchSaveDictionaryResponse = await BaseTest.DictionaryClient.SearchDictionaryEntriesAsync<Stopword>(algoliaDictionary, query);

            Assert.That(searchSaveDictionaryResponse.NbHits, Is.EqualTo(1));
            Assert.AreEqual(searchSaveDictionaryResponse.Hits[0].Word, stopword.Word);

            // Replace Entry
            stopword.Word = "uppercase";
            var replaceDictionaryResponse = await BaseTest.DictionaryClient.ReplaceDictionaryEntriesAsync(algoliaDictionary, new List<DictionaryEntry>() { stopword });
            replaceDictionaryResponse.Wait();

            var searchReplaceDictionaryResponse = await BaseTest.DictionaryClient.SearchDictionaryEntriesAsync<Stopword>(algoliaDictionary, query);

            Assert.That(searchReplaceDictionaryResponse.NbHits, Is.EqualTo(1));
            Assert.AreEqual(searchReplaceDictionaryResponse.Hits[0].Word, stopword.Word);

            // Delete Entry
            var deleteDictionaryResponse = await BaseTest.DictionaryClient.DeleteDictionaryEntriesAsync(algoliaDictionary, new List<string>() { objectId });
            deleteDictionaryResponse.Wait();

            var searchDeleteDictionaryResponse = await BaseTest.DictionaryClient.SearchDictionaryEntriesAsync<Stopword>(algoliaDictionary, query);
            Assert.That(searchDeleteDictionaryResponse.Hits, Is.Empty);
        }

        [Test]
        [Parallelizable]
        public async Task TestPluralsDictionnaries()
        {
            // Search non-existent
            String objectId = System.Guid.NewGuid().ToString();

            Query query = new Query(objectId);

            AlgoliaDictionary algoliaDictionary = new AlgoliaDictionary
            {
                Name = AlgoliaDictionaryType.Plurals
            };

            var searchDictionaryResponse = await BaseTest.DictionaryClient.SearchDictionaryEntriesAsync<Plural>(algoliaDictionary, query);
            Assert.That(searchDictionaryResponse.Hits, Is.Empty);

            // Save Entry
            Plural plural = new Plural()
            {
                ObjectID = objectId,
                Language = "en",
                Words = new List<String>() { "cheval", "chevaux" }
            };

            var saveDictionaryResponse = await BaseTest.DictionaryClient.SaveDictionaryEntriesAsync(algoliaDictionary, new List<DictionaryEntry>() { plural });
            saveDictionaryResponse.Wait();

            var searchSaveDictionaryResponse = await BaseTest.DictionaryClient.SearchDictionaryEntriesAsync<Plural>(algoliaDictionary, query);

            Assert.That(searchSaveDictionaryResponse.NbHits, Is.EqualTo(1));
            Assert.AreEqual(searchSaveDictionaryResponse.Hits[0].Words, plural.Words);

            // Delete Entry
            var deleteDictionaryResponse = await BaseTest.DictionaryClient.DeleteDictionaryEntriesAsync(algoliaDictionary, new List<string>() { objectId });
            deleteDictionaryResponse.Wait();

            var searchDeleteDictionaryResponse = await BaseTest.DictionaryClient.SearchDictionaryEntriesAsync<Stopword>(algoliaDictionary, query);
            Assert.That(searchDeleteDictionaryResponse.Hits, Is.Empty);
        }

        [Test]
        [Parallelizable]
        public async Task TestCompoundsDictionnaries()
        {
            // Search non-existent
            String objectId = System.Guid.NewGuid().ToString();

            Query query = new Query(objectId);

            AlgoliaDictionary algoliaDictionary = new AlgoliaDictionary
            {
                Name = AlgoliaDictionaryType.Compounds
            };

            var searchDictionaryResponse = await BaseTest.DictionaryClient.SearchDictionaryEntriesAsync<Compound>(algoliaDictionary, query);
            Assert.That(searchDictionaryResponse.Hits, Is.Empty);

            // Save Entry
            Compound compound = new Compound()
            {
                ObjectID = objectId,
                Language = "nl",
                Word = "kopfschmerztablette",
                Decomposition = new List<String>() { "kopf", "schmerz", "tablette" }
            };

            var saveDictionaryResponse = await BaseTest.DictionaryClient.SaveDictionaryEntriesAsync(algoliaDictionary, new List<DictionaryEntry>() { compound });
            saveDictionaryResponse.Wait();

            var searchSaveDictionaryResponse = await BaseTest.DictionaryClient.SearchDictionaryEntriesAsync<Compound>(algoliaDictionary, query);

            Assert.That(searchSaveDictionaryResponse.NbHits, Is.EqualTo(1));
            Assert.AreEqual(searchSaveDictionaryResponse.Hits[0].Word, compound.Word);

            // Delete Entry
            var deleteDictionaryResponse = await BaseTest.DictionaryClient.DeleteDictionaryEntriesAsync(algoliaDictionary, new List<string>() { objectId });
            deleteDictionaryResponse.Wait();

            var searchDeleteDictionaryResponse = await BaseTest.DictionaryClient.SearchDictionaryEntriesAsync<Stopword>(algoliaDictionary, query);
            Assert.That(searchDeleteDictionaryResponse.Hits, Is.Empty);
        }

        [Test]
        [Parallelizable]
        public async Task TestSettings()
        {
            DictionarySettings dictionarySettings = new DictionarySettings()
            {
                DisableStandardEntries = new DisableStandardEntries() { Stopwords = new Dictionary<String, Boolean>() { { "en", true } } }
            };

            var SetDictionarySettingsResponse = await BaseTest.DictionaryClient.SetDictionarySettingsAsync(dictionarySettings);
            SetDictionarySettingsResponse.Wait();

            var DictionarySettingsResponse = BaseTest.DictionaryClient.GetDictionarySettings();
            Assert.AreEqual(DictionarySettingsResponse.DisableStandardEntries.Stopwords, dictionarySettings.DisableStandardEntries.Stopwords);
        }
    }
}
