using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algolia.Search
{
    /// <summary>
    /// Client wrapper for the Algolia Search cloud API.
    /// </summary>
    [Obsolete("This class will be removed in a future release. Please use AlgoliaClient instead.", false)]
    public class AlgoliaClientWrapper : AlgoliaClient
    {
        /// <summary>
        /// Algolia Search initialization wrapper
        /// </summary>
        /// <param name="applicationId">The application ID you have in your admin interface</param>
        /// <param name="apiKey">A valid API key for the service</param>
        /// <param name="hosts">The list of hosts that you have received for the service</param>
        /// <param name="mock">Mocking object for controlling HTTP message handler</param>
        public AlgoliaClientWrapper(string applicationId, string apiKey, IEnumerable<string> hosts = null, MockHttpMessageHandler mock = null)
            : base(applicationId, apiKey, hosts, mock)
        {
        }

        /// <summary>
        /// Generates a secured and public API Key from a list of tagFilters and an optional user token identifying the current user
        /// </summary>
        /// <param name="privateApiKey">Your private API Key</param>
        /// <param name="tagFilter">The list of tags applied to the query (used as security)</param>
        /// <param name="userToken">An optional token identifying the current user</param>
        /// <returns></returns>
        public new string GenerateSecuredApiKey(String privateApiKey, String tagFilter, String userToken = null)
        {
            return base.GenerateSecuredApiKey(privateApiKey, tagFilter, userToken);
        }
    }
}