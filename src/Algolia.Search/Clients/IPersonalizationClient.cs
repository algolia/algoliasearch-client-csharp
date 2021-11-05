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

using System.Threading;
using System.Threading.Tasks;
using Algolia.Search.Http;
using Algolia.Search.Models.Personalization;

namespace Algolia.Search.Clients
{
    /// <summary>
    /// Client for the <see href="https://www.algolia.com/doc/rest-api/personalization/">Personalization API</see>
    /// </summary>
    public interface IPersonalizationClient
    {
        /// <summary>
        /// Returns the personalization strategy of the application
        /// </summary>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        GetStrategyResponse GetPersonalizationStrategy(RequestOptions requestOptions = null);

        /// <summary>
        /// Returns the personalization strategy of the application
        /// </summary>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        Task<GetStrategyResponse> GetPersonalizationStrategyAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default);

        /// <summary>
        /// This command configures the personalization strategy
        /// </summary>
        /// <param name="request">The personalization strategy</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        SetStrategyResponse
            SetPersonalizationStrategy(SetStrategyRequest request, RequestOptions requestOptions = null);

        /// <summary>
        /// This command configures the personalization strategy
        /// </summary>
        /// <param name="request">The personalization strategy</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        Task<SetStrategyResponse> SetPersonalizationStrategyAsync(SetStrategyRequest request,
            RequestOptions requestOptions = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get the user profile built from Personalization strategy
        /// </summary>
        /// <param name="userToken">userToken representing the user for which to fetch the Personalization profile</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        GetPersonalizationProfileResponse GetPersonalizationProfile(string userToken, RequestOptions requestOptions = null);

        /// <summary>
        /// Get the user profile built from Personalization strategy
        /// </summary>
        /// <param name="userToken">userToken representing the user for which to fetch the Personalization profile</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        Task<GetPersonalizationProfileResponse> GetPersonalizationProfileAsync(string userToken,
            RequestOptions requestOptions = null,
            CancellationToken ct = default);

        /// <summary>
        /// Delete the user profile and all its associated data
        /// </summary>
        /// <param name="userToken">userToken representing the user for which to delete the Personalization profile and associated data</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        DeletePersonalizationProfileResponse DeletePersonalizationProfile(string userToken, RequestOptions requestOptions = null);

        /// <summary>
        /// Delete the user profile and all its associated data
        /// </summary>
        /// <param name="userToken">userToken representing the user for which to delete the Personalization profile and associated data</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        Task<DeletePersonalizationProfileResponse> DeletePersonalizationProfileAsync(string userToken,
            RequestOptions requestOptions = null,
            CancellationToken ct = default);
    }
}
