using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algolia.Search.Models
{
    public interface IAllRadius
    {
        string GetValue();
    }

    public class AllRadiusInt : IAllRadius
    {
        public int Radius { get; set; }
        public string GetValue()
        {
            return Radius.ToString();
        }
    }

    public class AllRadiusString : IAllRadius
    {
        public string GetValue()
        {
            return "all";
        }
    }
}
