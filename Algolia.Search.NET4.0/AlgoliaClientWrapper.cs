using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algolia.Search
{
    public class AlgoliaClientWrapper : AlgoliaClient
    {

        public AlgoliaClientWrapper(string applicationId, string apiKey, IEnumerable<string> hosts = null, MockHttpMessageHandler mock = null)
            : base(applicationId, apiKey, hosts, mock)
        {
        }
        /// <summary>
        /// <summary>
        /// Generate a secured and public API Key from a list of tagFilters and an
        /// optional user token identifying the current user
        /// </summary>
        /// <param name="privateApiKey">your private API Key</param>
        /// <param name="tagFilter">the list of tags applied to the query (used as security)</param>
        /// <param name="userToken">an optional token identifying the current user</param>
        /// <returns></returns>
        public override string GenerateSecuredApiKey(String privateApiKey, String tagFilter, String userToken = null)
        {
            string msg = tagFilter;
            if (userToken != null)
            {
                msg += userToken;
            }
            return Hmac(privateApiKey, msg);
        }

        private string Hmac(string key, string msg)
        {
            System.Security.Cryptography.HMACSHA256 hmac = new System.Security.Cryptography.HMACSHA256(Encoding.ASCII.GetBytes(key));
            return hmac.ComputeHash(Encoding.ASCII.GetBytes(msg)).Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s);
        }
    }
}
