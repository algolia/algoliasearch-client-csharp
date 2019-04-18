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
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Algolia.Search.Test
{
    internal static class TestHelper
    {
        internal static string ApplicationId1 = Environment.GetEnvironmentVariable("ALGOLIA_APPLICATION_ID_1");
        internal static string AdminKey1 = Environment.GetEnvironmentVariable("ALGOLIA_ADMIN_KEY_1");
        internal static string SearchKey1 = Environment.GetEnvironmentVariable("ALGOLIA_SEARCH_KEY_1");
        internal static string ApplicationId2 = Environment.GetEnvironmentVariable("ALGOLIA_APPLICATION_ID_2");
        internal static string AdminKey2 = Environment.GetEnvironmentVariable("ALGOLIA_ADMIN_KEY_2");
        internal static string McmApplicationId = Environment.GetEnvironmentVariable("ALGOLIA_APPLICATION_ID_MCM");
        internal static string McmAdminKey = Environment.GetEnvironmentVariable("ALGOLIA_ADMIN_KEY_MCM");

        /// <summary>
        /// Check env variable before starting tests suite
        /// </summary>
        internal static void CheckEnvironmentVariable()
        {
            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ALGOLIA_APPLICATION_ID_1")))
            {
                throw new ArgumentNullException("Please set the following environment variable : ALGOLIA_ADMIN_KEY_1");
            }

            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ALGOLIA_ADMIN_KEY_1")))
            {
                throw new ArgumentNullException("Please set the following environment variable : ALGOLIA_ADMIN_KEY_1");
            }

            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ALGOLIA_SEARCH_KEY_1")))
            {
                throw new ArgumentNullException("Please set the following environment variable : ALGOLIA_SEARCH_KEY_1");
            }

            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ALGOLIA_APPLICATION_ID_2")))
            {
                throw new ArgumentNullException("Please set the following environment variable : ALGOLIA_ADMIN_KEY_2");
            }

            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ALGOLIA_ADMIN_KEY_2")))
            {
                throw new ArgumentNullException("Please set the following environment variable : ALGOLIA_ADMIN_KEY_2");
            }

            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ALGOLIA_APPLICATION_ID_MCM")))
            {
                throw new ArgumentNullException(
                    "Please set the following environment variable : ALGOLIA_APPLICATION_ID_MCM");
            }

            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ALGOLIA_ADMIN_KEY_MCM")))
            {
                throw new ArgumentNullException(
                    "Please set the following environment variable : ALGOLIA_ADMIN_KEY_MCM");
            }
        }

        internal static string GetTestIndexName(string testName)
        {
            var date = DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss", CultureInfo.InvariantCulture);
            return $"csharp_{Environment.OSVersion.Platform}_{date}_{Environment.UserName}_{testName}";
        }

        internal static string GetMcmUserId()
        {
            var date = DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture);
            return $"csharp-{date}-{Environment.UserName}";
        }

        /// <summary>
        /// https://www.cyotek.com/blog/comparing-the-properties-of-two-objects-via-reflection
        /// Compares the properties of two objects of the same type and returns if all properties are equal.
        /// </summary>
        /// <param name="objectA">The first object to compare.</param>
        /// <param name="objectB">The second object to compre.</param>
        /// <param name="ignoreList">A list of property names to ignore from the comparison.</param>
        /// <returns><c>true</c> if all property values are equal, otherwise <c>false</c>.</returns>
        internal static bool AreObjectsEqual(object objectA, object objectB, params string[] ignoreList)
        {
            bool result;

            if (objectA != null && objectB != null)
            {
                var objectType = objectA.GetType();

                result = true; // assume by default they are equal

                foreach (PropertyInfo propertyInfo in objectType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && !ignoreList.Contains(p.Name)))
                {
                    var valueA = propertyInfo.GetValue(objectA, null);
                    var valueB = propertyInfo.GetValue(objectB, null);

                    // if it is a primative type, value type or implements IComparable, just directly try and compare the value
                    if (CanDirectlyCompare(propertyInfo.PropertyType))
                    {
                        if (!AreValuesEqual(valueA, valueB))
                        {
                            Console.WriteLine("Mismatch with property '{0}.{1}' found.", objectType.FullName,
                                propertyInfo.Name);
                            result = false;
                        }
                    }
                    // if it implements IEnumerable, then scan any items
                    else if (typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
                    {
                        // null check
                        if (valueA == null && valueB != null || valueA != null && valueB == null)
                        {
                            Console.WriteLine("Mismatch with property '{0}.{1}' found.", objectType.FullName,
                                propertyInfo.Name);
                            result = false;
                        }
                        else if (valueA != null)
                        {
                            var collectionItems1 = ((IEnumerable)valueA).Cast<object>();
                            var collectionItems2 = ((IEnumerable)valueB).Cast<object>();
                            var collectionItemsCount1 = collectionItems1.Count();
                            var collectionItemsCount2 = collectionItems2.Count();

                            // check the counts to ensure they match
                            if (collectionItemsCount1 != collectionItemsCount2)
                            {
                                Console.WriteLine("Collection counts for property '{0}.{1}' do not match.",
                                    objectType.FullName, propertyInfo.Name);
                                result = false;
                            }
                            // and if they do, compare each item... this assumes both collections have the same order
                            else
                            {
                                for (int i = 0; i < collectionItemsCount1; i++)
                                {
                                    var collectionItem1 = collectionItems1.ElementAt(i);
                                    var collectionItem2 = collectionItems2.ElementAt(i);
                                    var collectionItemType = collectionItem1.GetType();

                                    if (CanDirectlyCompare(collectionItemType))
                                    {
                                        if (!AreValuesEqual(collectionItem1, collectionItem2))
                                        {
                                            Console.WriteLine(
                                                "Item {0} in property collection '{1}.{2}' does not match.", i,
                                                objectType.FullName, propertyInfo.Name);
                                            result = false;
                                        }
                                    }
                                    else if (!AreObjectsEqual(collectionItem1, collectionItem2, ignoreList))
                                    {
                                        Console.WriteLine("Item {0} in property collection '{1}.{2}' does not match.",
                                            i, objectType.FullName, propertyInfo.Name);
                                        result = false;
                                    }
                                }
                            }
                        }
                    }
                    else if (propertyInfo.PropertyType.IsClass)
                    {
                        if (!AreObjectsEqual(propertyInfo.GetValue(objectA, null), propertyInfo.GetValue(objectB, null),
                            ignoreList))
                        {
                            Console.WriteLine("Mismatch with property '{0}.{1}' found.", objectType.FullName,
                                propertyInfo.Name);
                            result = false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Cannot compare property '{0}.{1}'.", objectType.FullName, propertyInfo.Name);
                        result = false;
                    }
                }
            }
            else
                result = Equals(objectA, objectB);

            return result;
        }

        /// <summary>
        /// Determines whether value instances of the specified type can be directly compared.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// 	<c>true</c> if this value instances of the specified type can be directly compared; otherwise, <c>false</c>.
        /// </returns>
        private static bool CanDirectlyCompare(Type type)
        {
            return typeof(IComparable).IsAssignableFrom(type) || type.IsPrimitive || type.IsValueType;
        }

        /// <summary>
        /// Compares two values and returns if they are the same.
        /// </summary>
        /// <param name="valueA">The first value to compare.</param>
        /// <param name="valueB">The second value to compare.</param>
        /// <returns><c>true</c> if both values match, otherwise <c>false</c>.</returns>
        private static bool AreValuesEqual(object valueA, object valueB)
        {
            bool result;

            if (valueA == null && valueB != null || valueA != null && valueB == null)
                result = false; // one of the values is null
            else if (valueA is IComparable selfValueComparer && selfValueComparer.CompareTo(valueB) != 0)
                result = false; // the comparison using IComparable failed
            else if (!Equals(valueA, valueB))
                result = false; // the comparison using Equals failed
            else
                result = true; // match

            return result;
        }
    }
}
