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
using System.Reflection;
using Algolia.Search.Exceptions;
using Algolia.Search.Models.Search;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Algolia.Search.Utils
{
    /// <summary>
    /// A collection of helpers related to Algolia
    /// </summary>
    public static class AlgoliaHelper
    {
        /// <summary>
        /// GetObjectPosition returns the position (0-based) within the `Hits`
        /// result list of the record matching against the given `objectID`. If the
        /// `objectID` is not found, `-1` is returned.
        /// </summary>
        /// <param name="objectID">ID of the record the check.</param>
        /// <param name="searchResult">SearchResult to look in. </param>
        public static int GetObjectPosition<T>(this SearchResponse<T> searchResult, string objectID) where T : class
        {
            return searchResult.Hits.FindIndex(x => GetObjectID(x).Equals(objectID));
        }


        /// <summary>
        /// GetObjectIDPosition returns the position (0-based) within the `Hits`
        /// result list of the record matching against the given `objectID`. If the
        /// `objectID` is not found, `-1` is returned.
        /// </summary>
        /// <param name="objectID">ID of the record the check.</param>
        /// <param name="searchResult">SearchResult to look in. </param>
        [ObsoleteAttribute("This function will be deprecated. Use GetObjectPosition instead.")]
        public static int GetObjectIDPosition<T>(this SearchResponse<T> searchResult, string objectID) where T : class
        {
            return GetObjectPosition(searchResult, objectID);
        }

        /// <summary>
        /// Ensure that the type T has a property ObjectID or a JsonPropertyAttribute(Name="objectID")
        /// </summary>
        /// <typeparam name="T">Type to check/type</typeparam>
        internal static void EnsureObjectID<T>()
        {
            Type itemType = typeof(T);
            if (itemType == typeof(JToken) || itemType == typeof(JObject) || itemType == typeof(JArray) ||
                itemType == typeof(JRaw))
            {
                return;
            }

            var objectIdProperty = PropertyOrJsonAttributeExists<T>("ObjectID");

            if (objectIdProperty != null)
            {
                if (objectIdProperty.PropertyType != typeof(string))
                {
                    throw new AlgoliaException(
                        $"The ObjectID property or the JsonPropertyAttribute with name='objectID' must be a string");
                }

                return;
            }

            throw new AlgoliaException(
                $"The type {nameof(T)} must have an ObjectID property or a JsonPropertyAttribute with name='objectID'");
        }

        /// <summary>
        /// Check if the Property or JsonPropertyAttribute with name='objectID' exists in the given type'
        /// </summary>
        /// <typeparam name="T">Type of the data to send/retrieve</typeparam>
        /// <param name="data">Data to send</param>
        /// <returns></returns>
        internal static string GetObjectID<T>(T data)
        {
            Type itemType = typeof(T);

            if (itemType == typeof(JObject))
            {
                var objectIdJProperty = JObject.FromObject(data).Property("objectID");

                if (objectIdJProperty != null && !string.IsNullOrWhiteSpace(objectIdJProperty.Value.ToString()))
                {
                    return objectIdJProperty.Value.ToString();
                }
            }

            var objectIdProperty = PropertyOrJsonAttributeExists<T>("ObjectID");

            if (objectIdProperty != null)
            {
                if (objectIdProperty.PropertyType != typeof(string))
                {
                    throw new AlgoliaException(
                        $"The ObjectID property or the JsonPropertyAttribute with name='objectID' must be a string");
                }

                return objectIdProperty.GetValue(data, null).ToString();
            }

            throw new AlgoliaException(
                $"The type {nameof(T)} must have an ObjectID properties or a JsonPropertyAttribute with name='objectID'");
        }

        /// <summary>
        /// Get all properties of a type (including base class)
        /// </summary>
        /// <param name="T"></param>
        internal static IEnumerable<PropertyInfo> GetAllProperties(Type T)
        {
            IEnumerable<PropertyInfo> PropertyList = T.GetTypeInfo().DeclaredProperties;

            if (T.GetTypeInfo().BaseType != null)
            {
                PropertyList = PropertyList.Concat(GetAllProperties(T.GetTypeInfo().BaseType));
            }

            return PropertyList;
        }

        /// <summary>
        /// Check if the Property or JsonPropertyAttribute exists in the given type, return null if not exist otherwise return propertyInfo
        /// </summary>
        /// <typeparam name="T">Type of the data to send/retrieve</typeparam>
        /// <param name="propertyName">Name of the property to check. Will be test as PascalCase for property and as camelCase for JsonAttribute</param>
        /// <returns></returns>
        private static PropertyInfo PropertyOrJsonAttributeExists<T>(string propertyName)
        {
            var declaredProperties = GetAllProperties(typeof(T));

            var objectIdProperty = declaredProperties.FirstOrDefault(p => p.Name.Equals(propertyName));

            if (objectIdProperty != null)
            {
                return objectIdProperty;
            }

            var camelCaseProperty = CamelCaseHelper.ToCamelCase(propertyName);

            if (declaredProperties.Any(x => x.GetCustomAttribute<JsonPropertyAttribute>() != null))
            {
                var propertiesWithJsonPropertyattributes =
                    declaredProperties.Where(p => p.GetCustomAttribute<JsonPropertyAttribute>() != null);
                return propertiesWithJsonPropertyattributes.FirstOrDefault(p =>
                    p.GetCustomAttribute<JsonPropertyAttribute>().PropertyName.Equals(camelCaseProperty));
            }

            return null;
        }
    }
}
