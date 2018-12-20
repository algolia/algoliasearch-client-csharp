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

namespace Algolia.Search.Models.Synonyms
{
    /// <summary>
    /// https://www.algolia.com/doc/api-reference/api-methods/save-synonym/
    /// </summary>
    public class Synonym
    {
        /// <summary>
        /// Synonym object ID
        /// </summary>
        public string ObjectID { get; set; }

        /// <summary>
        /// There are 4 synonym types. The parameter can be one of the following values <see cref="Enums.SynonymType"/>
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// A list of synonyms
        /// </summary>
        public List<string> Synonyms { get; set; }

        /// <summary>
        /// Defines the synonym. A word or expression, used as the basis for the array of synonyms.
        /// </summary>
        public string Input { get; set; }

        /// <summary>
        /// A single word, used as the basis for the below array of corrections.
        /// </summary>
        public string Word { get; set; }

        /// <summary>
        /// A list of corrections of the word.
        /// </summary>
        public List<string> Corrections { get; set; }

        /// <summary>
        /// A single word, used as the basis for the below list of replacements.
        /// </summary>
        public string Placeholder { get; set; }

        /// <summary>
        /// An list of replacements of the placeholder.
        /// </summary>
        public List<string> Replacements { get; set; }
    }
}