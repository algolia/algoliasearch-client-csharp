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

using Algolia.Search.Models.Enums;
using Newtonsoft.Json;

namespace Algolia.Search.Models.Search
{
    /// <summary>
    ///  PartialUpdateOperation used to apply a local change to an attribute
    ///  when performing a PartialUpdate[s] call
    /// </summary>
    public class PartialUpdateOperation<T>
    {
        /// <summary>
        /// Operation to apply on the attribute
        /// </summary>
        [JsonProperty(PropertyName = "_operation")]
        public string Operation { get; set; }

        /// <summary>
        /// Value used as an operand for the operation
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Increment by an integer value
        /// </summary>
        public static PartialUpdateOperation<int> Increment(int value)
        {
            return new PartialUpdateOperation<int>
            {
                Operation = PartialUpdateOperationType.Increment,
                Value = value,
            };
        }

        /// <summary>
        /// Increment a numeric integer attribute only if the provided value matches the current value, and otherwise ignores the whole object update
        /// </summary>
        public static PartialUpdateOperation<int> IncrementFrom(int value)
        {
            return new PartialUpdateOperation<int>
            {
                Operation = PartialUpdateOperationType.IncrementFrom,
                Value = value,
            };
        }

        /// <summary>
        /// Increment a numeric integer attribute only if the provided value is greater than the current value, and otherwise ignore the whole object update
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static PartialUpdateOperation<int> IncrementSet(int value)
        {
            return new PartialUpdateOperation<int>
            {
                Operation = PartialUpdateOperationType.IncrementSet,
                Value = value,
            };
        }

        /// <summary>
        /// Decrement by an integer value
        /// </summary>
        public static PartialUpdateOperation<int> Decrement(int value)
        {
            return new PartialUpdateOperation<int>
            {
                Operation = PartialUpdateOperationType.Decrement,
                Value = value,
            };
        }

        /// <summary>
        /// Add value to a collection
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static PartialUpdateOperation<T> Add(T value)
        {
            return new PartialUpdateOperation<T>
            {
                Operation = PartialUpdateOperationType.Add,
                Value = value,
            };
        }

        /// <summary>
        /// Add value to a collection if it does not exist
        /// </summary>
        public static PartialUpdateOperation<T> AddUnique(T value)
        {
            return new PartialUpdateOperation<T>
            {
                Operation = PartialUpdateOperationType.AddUnique,
                Value = value,
            };
        }

        /// <summary>
        /// Remove value from a collection
        /// </summary>
        public static PartialUpdateOperation<T> Remove(T value)
        {
            return new PartialUpdateOperation<T>
            {
                Operation = PartialUpdateOperationType.Remove,
                Value = value,
            };
        }
    }
}
