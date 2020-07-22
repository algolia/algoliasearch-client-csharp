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
using Newtonsoft.Json;

namespace Algolia.Search.Models.Mcm
{
    /// <summary>
    /// https://www.algolia.com/doc/api-reference/api-methods/list-user-id/
    /// </summary>
    public class ListUserIdsResponse
    {
        /// <summary>
        /// List of users id
        /// </summary>
        [JsonProperty(PropertyName = "userIDs")]
        public List<UserIdResponse> UserIds { get; set; }
    }

    /// <summary>
    /// https://www.algolia.com/doc/api-reference/api-methods/list-user-id/
    /// </summary>
    public class UserIdResponse
    {
        /// <summary>
        /// userID of the user.
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// Cluster on which the user is assigned
        /// </summary>
        public string ClusterName { get; set; }

        /// <summary>
        /// Number of records belonging to the user.
        /// </summary>
        public int NbRecords { get; set; }

        /// <summary>
        /// Data size used by the user.
        /// </summary>
        public int DataSize { get; set; }
    }
}
