using System.Collections.Generic;
using System.Net.Http.Headers;
using Algolia.Search.Models;
using Newtonsoft.Json.Converters;

namespace Algolia.Search
{
    internal class Constants
    {
        public static readonly List<string> AnalyticsUrl = new List<string>(1) { "analytics.algolia.com" };
        public const string Ampersand = "&";
        public const string Comma = ",";
        public const string EmptyArrayString = "[]";

        public static readonly MediaTypeWithQualityHeaderValue JsonMediaType = new MediaTypeWithQualityHeaderValue("application/json");

        public static readonly StringEnumConverter StringEnumConverter = new Newtonsoft.Json.Converters.StringEnumConverter();

    }
}