using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algolia.Search
{
    public class AlgoliaException : Exception
    {
        public AlgoliaException(string message) : base(message)
        {
            
        }
    }
}
