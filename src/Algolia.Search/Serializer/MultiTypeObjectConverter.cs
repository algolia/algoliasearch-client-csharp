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
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Algolia.Search.Serializer
{
    /// <summary>
    /// Multi type converter for handling multi type properties in Algolia settings
    /// </summary>
    public class MultiTypeObjectConverter : JsonConverter
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(object);
        }

        /// <summary>
        /// Algolia's specific converter for backward compatibilities
        /// Some object could be primtive or list of string
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            object ret = new object();

            if (reader.TokenType == JsonToken.Null)
                return ret;

            if (reader.TokenType == JsonToken.StartArray)
            {
                JToken token = JToken.Load(reader);
                List<string> objects = new List<string>();

                foreach (var tokenValue in token)
                {
                    switch (tokenValue.Type)
                    {
                        case JTokenType.String:
                            objects.Add(Convert.ToString(tokenValue));
                            break;
                    }
                }

                return objects;
            }

            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    ret = Convert.ToInt32(reader.Value);
                    break;

                case JsonToken.Float:
                    ret = Convert.ToDecimal(reader.Value);
                    break;

                case JsonToken.String:
                    string tmp = reader.Value.ToString();

                    if (tmp.Contains("false") || tmp.Contains("true"))
                    {
                        ret = Convert.ToBoolean(reader.Value);
                        break;
                    }

                    ret = reader.Value.ToString();
                    break;

                case JsonToken.Boolean:
                    ret = Convert.ToBoolean(reader.Value);
                    break;

                case JsonToken.Null:
                    ret = null;
                    break;

                case JsonToken.Date:
                    ret = Convert.ToDateTime(reader.Value);
                    break;

                case JsonToken.Bytes:
                    ret = Convert.ToByte(reader.Value);
                    break;

                default:
                    ret = null;
                    break;
            }

            return ret;
        }

        /// <summary>
        /// No need to implement this method as we want the default behavior for writting
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Disable write json
        /// </summary>
        public override bool CanWrite => false;
    }
}