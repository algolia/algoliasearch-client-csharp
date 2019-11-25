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
using Algolia.Search.Exceptions;
using Algolia.Search.Models.Rules;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Algolia.Search.Serializer
{
    /// <summary>
    /// Custom converter for legacy EDIT objects
    /// </summary>
    public class ConsequenceQueryConverter : JsonConverter<ConsequenceQuery>
    {
        /// <summary>
        /// Custom deserializer to handle polymorphism/legacy of "query" attributes in ConsequenceQuery
        /// </summary>
        /// <example>
        /// <code>
        /// // Raw Query String
        /// "query": "some query string"
        ///
        /// // remove attribute (deprecated)
        ///  "query": { "remove": ["query1", "query2"] }
        ///
        /// // edits attribute
        ///  "query": {
        ///     "edits": [
        ///            { "type": "remove", "delete":s "old"},
        ///            { "type": "replace", "delete": "new", "insert": "newer" } ]
        ///         }
        /// </code>
        /// </example>
        public override ConsequenceQuery ReadJson(JsonReader reader, Type objectType, ConsequenceQuery existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Null:
                    return null;
                case JsonToken.StartArray:
                    throw new AlgoliaException($"Unexpected Array Token in query: {reader.ReadAsString()}");
                case JsonToken.StartObject:
                {
                    JObject items = JObject.Load(reader);
                    var ret = new ConsequenceQuery();
                    var edits = new List<Edit>();

                    if (items["remove"] != null)
                    {
                        var removeList = items["remove"].ToObject<List<string>>();
                        foreach (var remove in removeList)
                        {
                            edits.Add(new Edit { Type = EditType.Remove, Delete = remove });
                        }
                    }

                    if (items["edits"] != null)
                    {
                        var editsList = items["edits"].ToObject<List<Edit>>();
                        foreach (var edit in editsList)
                        {
                            edits.Add(edit);
                        }
                    }

                    ret.Edits = edits;
                    return ret;
                }
                case JsonToken.String:
                    var jValue = new JValue(reader.Value);
                    return new ConsequenceQuery { SearchQuery = (string)jValue.Value };
                default:
                    throw new AlgoliaException($"Unexpected JSON Token in consequenceQuery: {reader.ReadAsString()}");
            }
        }

        /// <summary>
        /// Custom serializer to handle polymorphism/legacy of "query" attributes in ConsequenceQuery
        /// </summary>
        /// <example>
        /// <code>
        /// // Raw Query String
        /// "query": "some query string"
        ///
        /// // remove attribute (deprecated)
        ///  "query": { "remove": ["query1", "query2"] }
        ///
        /// // edits attribute
        ///  "query": {
        ///     "edits": [
        ///            { "type": "remove", "delete":s "old"},
        ///            { "type": "replace", "delete": "new", "insert": "newer" } ]
        ///         }
        /// </code>
        /// </example>
        public override void WriteJson(JsonWriter writer, ConsequenceQuery value, JsonSerializer serializer)
        {
            if (!string.IsNullOrEmpty(value.SearchQuery))
            {
                writer.WriteValue(value.SearchQuery);
            }
            else
            {
                writer.WriteStartObject();
                writer.WritePropertyName("edits");
                serializer.Serialize(writer, value.Edits);
                writer.WriteEndObject();
            }
        }

        /// <inheritdoc />
        public override bool CanWrite => true;
    }
}
