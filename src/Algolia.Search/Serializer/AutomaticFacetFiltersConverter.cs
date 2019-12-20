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
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Algolia.Search.Models.Rules;

namespace Algolia.Search.Serializer
{
    /// <summary>
    /// Custom converter for legacy AutomaticFacetFilters
    /// </summary>
    public class AutomaticFacetFiltersConverter : JsonConverter<List<AutomaticFacetFilter>>
    {
        private static readonly byte[] BytesFacets = Encoding.UTF8.GetBytes("facet");
        private static readonly byte[] BytesDisjunctive = Encoding.UTF8.GetBytes("disjunctive");
        private static readonly byte[] BytesScore = Encoding.UTF8.GetBytes("score");

        /// <summary>
        /// Algolia's specific converter to handle this specific object that could be a List of string or AutomaticFacetFilter
        /// </summary>
        public override List<AutomaticFacetFilter> Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            List<AutomaticFacetFilter> ret = null;

            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    break;
                case JsonTokenType.StartArray:
                    ret = ReadArray(ref reader);
                    break;
                default:
                    throw new JsonException(
                        $"Error while reading Token {reader.GetString()} of type {reader.TokenType}.");
            }

            return ret;
        }

        /// <summary>
        /// Using the default writer because only serialization needs custom implementation.
        /// </summary>
        public override void Write(Utf8JsonWriter writer, List<AutomaticFacetFilter> value,
            JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }

        private static List<AutomaticFacetFilter> ReadArray(ref Utf8JsonReader reader)
        {
            List<AutomaticFacetFilter> ret = new List<AutomaticFacetFilter>();

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.EndArray:
                        return ret;
                    case JsonTokenType.StartObject:
                        ret.Add(ParseAutomaticFacetFilter(ref reader));
                        break;
                    case JsonTokenType.String:
                        ret.Add(new AutomaticFacetFilter { Facet = reader.GetString(), Disjunctive = false });
                        break;
                    default:
                        throw new JsonException(
                            $"Unexpected Token{reader.TokenType} for AutomaticFacetFilter.");
                }
            }

            return ret;
        }

        private static AutomaticFacetFilter ParseAutomaticFacetFilter(ref Utf8JsonReader reader)
        {
            var automaticFacetFilter = new AutomaticFacetFilter();

            while (reader.Read()
                   && reader.TokenType != JsonTokenType.EndObject)
            {
                var itemPropertyName = reader.ValueSpan;

                if (itemPropertyName.SequenceEqual(BytesFacets))
                {
                    reader.Read();
                    automaticFacetFilter.Facet = reader.GetString();
                }
                else if (itemPropertyName.SequenceEqual(BytesDisjunctive))
                {
                    reader.Read();
                    automaticFacetFilter.Disjunctive = reader.GetBoolean();
                }
                else if (itemPropertyName.SequenceEqual(BytesScore))
                {
                    reader.Read();
                    bool success = reader.TryGetInt32(out var i);
                    automaticFacetFilter.Score = success ? (int?)i : null;
                }
            }

            return automaticFacetFilter;
        }
    }
}
