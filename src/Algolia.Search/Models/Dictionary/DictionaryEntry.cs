/*
* Copyright (c) 2021 Algolia
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

namespace Algolia.Search.Models.Dictionary
{
    /// <summary>
    /// Dictionnary Entry interface
    /// </summary>
    public class DictionaryEntry
    {
        /// <summary>
        /// Algolia's objectID
        /// </summary>
        String objectID { get; set; }

        /// <summary>
        /// The state of the entry. Can be either "enabled" or "disabled".
        /// </summary>
        String language { get; set; }

        /// <summary>
        /// Language ISO code supported by the dictionary.
        /// </summary>
        String word { get; set; }
    }

    /// <summary>
    /// Represents an entry for Compounds dictionary.
    /// </summary>
    public class Compound : DictionaryEntry
    {
        /// <summary>
        /// When empty, the key word is considered as a compound atom. Otherwise, it is the decomposition of word.
        /// </summary>
        private List<String> decomposition { get; set; }
    }

    /// <summary>
    /// Represents an entry for Plural dictionary.
    /// </summary>
    public class Plural : DictionaryEntry
    {
    }

    /// <summary>
    /// Represents an entry for Stopword dictionary.
    /// </summary>
    public class Stopword
    {
        /// <summary>
        /// The state of the entry. Can be either "enabled" or "disabled".
        /// </summary>
        private String state { get; set; }
    }
}