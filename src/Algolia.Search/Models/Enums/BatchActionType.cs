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

namespace Algolia.Search.Models.Enums
{
    /// <summary>
    /// Actions that need to be performed
    /// https://www.algolia.com/doc/api-reference/api-methods/batch/
    /// </summary>
    public static class BatchActionType
    {
        /// <summary>
        /// https://www.algolia.com/doc/api-reference/api-methods/batch/?language=csharp#method-param-operation
        /// </summary>
        public const string AddObject = "addObject";

        /// <summary>
        /// https://www.algolia.com/doc/api-reference/api-methods/batch/?language=csharp#method-param-operation
        /// </summary>
        public const string UpdateObject = "updateObject";

        /// <summary>
        /// https://www.algolia.com/doc/api-reference/api-methods/batch/?language=csharp#method-param-operation
        /// </summary>
        public const string PartialUpdateObject = "partialUpdateObject";

        /// <summary>
        /// https://www.algolia.com/doc/api-reference/api-methods/batch/?language=csharp#method-param-operation
        /// </summary>
        public const string PartialUpdateObjectNoCreate = "partialUpdateObjectNoCreate";

        /// <summary>
        /// https://www.algolia.com/doc/api-reference/api-methods/batch/?language=csharp#method-param-operation
        /// </summary>
        public const string DeleteObject = "deleteObject";

        /// <summary>
        /// https://www.algolia.com/doc/api-reference/api-methods/batch/?language=csharp#method-param-operation
        /// </summary>
        public const string Delete = "delete";

        /// <summary>
        /// https://www.algolia.com/doc/api-reference/api-methods/batch/?language=csharp#method-param-operation
        /// </summary>
        public const string Clear = "clear";
    }
}