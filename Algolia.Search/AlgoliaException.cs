using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algolia.Search
{
    /// <summary>
    /// Algolia exception.
    /// </summary>
    public class AlgoliaException : Exception
    {
        /// <summary>
        /// Create a new Algolia exception.
        /// </summary>
        /// <param name="message">The exception details.</param>
        public AlgoliaException(string message) : base(message)
        {
            
        }
    }
}
