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
    /// Move operation
    /// </summary>
    public class PartialUpdateOperationType
    {
        /// <summary>
        /// Increment by an integer value
        /// </summary>
        public const string Increment = "Increment";

        /// <summary>
        /// Decrement by an integer value
        /// </summary>
        public const string Decrement = "Decrement";

        /// <summary>
        /// Add value to a collection
        /// </summary>
        public const string Add = "Add";

        /// <summary>
        /// Add value to a collection if it does not exist
        /// </summary>
        public const string AddUnique = "AddUnique";

        /// <summary>
        /// Remove value from a collection
        /// </summary>
        public const string Remove = "Remove";
    }
}
