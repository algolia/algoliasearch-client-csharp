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

using Algolia.Search.Models.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Algolia.Search.Serializer
{
    internal class SettingsConverter : JsonConverter<IndexSettings>
    {
        public override bool CanWrite => false;

        /// <summary>
        /// Custom serializer to handle/map legacy objects to new properties
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="hasExistingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override IndexSettings ReadJson(JsonReader reader, Type objectType, IndexSettings existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.Null)
            {
                JObject json = JObject.Load(reader);
                IndexSettings settings = new IndexSettings();
                serializer.Populate(json.CreateReader(), settings);
                var properties = json.Properties().ToList();

                if (properties.Exists(x => x.Name.Equals("attributesToIndex")))
                {
                    settings.SearchableAttributes = new List<string>();
                    foreach (var token in properties.FirstOrDefault(x => x.Name.Equals("attributesToIndex")).Values())
                    {
                        if (token.Type != JTokenType.Null)
                        {
                            settings.SearchableAttributes.Add(token.Value<string>());
                        }
                    }
                }

                if (properties.Exists(x => x.Name.Equals("numericAttributesToIndex")))
                {
                    settings.NumericAttributesForFiltering = new List<string>();
                    foreach (var token in properties.FirstOrDefault(x => x.Name.Equals("numericAttributesToIndex"))
                        .Values())
                    {
                        if (token.Type != JTokenType.Null)
                        {
                            settings.NumericAttributesForFiltering.Add(token.Value<string>());
                        }
                    }
                }

                if (properties.Exists(x => x.Name.Equals("slaves")))
                {
                    settings.Replicas = new List<string>();
                    foreach (var token in properties.FirstOrDefault(x => x.Name.Equals("slaves")).Values())
                    {
                        if (token.Type != JTokenType.Null)
                        {
                            settings.Replicas.Add(token.Value<string>());
                        }
                    }
                }

                return settings;
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, IndexSettings value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}