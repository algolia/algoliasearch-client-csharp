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
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Algolia.Search.Serializer
{
    /// <summary>
    /// Custom converter for legacy Filters
    /// </summary>
    public class FiltersConverter : JsonConverter<IEnumerable<IEnumerable<string>>>
    {
        /// <summary>
        /// No need to implement this method as we don't want to override default writer
        /// </summary>
        public override void WriteJson(JsonWriter writer, IEnumerable<IEnumerable<string>> value,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Algolia's specific reader to handle multiple form of legacy filters
        /// This reader is converting single string to nested arrays
        /// This reader is also converting single array to nested arrays
        /// </summary>
        public override IEnumerable<IEnumerable<string>> ReadJson(JsonReader reader, Type objectType,
            IEnumerable<IEnumerable<string>> existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            if (reader.TokenType == JsonToken.StartArray)
            {
                JToken token = JToken.Load(reader);
                var ret = new List<List<string>>();

                foreach (var tokenValue in token)
                {
                    switch (tokenValue.Type)
                    {
                        case JTokenType.Null:
                            break;
                        case JTokenType.String:
                            ret.Add(new List<string> { tokenValue.ToObject<string>() });
                            break;
                        case JTokenType.Array:
                            var jArray = (JArray)tokenValue;
                            ret.Add(jArray.ToObject<List<string>>());
                            break;
                    }
                }

                return ret;
            }

            if (reader.TokenType == JsonToken.String)
                return buildFilters(Convert.ToString(reader.Value)).ToList();

            throw new JsonSerializationException(
                $"Error while reading Token {reader.Value} of type {reader.TokenType}.");
        }

        /** Build filters from (legacy) string */
        private IEnumerable<List<string>> buildFilters(string str)
        {
            // Extract groups: "(A:1,B:2),C:3" -> ["(A:1,B:2)","C:3"]
            var groups = System.Text.RegularExpressions.Regex.Split(str, ",(?![^()]*\\))");
            return groups.Select(group =>
            {
                if (group.StartsWith("(") && group.EndsWith(")"))
                {
                    var input = group.Substring(1, group.Length - 1);
                    return input.Split(',').ToList();
                }
                else
                {
                    return new List<string> { group };
                }
            });
        }

        /// <summary>
        /// Disable write json
        /// </summary>
        public override bool CanWrite => false;
    }
}
