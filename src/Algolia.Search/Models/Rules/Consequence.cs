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

namespace Algolia.Search.Models.Rules
{
    /// <summary>
    /// Consequence of the rule.
    /// </summary>
    public class Consequence
    {
        /// <summary>
        /// Additional search parameters. Any valid search parameter is allowed.
        /// </summary>
        public ConsequenceParams Params { get; set; }

        /// <summary>
        /// Objects to promote as hits.
        /// </summary>
        public IEnumerable<ConsequencePromote> Promote { get; set; }

        /// <summary>
        /// Objects to hide from hits.
        /// </summary>
        public IEnumerable<Hide> Hide { get; set; }

        /// <summary>
        /// Custom JSON object that will be appended to the userData array in the response. 
        /// This object is not interpreted by the API. It is limited to 1kB of minified JSON.
        /// </summary>
        public object UserData { get; set; }
    }
}