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
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Algolia.Search.Serializer
{
    /// <summary>
    /// Helper to improve performance with large json
    /// https://www.newtonsoft.com/json/help/html/performance.htm
    /// </summary>
    internal static class SerializerHelper
    {
        private static readonly UTF8Encoding _encoding = new UTF8Encoding(false);

        public static void Serialize<T>(T data, Stream stream, JsonSerializerSettings settings)
        {
            using (var sw = new StreamWriter(stream, _encoding, 1024, true))
            using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
            {
                JsonSerializer serializer = JsonSerializer.Create(settings);
                serializer.Serialize(jtw, data);
                jtw.Flush();
            }
        }

        public static T Deserialize<T>(Stream stream, JsonSerializerSettings settings)
        {
            if (stream == null || stream.CanRead == false)
                return default;

            using (stream)
            using (var sr = new StreamReader(stream))
            using (var jtr = new JsonTextReader(sr))
            {
                JsonSerializer serializer = JsonSerializer.Create(settings);
                return serializer.Deserialize<T>(jtr);
            }
        }

        public static async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;

            if (stream != null)
            {
                using (var sr = new StreamReader(stream))
                {
                    content = await sr.ReadToEndAsync().ConfigureAwait(false);
                }
            }

            return content;
        }
    }
}
