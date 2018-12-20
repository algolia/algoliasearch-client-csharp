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

namespace Algolia.Search.Models.Rules
{
    /// <summary>
    /// Condition of a rule
    /// </summary>
    public class Condition
    {
        /// <summary>
        /// Query patterns are expressed as a string with a specific syntax. A pattern is a sequence of tokens.
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// { is | startsWith | endsWith | contains }: Whether the pattern must match the beginning or the end of the query string, or both, or none.
        /// </summary>
        public string Anchoring { get; set; }

        /// <summary>
        /// Rule context (format: [A-Za-z0-9_-]+). When specified, the rule is contextual and applies only when the same context is specified at query time (using the ruleContexts parameter).
        /// When absent, the rule is generic and always applies (provided that its other conditions are met, of course).
        /// </summary>
        public string Context { get; set; }
    }
}