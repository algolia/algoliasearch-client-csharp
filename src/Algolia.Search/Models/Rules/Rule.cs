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
using System.Collections.Generic;

namespace Algolia.Search.Models.Rules
{
    /// <summary>
    /// https://www.algolia.com/doc/tutorials/query-rules/coding-query-rules-parameters/#coding-query-rule-parameters
    /// </summary>
    public class Rule
    {
        /// <summary>
        /// Unique identifier for the rule (format: [A-Za-z0-9_-]+).
        /// </summary>
        public string ObjectID { get; set; }

        /// <summary>
        /// Condition of the rule, expressed using the following variables: pattern, anchoring, context.
        /// </summary>
        [Obsolete("Single condition is deprecated, use Conditions (plural) which accept one or more condition(s).")]
        public Condition Condition { get; set; }

        /// <summary>
        /// Conditions of the rule, which can be used to apply more than a single condition to the Rule.
        /// </summary>
        public List<Condition> Conditions { get; set; }

        /// <summary>
        /// Consequence of the rule. At least one of the following object must be used: params, promote, hide, userData
        /// </summary>
        public Consequence Consequence { get; set; }

        /// <summary>
        /// This field is intended for rule management purposes, in particular to ease searching for rules and presenting them to human readers. It is not interpreted by the API.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Whether the rule is enabled. Disabled rules remain in the index, but are not applied at query time.
        /// </summary>
        public bool? Enabled { get; set; }

        /// <summary>
        /// By default, rules are permanently valid. When validity periods are specified, the rule applies only during those periods; it is ignored the rest of the time.
        /// The list must not be empty.
        /// </summary>
        public IEnumerable<TimeRange> Validity { get; set; }
    }
}
