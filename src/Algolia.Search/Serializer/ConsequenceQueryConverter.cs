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
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Rules;

namespace Algolia.Search.Serializer
{
    /// <summary>
    /// Custom converter for legacy EDIT objects
    /// </summary>
    public class ConsequenceQueryConverter : JsonConverter<ConsequenceQuery>
    {
        private static readonly byte[] BytesType = Encoding.UTF8.GetBytes("type");
        private static readonly byte[] BytesDelete = Encoding.UTF8.GetBytes("delete");
        private static readonly byte[] BytesInsert = Encoding.UTF8.GetBytes("insert");
        private static readonly byte[] Remove = Encoding.UTF8.GetBytes("remove");
        private static readonly byte[] Edits = Encoding.UTF8.GetBytes("edits");

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
        public override void Write(Utf8JsonWriter writer, ConsequenceQuery value, JsonSerializerOptions options)
        {
            if (!string.IsNullOrEmpty(value.SearchQuery))
            {
                writer.WriteStringValue(value.SearchQuery);
            }
            else
            {
                writer.WriteStartObject();
                writer.WritePropertyName("edits");
                JsonSerializer.Serialize(writer, value.Edits, options);
                writer.WriteEndObject();
            }
        }

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
        public override ConsequenceQuery Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            if (reader.TokenType == JsonTokenType.String)
                return new ConsequenceQuery { SearchQuery = reader.GetString() };

            var ret = new ConsequenceQuery();
            var edits = new List<Edit>();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject &&
                   reader.TokenType != JsonTokenType.Null)
            {
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    continue;
                }

                var property = reader.ValueSpan;

                reader.Read();

                if (property.SequenceEqual(Remove))
                {
                    edits.AddRange(ReadArray(ref reader));
                }

                if (property.SequenceEqual(Edits))
                {
                    edits.AddRange(ReadArray(ref reader));
                }
            }

            ret.Edits = edits;
            return ret;
        }

        private static IEnumerable<Edit> ReadArray(ref Utf8JsonReader reader)
        {
            List<Edit> ret = new List<Edit>();

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.EndArray:
                        return ret;
                    case JsonTokenType.StartObject:
                        ret.Add(ParseEdit(ref reader));
                        break;
                    case JsonTokenType.String:
                        ret.Add(new Edit { Type = EditType.Remove, Delete = reader.GetString() });
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            throw new ArgumentOutOfRangeException();
        }

        private static Edit ParseEdit(ref Utf8JsonReader reader)
        {
            var edit = new Edit();

            while (reader.Read()
                   && reader.TokenType != JsonTokenType.EndObject)
            {
                var itemPropertyName = reader.ValueSpan;

                if (itemPropertyName.SequenceEqual(BytesDelete))
                {
                    reader.Read();
                    edit.Delete = reader.GetString();
                }
                else if (itemPropertyName.SequenceEqual(BytesInsert))
                {
                    reader.Read();
                    edit.Insert = reader.GetString();
                }
                else if (itemPropertyName.SequenceEqual(BytesType))
                {
                    reader.Read();
                    edit.Type = reader.GetString();
                }
            }

            return edit;
        }
    }
}
