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

using Algolia.Search.Clients;
using Algolia.Search.Models.Batch;
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Common;
using Algolia.Search.Test;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System;

[SetUpFixture]
public class BaseTest
{
    internal static SearchClient SearchClient;
    internal static SearchClient SearchClient2;
    internal static SearchClient McmClient;
    internal static AnalyticsClient AnalyticsClient;
    private static ListIndicesResponse _indices;

    [OneTimeSetUp]
    public void Setup()
    {
        TestHelper.CheckEnvironmentVariable();
        SearchClient = new SearchClient(TestHelper.ApplicationId1, TestHelper.AdminKey1);
        SearchClient2 = new SearchClient(TestHelper.ApplicationId2, TestHelper.AdminKey2);
        McmClient = new SearchClient(TestHelper.McmApplicationId, TestHelper.McmAdminKey);
        AnalyticsClient = new AnalyticsClient(TestHelper.ApplicationId1, TestHelper.AdminKey1);
    }

    [OneTimeTearDown]
    public void Teardown()
    {
        PreviousTestCleanUp();
    }

    protected void PreviousTestCleanUp()
    {
        _indices = SearchClient.ListIndices();

        if (_indices?.Items == null || !_indices.Items.Any())
            return;

        var yesterday = DateTime.UtcNow.AddDays(-1);
        var indicesToDelete = _indices?.Items?.Where(x =>
            x.Name.Contains($"csharp_") && x.CreatedAt <= yesterday).ToList();

        var operations = new List<BatchOperation<string>>();

        if (!indicesToDelete.Any())
            return;

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