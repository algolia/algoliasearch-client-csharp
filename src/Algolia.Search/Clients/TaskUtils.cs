/*
* Copyright (c) 2021 Algolia
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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Algolia.Search.Exceptions;
using Algolia.Search.Http;
using Algolia.Search.Iterators;
using Algolia.Search.Models.Batch;
using Algolia.Search.Models.Common;
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Rules;
using Algolia.Search.Models.Search;
using Algolia.Search.Models.Settings;
using Algolia.Search.Models.Synonyms;
using Algolia.Search.Transport;
using Algolia.Search.Utils;

/// <summary>
/// TaskUltils class/>
/// </summary>
public class TaskUtils
{
    private TaskUtils()
    {
        throw new System.InvalidOperationException("Utility class");
    }

    /// <summary>
    /// Wait for a task to complete before executing the next line of code
    /// </summary>
    /// <param name="taskId">The Algolia taskID</param>
    /// <param name="timeToWait">The time to wait between each call</param>
    /// <param name="requestOptions">Options to pass to this request</param>
    /// <param name="getTask">The function to retrieve the task status</param>
    /// <returns></returns>
    public void WaitTask(long taskId, int timeToWait, RequestOptions requestOptions, Func<long, RequestOptions, TaskStatusResponse> getTask)
    {
        AsyncHelper.RunSync(() => WaitTaskAsync(taskId, timeToWait, requestOptions, getTask));
    }

    /// <summary>
    /// Wait for a task to complete before executing the next line of code
    /// </summary>
    /// <param name="taskId">The Algolia taskID</param>
    /// <param name="timeToWait">The time to wait between each call</param>
    /// <param name="requestOptions">Options to pass to this request</param>
    /// <param name="getTask">The function to retrieve the task status</param>
    /// <param name="ct">The function to retrieve the task status</param>

    /// <returns></returns>
    public async Task WaitTaskAsync(long taskId, int timeToWait, RequestOptions requestOptions, Func<long, RequestOptions, TaskStatusResponse> getTask, CancellationToken ct = default)
    {
        while (true)
        {
            TaskStatusResponse response = getTask(taskId, requestOptions);

            if (response.Status.Equals("published"))
            {
                return;
            }

            await Task.Delay(timeToWait, ct).ConfigureAwait(false);
            timeToWait *= 2;

            if (timeToWait > Defaults.MaxTimeToWait)
            {
                timeToWait = Defaults.MaxTimeToWait;
            }
        }
    }
}
