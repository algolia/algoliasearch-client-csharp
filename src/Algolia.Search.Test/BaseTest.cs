/*
* Copyright (c) 2018 Algolia
* http://www.algolia.com/
* Based on the first version developed by Christopher Maneu under the same license:
*  https://github.com/cmaneu/algoliasearch-client-csharp
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
using System.Linq;
using Algolia.Search.Clients;
using Algolia.Search.Models.Batch;
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Responses;
using Algolia.Search.Test;
using NUnit.Framework;

[SetUpFixture]
public class BaseTest
{
    internal static SearchClient SearchClient;
    internal static SearchClient McmClient;
    private static ListIndexesResponse indices;

    [OneTimeSetUp]
    public void Setup()
    {
        TestHelper.CheckEnvironmentVariable();
        SearchClient = new SearchClient(TestHelper.ApplicationId, TestHelper.TestApiKey);
        McmClient = new SearchClient(TestHelper.McmApplicationId, TestHelper.McmApiKey);
        indices = SearchClient.ListIndexes();
    }

    [OneTimeTearDown]
    public void Teardown()
    {
        PreviousTestCleanUp();
    }

    protected void PreviousTestCleanUp()
    {
        if (indices?.Items != null && indices.Items.Any())
        {
            var indicesToDelete = indices.Items.Where(x => x.Name.Contains("csharp_"));
            List<BatchOperation<string>> operations = new List<BatchOperation<string>>();

            foreach (var index in indicesToDelete)
            {
                operations.Add(new BatchOperation<string>
                {
                    IndexName = index.Name,
                    Action = BatchActionType.Delete
                });
            }

            SearchClient.MultipleBatch(operations);
        }
    }
}
