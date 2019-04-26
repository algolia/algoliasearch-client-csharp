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

/// <summary>
/// Holding all default values of the library
/// </summary>
internal class Defaults
{
    /// <summary>
    /// Maximum time to wait for wait task in milliseconds
    /// </summary>
    public const int MaxTimeToWait = 10000;

    /// <summary>
    /// Read timeout in seconds
    /// </summary>
    public const int ReadTimeout = 5;

    /// <summary>
    /// Write timeout in seconds
    /// </summary>
    public const int WriteTimeut = 30;
    public const string AcceptHeader = "Accept";
    public const string AlgoliaApplicationHeader = "X-Algolia-Application-Id";
    public const string AlgoliaApiKeyHeader = "X-Algolia-API-Key";
    public const string AlgoliaUserIdHeader = "X-Algolia-USER-ID";
    public const string UserAgentHeader = "User-Agent";
    public const string Connection = "Connection";
    public const string KeepAlive = "keep-alive";
}
