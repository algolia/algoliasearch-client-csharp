using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algolia.Search.Models
{
    public interface IEnabledRemoveStopWords
    {
        string GetValue();
    }

    public class EnabledRemoveStopWordsBool : IEnabledRemoveStopWords
    {
        public bool Enabled { get; set; }
        public string GetValue()
        {
            return Enabled.ToString().ToLower();
        }
    }

    public class EnabledRemoveStopWordsList : IEnabledRemoveStopWords
    {
        public string Enabled { get; set; }
        public string GetValue()
        {
            return Enabled;
        }
    }

}
