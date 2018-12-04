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

using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Rules;
using Algolia.Search.Serializer;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Algolia.Search.Test.Serializer
{
    [TestFixture]
    [Parallelizable]
    public class SerializerTest
    {
        [Test]
        public void TestAutomaticFacetFilters()
        {
            string json = "[\"lastname\",\"firstname\"]";

            List<AutomaticFacetFilter> deserialized = JsonConvert.DeserializeObject<List<AutomaticFacetFilter>>(json, new AutomaticFacetFiltersConverter());

            Assert.True(deserialized.ElementAt(0).Facet.Equals("lastname"));
            Assert.False(deserialized.ElementAt(0).Disjunctive);
            Assert.IsNull(deserialized.ElementAt(0).Score);

            Assert.True(deserialized.ElementAt(1).Facet.Equals("firstname"));
            Assert.False(deserialized.ElementAt(1).Disjunctive);
            Assert.IsNull(deserialized.ElementAt(1).Score);
        }

        [Test]
        public void TestEditConverter()
        {
            string json = "[\"lastname\",\"firstname\"]";

            List<Edit> deserialized = JsonConvert.DeserializeObject<List<Edit>>(json, new EditConverter());

            Assert.True(deserialized.ElementAt(0).Delete.Equals("lastname"));
            Assert.True(deserialized.ElementAt(0).Type.Equals(EditType.Remove));
            Assert.IsNull(deserialized.ElementAt(0).Insert);

            Assert.True(deserialized.ElementAt(1).Delete.Equals("firstname"));
            Assert.True(deserialized.ElementAt(1).Type.Equals(EditType.Remove));
            Assert.IsNull(deserialized.ElementAt(1).Insert);
        }
    }
}
