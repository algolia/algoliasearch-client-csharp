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

using Newtonsoft.Json;
using System;

namespace Algolia.Search.Serializer
{
    /// <summary>
    /// Custom converter for Alternatives object
    /// </summary>
    public class AlternativesConverter : JsonConverter<Alternatives>
    {
        /// <summary>
        /// Read the JSON taking into account the polymorphism of Alternatives
        /// </summary>
        public override Alternatives ReadJson(JsonReader reader, Type objectType, Alternatives existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            if (reader.TokenType == JsonToken.Boolean)
                return Alternatives.Of(Convert.ToBoolean(reader.Value));

            return null;
        }

        /// <summary>
        /// Write the JSON taking into account the polymorphism of Alternatives
        /// </summary>
        public override void WriteJson(JsonWriter writer, Alternatives value, JsonSerializer serializer)
        {
            if (value.GetType() == typeof(AlternativesBoolean))
                writer.WriteValue(value.InsideValue);
        }
    }
}
