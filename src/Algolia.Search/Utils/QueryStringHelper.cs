/*
* Copyright (c) 2018 Algolia
* http://www.algolia.com/
* Based on the first version developed by Christopher Maneu under the same license:
*  https://github.com/cmaneu/algoliasearch-client-csharp
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
using System.Net;
using System.Reflection;

namespace Algolia.Search.Utils
{
    internal static class QueryStringHelper
    {
        public static string ToQueryString<T>(T value)
        {
            IEnumerable<string> properties = typeof(T).GetTypeInfo()
                    .DeclaredProperties.Where(p => p.GetValue(value, null) != null)
                    .Select(p => p.Name.ToCamelCase() + "=" + WebUtility.UrlEncode(p.GetValue(value, null).ToString()));

            return string.Join("&", properties.ToArray());
        }

        public static string ToQueryString(this Dictionary<string, Object> dic)
        {
            if (dic == null)
            {
                throw new ArgumentNullException(nameof(dic));
            }

            return WebUtility.UrlEncode(string.Join("&",
                  dic.Select(kvp =>
                      string.Format("{0}={1}", kvp.Key, kvp.Value))));
        }

        public static Dictionary<string, Object> BuildQueryParams<T>(params T[] objects)
        {
            Dictionary<string, object> queryParams = new Dictionary<string, object>();

            if (objects != null)
            {
                foreach (var item in objects)
                {
                    if (item != null)
                    {
                        queryParams.Add(nameof(item), item);
                    }
                }
            }

            return queryParams;
        }
    }
}