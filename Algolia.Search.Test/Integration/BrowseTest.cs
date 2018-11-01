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

using Algolia.Search.Iterators;
using Algolia.Search.Models.Query;
using Algolia.Search.Models.Requests;
using Algolia.Search.Models.Rules;
using Algolia.Search.Models.Synonyms;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Algolia.Search.Test.Integration
{
    public class IteratorsTest : BaseTest
    {
        [Fact]
        public void TestBrowseSynonyms()
        {
            List<Synonym> synonyms = new List<Synonym>();

            foreach (var result in new SynonymsIterator(_index, 1))
            {
                synonyms.Add(result);
            }
        }

        [Fact]
        public void TestBrowseRules()
        {
            List<Rule> rules = new List<Rule>();

            foreach (var result in new RulesIterator(_index, 1))
            {
                rules.Add(result);
            }
        }

        [Fact]
        public void TestBrowseAll()
        {
            var browseIndex = new IndexIterator<Actor>(_index, new BrowseIndexQuery { HitsPerPage = 100 });
            List<Actor> actors = new List<Actor>();

            foreach (var result in browseIndex)
            {
                actors.Add(result);
            }
        }
    }
}