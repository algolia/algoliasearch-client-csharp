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
using System.Security.Cryptography;
using System.Text;

namespace Algolia.Search.Utils
{
    /// <summary>
    /// Helper to compute HMAC SHA 256
    /// </summary>
    internal static class HmacShaHelper
    {
        private static readonly ASCIIEncoding _encoding = new ASCIIEncoding();

        internal static string GetHash(string text, string key)
        {
            Byte[] keyBytes = _encoding.GetBytes(key);
            Byte[] textBytes = _encoding.GetBytes(text);
            string hash;

            using (HMACSHA256 hmac = new HMACSHA256(keyBytes))
            {
                byte[] hmBytes = hmac.ComputeHash(textBytes);
                return hash = BitConverter.ToString(hmBytes).Replace("-", string.Empty);
            }
        }

        internal static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}