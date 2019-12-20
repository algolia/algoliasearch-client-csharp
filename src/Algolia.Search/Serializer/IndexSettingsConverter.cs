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
using System.Text.Json;
using System.Text.Json.Serialization;
using Algolia.Search.Models.Settings;

namespace Algolia.Search.Serializer
{
    /// <summary>
    /// Algolia's custom converter for IndexSettings
    /// This could be deleted when System.Text.Json while handle field
    /// This converter also has it's own Serializer option. You might modify this one as well
    /// </summary>
    public class IndexSettingsConverter : JsonConverter<IndexSettings>
    {
        /// <summary>
        /// The converter requires it's own options instance to avoid circular calls.
        /// </summary>
        private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            IgnoreNullValues = true,
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new ObjectToInferredTypesConverter(), new NullableDateTimeConverter(), new DateTimeConverter()
            }
        };

        /// <summary>
        /// Custom reader for IndexSettings. Handle legacy settings
        /// </summary>
        public override IndexSettings Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var settings =
                JsonSerializer.Deserialize<IndexSettings>(ref reader, _serializerOptions);

            if (settings.CustomSettings.ContainsKey("attributesToIndex") &&
                settings.CustomSettings["attributesToIndex"] != null)
            {
                settings.SearchableAttributes = (List<string>)settings.CustomSettings["attributesToIndex"];
                settings.CustomSettings.Remove("attributesToIndex");
            }

            if (settings.CustomSettings.ContainsKey("slaves") && settings.CustomSettings["slaves"] != null)
            {
                settings.Replicas = (List<string>)settings.CustomSettings["slaves"];
                settings.CustomSettings.Remove("slaves");
            }

            if (settings.CustomSettings.ContainsKey("numericAttributesToIndex") &&
                settings.CustomSettings["numericAttributesToIndex"] != null)
            {
                settings.NumericAttributesForFiltering =
                    (List<string>)settings.CustomSettings["numericAttributesToIndex"];
                settings.CustomSettings.Remove("numericAttributesToIndex");
            }

            return settings;
        }

        /// <summary>
        /// Using default writer there.
        /// </summary>
        public override void Write(Utf8JsonWriter writer, IndexSettings value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, _serializerOptions);
        }
    }
}
