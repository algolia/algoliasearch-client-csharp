using System;
using System.Net;

namespace Algolia.Search
{
    /// <summary>
    /// Algolia exception.
    /// </summary>
    public class AlgoliaException : Exception
    {
        public HttpStatusCode? HttpStatusCode { get; }

        /// <summary>
        /// Create a new Algolia exception.
        /// </summary>
        /// <param name="message">The exception details.</param>
        /// <param name="httpStatusCode">The raw HttpStatusCode</param>
        public AlgoliaException(string message, HttpStatusCode? httpStatusCode = null) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}
