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

using Algolia.Search.Models.ApiKeys;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;

namespace Algolia.Search.Utils
{
    internal static class QueryStringHelper
    {
        public static string BuildRestrictionQueryString(SecuredApiKeyRestriction restriction)
        {
            string restrictionQuery = null;
            if (restriction.Query != null)
            {
                restrictionQuery = ToQueryString(restriction.Query);
            }

            string restrictIndices = null;
            if (restriction.RestrictIndices != null)
            {
                restrictIndices = $"restrictIndices={string.Join(";", restriction.RestrictIndices)}";
            }

            string restrictSources = null;
            if (restriction.RestrictSources != null)
            {
                restrictSources = $"restrictSources={string.Join(";", restriction.RestrictSources)}";
            }

            string restrictionQueryParams = ToQueryString(restriction, nameof(restriction.Query),
                nameof(restriction.RestrictIndices), nameof(restriction.RestrictSources));
            var array = new[] {restrictionQuery, restrictIndices, restrictSources, restrictionQueryParams};

            return string.Join("&", array.Where(s => !string.IsNullOrEmpty(s)));
        }

        /// <summary>
        /// Transfrom a poco (only class of primitive objects) to a query string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="ignoreList"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string ToQueryString<T>(T value, params string[] ignoreList)
        {
            IEnumerable<string> properties = typeof(T).GetTypeInfo()
                .DeclaredProperties.Where(p =>
                    p.GetValue(value, null) != null && !ignoreList.Contains(p.Name) &&
                    p.GetCustomAttribute<JsonPropertyAttribute>() == null)
                .Select(p => p.Name.ToCamelCase() + "=" + WebUtility.UrlEncode(p.GetValue(value, null).ToString()));

            // Handle properties with JsonPropertyAttribute
            var attr = typeof(T).GetTypeInfo().GetCustomAttribute<JsonPropertyAttribute>();
            IEnumerable<string> propertiesWithJsonAttribute = typeof(T).GetTypeInfo()
                .DeclaredProperties.Where(p =>
                    p.GetValue(value, null) != null && !ignoreList.Contains(p.Name) &&
                    p.GetCustomAttribute<JsonPropertyAttribute>() != null)
                .Select(p =>
                    p.GetCustomAttribute<JsonPropertyAttribute>().PropertyName + "=" +
                    WebUtility.UrlEncode(p.GetValue(value, null).ToString()));

            // Merge twoListBeforeSending
            var mergedProperties = propertiesWithJsonAttribute.Concat(properties);

            return string.Join("&", mergedProperties.ToArray());
        }

        public static string ToQueryString(this Dictionary<string, string> dic)
        {
            if (dic == null)
            {
                throw new ArgumentNullException(nameof(dic));
            }

            return string.Join("&",
                dic.Select(kvp =>
                    string.Format($"{WebUtility.UrlEncode(kvp.Key)}={WebUtility.UrlEncode(kvp.Value)}")));
        }
    }
}