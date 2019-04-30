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

using System.Collections.Generic;

/// <summary>
/// When highlighting is enabled, each hit in the response will contain an additional _highlightResult object (provided that at least one of its attributes is highlighted)
/// </summary>
public class HighlightResult
{
    /// <summary>
    /// Markup text with occurrences highlighted. The tags used for highlighting are specified via highlightPreTag and highlightPostTag.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// (string, enum) = {none | partial | full}: Indicates how well the attribute matched the search query.
    /// </summary>
    public string MatchLevel { get; set; }

    /// <summary>
    /// Whether the entire attribute value is highlighted.
    /// </summary>
    public bool? FullyHighlighted { get; set; }

    /// <summary>
    ///  List of words from the query that matched the object.
    /// </summary>
    public IEnumerable<string> MatchedWords { get; set; }
}
