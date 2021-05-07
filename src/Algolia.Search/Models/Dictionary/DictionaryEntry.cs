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
        public String ObjectID { get; set; }

        /// <summary>
        /// Language ISO code supported by the dictionary.
        /// </summary>
        public String Language { get; set; }

    }

    /// <summary>
    /// Represents an entry for Compounds dictionary.
    /// </summary>
    public class Compound : DictionaryEntry
    {
        /// <summary>
        /// When decomposition is empty: adds word as a compound atom.
        /// For example, atom “kino” decomposes the query “kopfkino” into “kopf” and “kino”.
        /// When decomposition isn’t empty: creates a decomposition exception.
        /// For example, when decomposition is set to ["hund", "hutte"], exception “hundehutte” decomposes the word into “hund” and “hutte”, discarding the linking morpheme “e”.
        /// </summary>
        public String Word { get; set; }

        /// <summary>
        /// When empty, the key word is considered as a compound atom. Otherwise, it is the decomposition of word.
        /// </summary>
        public List<String> Decomposition { get; set; }
    }

    /// <summary>
    /// Represents an entry for Plural dictionary.
    /// </summary>
    public class Plural : DictionaryEntry
    {
        /// <summary>
        /// List of word declensions. 
        /// The entry overrides existing entries when any of these words are defined in the standard dictionary provided by Algolia.
        /// </summary>
        public List<String> Words { get; set; }
    }

    /// <summary>
    /// Represents an entry for Stopword dictionary.
    /// </summary>
    public class Stopword : DictionaryEntry
    {
        /// <summary>
        /// The stop word being added or modified. When word already exists in the standard dictionary provided by Algolia, 
        /// the entry can be overridden by the one provided by the user.
        /// </summary>
        public String Word { get; set; }

        /// <summary>
        /// The state of the entry. Can be either "enabled" or "disabled".
        /// </summary>
        public String State { get; set; }

    }
}
