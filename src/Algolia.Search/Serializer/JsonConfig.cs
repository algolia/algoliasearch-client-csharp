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

using System.Text.Json;

namespace Algolia.Search.Serializer
{
    /// <summary>
    /// Options for the default serializer
    /// Naming policy is set to camelCase to respect both C# and JSON standards.
    /// </summary>
    internal static class JsonConfig
    {
        /// <summary>
        /// Json content type constant
        /// </summary>
        public const string JsonContentType = "application/json";

        /// <summary>
        /// Default JSONSerializer options - mostly used to respect Algolia, JSON and C# conventions
        /// Can be overriden at the class level
        /// </summary>
        public static JsonSerializerOptions AlgoliaJsonSerializerOption => new JsonSerializerOptions
        {
            IgnoreNullValues = true,
            WriteIndented = false,
            // Algolia API is expecting camel case naming policy
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                // Default converters to handle all date time as UTC and not in local time zone
                new NullableDateTimeConverter(),
                new DateTimeConverter(),
                // Default converter for "object"
                new ObjectToInferredTypesConverter(),
                // Specific converter for IndexSettings to handle legacy fields.
                // This one could be deleted when System.Text.Json while handle field
                // This converter also has it's own Serializer option. You might modify this one as well
                new IndexSettingsConverter(),
            }
        };
    }
}
