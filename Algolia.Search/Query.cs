/*
 * Copyright (c) 2013 Algolia
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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algolia.Search
{
    public class Query
    {
        public enum QueryType
        {
            /// <summary>
            /// All query words are interpreted as prefixes.
            /// </summary>
            PREFIX_ALL,
            /// <summary>
            /// Only the last word is interpreted as a prefix (default behavior).
            /// </summary>
            PREFIX_LAST,
            /// <summary>
            /// No query word is interpreted as a prefix. This option is not recommended.
            /// </summary>
            PREFIX_NONE
        }

        public Query(String query)
        {
            minWordSizeForApprox1 = 3;
            minWordSizeForApprox2 = 7;
            getRankingInfo = false;
            distinct = false;
            page = 0;
            maxValuesPerFacets = 0;
            hitsPerPage = 20;
            this.query = query;
            queryType = QueryType.PREFIX_LAST;
        }

        public Query()
        {
            minWordSizeForApprox1 = 3;
            minWordSizeForApprox2 = 7;
            getRankingInfo = false;
            distinct = false;
            page = 0;
            maxValuesPerFacets = 0;
            hitsPerPage = 20;
            queryType = QueryType.PREFIX_LAST;
        }

        /// <summary>
        /// Select how the query words are interpreted.
        /// </summary>
        public Query SetQueryType(QueryType type)
        {
            this.queryType = type;
            return this;
        }

        /// <summary>
        /// Set the full text query.
        /// </summary>
        public Query SetQueryString(string query)
        {
            this.query = query;
            return this;
        }

        /**
         * Specify the list of attribute names to retrieve. 
         * By default all attributes are retrieved.
         */
        public Query SetAttributesToRetrieve(IEnumerable<string> attributes)
        {
            this.attributes = attributes;
            return this;
        }

        /// <summary>
        /// Specify the list of attribute names to highlight. By default indexed attributes are highlighted.
        /// </summary>
        public Query SetAttributesToHighlight(IEnumerable<string> attributes)
        {
            this.attributesToHighlight = attributes;
            return this;
        }

        /// <summary>
        /// Specify the list of attribute names to Snippet alongside the number of words to return (syntax is 'attributeName:nbWords'). By default no snippet is computed.
        /// </summary>
        public Query SetAttributesToSnippet(IEnumerable<string> attributes)
        {
            this.attributesToSnippet = attributes;
            return this;
        }

        /// <summary>
        /// Specify the minimum number of characters in a query word to accept one typo in this word. Defaults to 3.
        /// </summary>
        public Query SetMinWordSizeToAllowOneTypo(int nbChars)
        {
            minWordSizeForApprox1 = nbChars;
            return this;
        }

        /// <summary>
        /// Specify the minimum number of characters in a query word to accept two typos in this word. Defaults to 7.
        /// </summary>
        public Query SetMinWordSizeToAllowTwoTypos(int nbChars)
        {
            minWordSizeForApprox2 = nbChars;
            return this;
        }

        /// <summary>
        /// if set, the result hits will contain ranking information in _rankingInfo attribute.
        /// </summary>
        public Query GetRankingInfo(bool enabled)
        {
            getRankingInfo = enabled;
            return this;
        }

        /**
         * 
         * @param If set to true, enable the distinct feature (disabled by default) if the attributeForDistinct index setting is set. 
         *   This feature is similar to the SQL "distinct" keyword: when enabled in a query with the distinct=1 parameter, 
         *   all hits containing a duplicate value for the attributeForDistinct attribute are removed from results. 
         *   For example, if the chosen attribute is show_name and several hits have the same value for show_name, then only the best 
         *   one is kept and others are removed.
         */
        public Query EnableDistinct(bool enabled)
        {
            distinct = enabled;
            return this;
        }

        /// <summary>
        /// Set the page to retrieve (zero base). Defaults to 0.
        /// </summary>
        public Query SetPage(int page)
        {
            this.page = page;
            return this;
        }

        /// <summary>
        /// Set the number of hits per page. Defaults to 10.
        /// </summary>
        public Query SetNbHitsPerPage(int nbHitsPerPage)
        {
            this.hitsPerPage = nbHitsPerPage;
            return this;
        }

        /// <summary>
        /// Search for entries around a given latitude/longitude. 
        /// Note: at indexing, geoloc of an object should be set with _geoloc attribute containing lat and lng attributes (for example {"_geoloc":{"lat":48.853409, "lng":2.348800}})
        /// </summary>
        /// <param name="radius">set the maximum distance in meters.</param>
        /// <returns></returns>
        public Query AroundLatitudeLongitude(float latitude, float longitude, int radius)
        {
            aroundLatLong = "aroundLatLng=" + latitude + "," + longitude + "&aroundRadius=" + radius;
            return this;
        }

        /// <summary>
        /// Search for entries around a given latitude/longitude. 
        /// Note: at indexing, geoloc of an object should be set with _geoloc attribute containing lat and lng attributes (for example {"_geoloc":{"lat":48.853409, "lng":2.348800}})
        /// </summary>
        /// <param name="radius">set the maximum distance in meters.</param>
        /// <param name="precision">set the precision for ranking (for example if you set precision=100, two objects that are distant of less than 100m will be considered as identical for "geo" ranking parameter).</param>
        /// <returns></returns>
        public Query AroundLatitudeLongitude(float latitude, float longitude, int radius, int precision)
        {
            aroundLatLong = "aroundLatLng=" + latitude + "," + longitude + "&aroundRadius=" + radius + "&aroundPrecision=" + precision;
            return this;
        }

        /// <summary>
        /// Search for entries inside a given area defined by the two extreme points of a rectangle.
        /// Note: at indexing, geoloc of an object should be set with _geoloc attribute containing lat and lng attributes (for example {"_geoloc":{"lat":48.853409, "lng":2.348800}})
        /// </summary>
        /// <param name="latitudeP1"></param>
        /// <param name="longitudeP1"></param>
        /// <param name="latitudeP2"></param>
        /// <param name="longitudeP2"></param>
        /// <returns></returns>
        public Query InsideBoundingBox(float latitudeP1, float longitudeP1, float latitudeP2, float longitudeP2)
        {
            insideBoundingBox = "insideBoundingBox=" + latitudeP1 + "," + longitudeP1 + "," + latitudeP2 + "," + longitudeP2;
            return this;
        }

        /// <summary>
        /// Filter the query by a set of tags. You can AND tags by separating them by commas. To OR tags, you must add parentheses. For example tag1,(tag2,tag3) means tag1 AND (tag2 OR tag3).
        /// Note: at indexing, tags should be added in the _tags attribute of objects (for example {"_tags":["tag1","tag2"]} )
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public Query SetTagFilters(string tags)
        {
            this.tags = tags;
            return this;
        }

        /// <summary>
        /// Add a list of numeric filters separated by a comma.
        /// The syntax of one filter is `attributeName` followed by `operand` and `value`.
        /// Suported operand are <, <=, =, > and >=
        /// You can have multiple conditions on one attribute like for example "numerics=price>100,price<1000"
        /// </summary>
        public Query SetNumericFilters(string value)
        {
            this.numerics = value;
            return this;
        }

        /**
         * Set the list of words that should be considered as optional when found in the query. 
         * @param words The list of optional words, comma separated.
         */
        public Query SetOptionalWords(string words)
        {
            this.optionalWords = words;
            return this;
        }

        /**
         * Filter the query by a list of facets. Each facet is encoded as `attributeName:value`. For example: `["category:Book","author:John%20Doe"].
         */
        public Query SetFacetFilters(IEnumerable<string> facets) {
    	    this.facetFilters = facets;
    	    return this;
        }

        public Query SetMaxValuesPerFacets(int numbers)
        {
            this.maxValuesPerFacets = numbers;
            return this;
        }

        /**
         * List of object attributes that you want to use for faceting. <br/>
         * Only attributes that have been added in **attributesForFaceting** index setting can be used in this parameter. 
         * You can also use `*` to perform faceting on all attributes specified in **attributesForFaceting**.
         */
        public Query SetFacets(IEnumerable<string> facets) {
    	    this.facets = facets;
    	    return this;
        }

        public string GetQueryString() {
            string stringBuilder = "";
        
            if (attributes != null) {
                stringBuilder += "attributesToRetrieve=";
                bool first = true;
                foreach (string attr in this.attributes) {
                    if (!first)
                        stringBuilder += ',';
                    stringBuilder += Uri.EscapeDataString(attr);
                    first = false;
                }
            }
            if (attributesToHighlight != null) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "attributesToHighlight=";
                bool first = true;
                foreach (string attr in this.attributesToHighlight) {
                    if (!first)
                        stringBuilder += ',';
                    stringBuilder += Uri.EscapeDataString(attr);
                    first = false;
                }
            }
            if (attributesToSnippet != null) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "attributesToSnippet=";
                bool first = true;
                foreach (string attr in this.attributesToSnippet) {
                    if (!first)
                        stringBuilder += ',';
                    stringBuilder += Uri.EscapeDataString(attr);
                    first = false;
                }
            }
            if (facets != null)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "facets=";
                bool first = true;
                foreach (string attr in this.facets)
                {
                    if (!first)
                        stringBuilder += ',';
                    stringBuilder += Uri.EscapeDataString(attr);
                    first = false;
                }
            }
            if (facetFilters != null)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "facetFilters=";
                stringBuilder += Newtonsoft.Json.JsonConvert.SerializeObject(facetFilters);
            }
            if (maxValuesPerFacets != 0)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "maxValuesPerFacet=";
                stringBuilder += Newtonsoft.Json.JsonConvert.SerializeObject(maxValuesPerFacets);
            }
            if (attributesToSnippet != null)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "attributesToSnippet=";
                bool first = true;
                foreach (string attr in this.attributesToSnippet)
                {
                    if (!first)
                        stringBuilder += ',';
                    stringBuilder += Uri.EscapeDataString(attr);
                    first = false;
                }
            }
            if (minWordSizeForApprox1 != 3) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "minWordSizefor1Typo=";
                stringBuilder += minWordSizeForApprox1.ToString();
            }
            if (minWordSizeForApprox2 != 7) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "minWordSizefor2Typos=";
                stringBuilder += minWordSizeForApprox2.ToString();
            }
            if (getRankingInfo) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "getRankingInfo=1";
            }
            if (distinct)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "distinct=1";
            }
            if (page > 0) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "page=";
                stringBuilder += page.ToString();
            }
            if (hitsPerPage != 20 && hitsPerPage > 0) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "hitsPerPage=";
                stringBuilder += hitsPerPage.ToString();
            }
            if (tags != null) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "tagFilters=";
                stringBuilder += Uri.EscapeDataString(tags);
            }
            if (numerics != null) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "numericFilters=";
                stringBuilder += Uri.EscapeDataString(numerics);
            }
            if (insideBoundingBox != null) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += insideBoundingBox;
            } else if (aroundLatLong != null) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += aroundLatLong;
            }
            if (query != null) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "query=";
                stringBuilder += Uri.EscapeDataString(query);
            }
            if (optionalWords != null)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "optionalWords=";
                stringBuilder += Uri.EscapeDataString(optionalWords);
            }

            switch (queryType) {
            case QueryType.PREFIX_ALL:
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "queryType=prefixAll";
            	break;
            case QueryType.PREFIX_LAST:
            	break;
            case QueryType.PREFIX_NONE:
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "queryType=prefixNone";
            	break;
            }
            return stringBuilder;
        }

        private IEnumerable<string> attributes;
        private IEnumerable<string> attributesToHighlight;
        private IEnumerable<string> attributesToSnippet;
        private int minWordSizeForApprox1;
        private int minWordSizeForApprox2;
        private bool getRankingInfo;
        private bool distinct;
        private int page;
        private int hitsPerPage;
        private string tags;
        private string numerics;
        private string insideBoundingBox;
        private string aroundLatLong;
        private string query;
        private string optionalWords;
        private QueryType queryType;
        private IEnumerable<string> facets;
        private IEnumerable<string> facetFilters;
        private int maxValuesPerFacets;
    }
}
