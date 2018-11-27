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

using Algolia.Search.Models.Rules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Algolia.Search.Serializer
{
    public class AutomaticFacetFiltersConverter : JsonConverter<IEnumerable<AutomaticFacetFilter>>
    {
        public override void WriteJson(JsonWriter writer, IEnumerable<AutomaticFacetFilter> value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<AutomaticFacetFilter> ReadJson(JsonReader reader, Type objectType,
            IEnumerable<AutomaticFacetFilter> existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            if (reader.TokenType != JsonToken.StartArray) return null;

            var ret = new List<AutomaticFacetFilter>();

            var tokens = JToken.Load(reader);
            foreach (var token in tokens)
            {
                string facet = token.Type != JTokenType.String ? token.Value<string>("facet") : token.Value<string>();
                bool disjunctive = token.Type != JTokenType.String && token.Value<bool>("disjunctive");
                int? score = token.Type != JTokenType.String ? token.Value<int?>("score") : null;

                ret.Add(new AutomaticFacetFilter { Facet = facet, Disjunctive = disjunctive, Score = score });
            }

            return ret;
        }

        public override bool CanWrite => false;
    }
}
