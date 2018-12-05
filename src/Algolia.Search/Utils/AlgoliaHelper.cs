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

using Algolia.Search.Exceptions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Algolia.Search.Utils
{
    internal static class AlgoliaHelper
    {
        /// <summary>
        /// Ensure that the List has an JsonPropertyAttribute(Name="ObjectID")
        /// </summary>
        /// <typeparam name="T">Type of the data to send/retrieve</typeparam>
        /// <param name="datas">Datas to send</param>
        public static void EnsureObjectID<T>(IEnumerable<T> datas)
        {
            var propertyWithObjectIdAttribute = CheckAttribute<T>();

            if (datas == null)
            {
                return;
            }

            if (datas.Any(x => string.IsNullOrWhiteSpace((string) propertyWithObjectIdAttribute.GetValue(x, null))))
            {
                throw new AlgoliaException(
                    "Property with JsonPropertyAttribute with name='objectID' must not be null or empty");
            }
        }

        /// <summary>
        /// Get the property which has JsonPropertyAttribute with name='objectID' 
        /// </summary>
        /// <typeparam name="T">Type of the data to send/retrieve</typeparam>
        /// <param name="data">Data to send</param>
        /// <returns></returns>
        public static string GetObjectID<T>(T data)
        {
            var propertyWithObjectIdAttribute = CheckAttribute<T>();
            return (string) propertyWithObjectIdAttribute.GetValue(data);
        }

        /// <summary>
        /// Check if JsonPropertyAttribute with name='objectID' exist in the given type
        /// </summary>
        /// <typeparam name="T">Type of the data to send/retrieve</typeparam>
        /// <returns></returns>
        private static PropertyInfo CheckAttribute<T>()
        {
            var attr = typeof(T).GetTypeInfo().GetCustomAttribute<JsonPropertyAttribute>();

            var propertyWithObjectIdAttribute = typeof(T).GetTypeInfo().DeclaredProperties
                .First(p => p.GetCustomAttribute<JsonPropertyAttribute>().PropertyName.Equals("objectID"));

            if (!attr.PropertyName.Equals("objectID"))
            {
                throw new AlgoliaException("The class mut have a JsonPropertyAttribute property with name='objectID'");
            }

            if (propertyWithObjectIdAttribute.GetType() != typeof(string))
            {
                throw new AlgoliaException("Property with JsonPropertyAttribute with name='objectID' must be a string");
            }

            return propertyWithObjectIdAttribute;
        }
    }
}