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

using System.IO;
using Algolia.Search.Models.Enums;

namespace Algolia.Search.Serializer
{
    /// <summary>
    /// Interface representing the expected behavior of the Serializer.
    /// </summary>
    internal interface ISerializer
    {
        /// <summary>
        /// Converts the value of a specified type into a JSON string.
        /// </summary>
        /// <param name="data">The value to convert and write.</param>
        /// <param name="stream">The Stream containing the data to read.</param>
        /// <param name="compressionType">How the stream should be compressed <see cref="CompressionType"/></param>
        /// <typeparam name="T">The type of the value to convert.</typeparam>
        void Serialize<T>(T data, Stream stream, CompressionType compressionType);

        /// <summary>
        /// Parses the stream into an instance of a specified type.
        /// </summary>
        /// <param name="stream">The Stream containing the data to read.</param>
        /// <typeparam name="T">The type of the value to convert.</typeparam>
        /// <returns></returns>
        T Deserialize<T>(Stream stream);
    }
}
