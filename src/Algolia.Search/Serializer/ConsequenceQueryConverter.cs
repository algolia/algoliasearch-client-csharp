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
        /// Converter for handling legacy Edit
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="hasExistingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override ConsequenceQuery ReadJson(JsonReader reader, Type objectType, ConsequenceQuery existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;
            if (reader.TokenType != JsonToken.StartObject)
                return null;

            ConsequenceQuery ret = new ConsequenceQuery();

            var tokens = JToken.Load(reader);
            foreach (var token in tokens)
            {
                var isProperty = token.Type == JTokenType.Property;
                if (!isProperty)
                {
                    continue;
                }

                var property = (JProperty)token;
                var isArray = property.Value.Type == JTokenType.Array;
                if (!isArray)
                {
                    continue;
                }

                var array = (JArray)property.Value;
                List<Edit> edits = new List<Edit>();

                switch (property.Name)
                {
                    case "remove":
                        List<string> removeList = array.ToObject<List<string>>();
                        if (removeList.Count > 0)
                        {
                            foreach (var remove in removeList)
                            {
                                edits.Add(new Edit { Type = EditType.Remove, Delete = remove });
                            }
                        }
                        break;

                    case "edits":
                        edits = array.ToObject<List<Edit>>();
                        break;
                }

                if (ret.Edits == null)
                {
                    ret.Edits = edits;
                }
                else
                {
                    ret.Edits = Enumerable.Concat(ret.Edits, edits);
                }
            }

            return ret;
        }

        /// <summary>
        /// No need to implement this method as we want to keep the default writer
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, ConsequenceQuery value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Disable write JSON
        /// </summary>
        public override bool CanWrite => false;
    }
}
