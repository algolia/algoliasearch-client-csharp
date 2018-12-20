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

namespace Algolia.Search.Models.Insights
{
    /// <summary>
    /// Insights request
    /// </summary>
    public class InsightsRequest
    {
        /// <summary>
        /// Events to send
        /// </summary>
        public IEnumerable<InsightsEvent> Events { get; set; }
    }

    /// <summary>
    /// Insights event
    /// </summary>
    public class InsightsEvent
    {
        /// <summary>
        /// Event type
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// User defined string, format: any ascii char except control points with length between 1 and 64.
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// Index name, format, same as the engine.
        /// </summary>
        public string Index { get; set; }

        /// <summary>
        /// A user identifier, format [a-zA-Z0-9_-]{1,64}.
        /// </summary>
        public string UserToken { get; set; }

        /// <summary>
        /// Event timestamp
        /// </summary>
        public long? Timestamp { get; set; }

        /// <summary>
        /// Algolia queryID, format: [a-z1-9]{32}.
        /// </summary>
        public string QueryID { get; set; }

        /// <summary>
        /// List of index objectIDs. Limited to 20 objects.
        /// </summary>
        public IEnumerable<string> ObjectIDs { get; set; }

        /// <summary>
        ///  A filter is defined by the ${attr}${op}${value} string e.g. brand:apple. Limited to 10 filters.
        /// </summary>
        public IEnumerable<string> Filters { get; set; }

        /// <summary>
        /// Position of the click in the list of Algolia search results.
        /// </summary>
        public IEnumerable<uint> Positions { get; set; }
    }
}