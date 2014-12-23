﻿using Newtonsoft.Json.Linq;
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
    /// <summary>
    /// Builder of queries.
    /// </summary>
    public class Query
    {
        /// <summary>
        /// The type of query.
        /// </summary>
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

        /// <summary>
        /// Remove words if no result.
        /// </summary>
        public enum RemoveWordsIfNoResult
        {
            /// <summary>
            /// No specific processing is done when a query does not return any result.
            /// </summary>
            NONE,
            /// <summary>
            /// When a query does not return any result, the final word will be removed until there is results.
            /// </summary>
            LAST_WORDS,
            /// <summary>
            /// When a query does not return any result, the first word will be removed until there is results.
            /// </summary>
            FIRST_WORDS
        }

        /// <summary>
        /// Typo tolerance.
        /// </summary>
        public enum TypoTolerance
        {
            /// <summary>
            /// the typo-tolerance is enabled and all matching hits are retrieved. (Default behavior)
            /// </summary>
            TYPO_TRUE,
            /// <summary>
            /// the typo-tolerance is disabled.
            /// </summary>
            TYPO_FALSE,
            /// <summary>
            ///  only keep the results with the minimum number of typos.
            /// </summary>
            TYPO_MIN,
            /// <summary>
            /// hits matching with 2 typos are not retrieved if there are some matching without typos.
            /// </summary>
            TYPO_STRICT
        }

        /// <summary>
        /// Create a new query.
        /// </summary>
        /// <param name="query">The query.</param>
        public Query(String query)
        {
            minWordSizeForApprox1 = 3;
            minWordSizeForApprox2 = 7;
            getRankingInfo = false;
            ignorePlural = false;
            distinct = false;
            
            page = 0;
            maxValuesPerFacets = 0;
            hitsPerPage = 20;
            this.query = query;
            queryType = QueryType.PREFIX_LAST;
            removeWordsIfNoResult = RemoveWordsIfNoResult.NONE;
            analytics = synonyms = replaceSynonyms = allowTyposOnNumericTokens = true;
            typoTolerance = TypoTolerance.TYPO_TRUE;
        }

        /// <summary>
        /// Create a new query.
        /// </summary>
        public Query()
        {
            minWordSizeForApprox1 = 3;
            minWordSizeForApprox2 = 7;
            getRankingInfo = false;
            ignorePlural = false;
            distinct = false;
            page = 0;
            maxValuesPerFacets = 0;
            hitsPerPage = 20;
            queryType = QueryType.PREFIX_LAST;
            removeWordsIfNoResult = RemoveWordsIfNoResult.NONE;
            analytics = synonyms = replaceSynonyms = allowTyposOnNumericTokens = true;
            typoTolerance = TypoTolerance.TYPO_TRUE;
        }

        /// <summary>
        /// Clone this query to a new query.
        /// </summary>
        /// <returns>The cloned query.</returns>
        public Query clone()
        {
            Query q = new Query();
            q.advancedSyntax = advancedSyntax;
            q.allowTyposOnNumericTokens = allowTyposOnNumericTokens;
            q.analytics = analytics;
            q.aroundLatLong = aroundLatLong;
            q.aroundLatLongViaIP = aroundLatLongViaIP;
            q.attributes = attributes;
            q.attributesToHighlight = attributesToHighlight;
            q.attributesToSnippet = attributesToSnippet;
            q.distinct = distinct;
            q.facetFilters = facetFilters;
            q.facets = facets;
            q.getRankingInfo = getRankingInfo;
            q.hitsPerPage = hitsPerPage;
            q.ignorePlural = ignorePlural;
            q.insideBoundingBox = insideBoundingBox;
            q.maxValuesPerFacets = maxValuesPerFacets;
            q.minWordSizeForApprox1 = minWordSizeForApprox1;
            q.minWordSizeForApprox2 = minWordSizeForApprox2;
            q.numerics = numerics;
            q.optionalWords = optionalWords;
            q.page = page;
            q.query = query;
            q.queryType = queryType;
            q.removeWordsIfNoResult = removeWordsIfNoResult;
            q.replaceSynonyms = replaceSynonyms;
            q.restrictSearchableAttributes = restrictSearchableAttributes;
            q.synonyms = synonyms;
            q.tags = tags;
            q.typoTolerance = typoTolerance;
            return q;
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
        /// Select the spécific processing for the query.
        /// </summary>
        public Query SetRemoveWordsIfNoResult(RemoveWordsIfNoResult type)
        {
            this.removeWordsIfNoResult = type;
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

        /// <summary>
        /// Specify the list of attribute names to retrieve.
        /// </summary>
        /// <param name="attributes">The attributes to retrieve.</param>
        /// <returns>Query for the attributes.</returns>
        public Query SetAttributesToRetrieve(IEnumerable<string> attributes)
        {
            this.attributes = attributes;
            return this;
        }

        /// <summary>
        /// Specify the list of attribute names to highlight.
        /// </summary>
        /// <param name="attributes">The attributes to highlight.</param>
        /// <returns>Query for the attributes.</returns>
        public Query SetAttributesToHighlight(IEnumerable<string> attributes)
        {
            this.attributesToHighlight = attributes;
            return this;
        }

        /// <summary>
        /// Specify the list of attribute names to Snippet alongside the number of words to return (syntax is 'attributeName:nbWords'). By default no snippet is computed.
        /// </summary>
        /// <param name="attributes">The attributes to Snippet.</param>
        /// <returns>Query for the attributes.</returns>
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
        /// If set, the result hits will contain ranking information in _rankingInfo attribute.
        /// </summary>
        public Query GetRankingInfo(bool enabled)
        {
            getRankingInfo = enabled;
            return this;
        }

        /// <summary>
        /// If set to true, plural won't be considered as a typo (for example car/cars will be considered as equals). Defaults to false.
        /// </summary>
        public Query IgnorePlural(bool enabled)
        {
            ignorePlural = enabled;
            return this;
        }

        /// <summary>
        /// This feature is similar to the SQL "distinct" keyword: when enabled in a query with the distinct=1 parameter, 
        /// all hits containing a duplicate value for the attributeForDistinct attribute are removed from results. 
        /// For example, if the chosen attribute is show_name and several hits have the same value for show_name, then only the best 
        /// one is kept and others are removed.
        /// </summary>
        /// <param name="enabled">If set to true, enable the distinct feature (disabled by default) if the attributeForDistinct index setting is set.</param>
        /// <returns></returns>
        public Query EnableDistinct(bool enabled)
        {
            distinct = enabled;
            return this;
        }

        /// <summary>
        /// If set to false, this query will not be taken into account in analytics feature. Default to true.
        /// </summary>
        public Query EnableAnalytics(bool enabled)
        {
            analytics = enabled;
            return this;
        }

        /// <summary>
        ///  If set to false, this query will not use synonyms defined in configuration. Default to true.
        /// </summary>
        public Query EnableSynonyms(bool enabled)
        {
            synonyms = enabled;
            return this;
        }

        /// <summary>
        /// If set to false, words matched via synonyms expansion will not be replaced by the matched synonym in highlight result. Default to true.
        /// </summary>
        public Query EnableReplaceSynonymsInHighlight(bool enabled)
        {
            replaceSynonyms = enabled;
            return this;
        }

        /// <summary>
        /// If set to false, disable typo-tolerance. Default to true.
        /// </summary>
        public Query EnableTypoTolerance(bool enabled)
        {
            if (enabled)
            {
                typoTolerance = TypoTolerance.TYPO_TRUE;
            }
            else
            {
                typoTolerance = TypoTolerance.TYPO_FALSE;
            }
            return this;
        }

        /// <summary>
        /// This option allows you to control the number of typos in the result set.
        /// </summary>
        public Query SetTypoTolerance(TypoTolerance typoTolerance)
        {
            this.typoTolerance = typoTolerance;
            return this;
        }

        /// <summary>
        /// If set to false, disable typo-tolerance on numeric tokens. Default to true.
        /// </summary> 
        public Query EnableTyposOnNumericTokens(bool enabled)
        {
            allowTyposOnNumericTokens = enabled;
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
        /// <param name="latitude">The latitude</param>
        /// <param name="longitude">The longitude</param>
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
        /// <param name="latitude">The latitude</param>
        /// <param name="longitude">The longitude</param>
        /// <param name="radius">set the maximum distance in meters.</param>
        /// <param name="precision">set the precision for ranking (for example if you set precision=100, two objects that are distant of less than 100m will be considered as identical for "geo" ranking parameter).</param>
        /// <returns></returns>
        public Query AroundLatitudeLongitude(float latitude, float longitude, int radius, int precision)
        {
            aroundLatLong = "aroundLatLng=" + latitude + "," + longitude + "&aroundRadius=" + radius + "&aroundPrecision=" + precision;
            return this;
        }

        /// <summary>
        /// Search for entries around a given latitude/longitude (using IP geolocation).
        /// Note: at indexing, geoloc of an object should be set with _geoloc attribute containing lat and lng attributes (for example {"_geoloc":{"lat":48.853409, "lng":2.348800}})
        /// </summary>
        /// <param name="radius">set the maximum distance in meters.</param>
        /// <returns></returns>
        public Query AroundLatitudeLongitudeViaIP(int radius)
        {
            aroundLatLong = "aroundRadius=" + radius;
            aroundLatLongViaIP = true;
            return this;
        }

        /// <summary>
        /// Search for entries around a given latitude/longitude (using IP geolocation).
        /// Note: at indexing, geoloc of an object should be set with _geoloc attribute containing lat and lng attributes (for example {"_geoloc":{"lat":48.853409, "lng":2.348800}})
        /// </summary>
        /// <param name="radius">set the maximum distance in meters.</param>
        /// <param name="precision">set the precision for ranking (for example if you set precision=100, two objects that are distant of less than 100m will be considered as identical for "geo" ranking parameter).</param>
        /// <returns></returns>
        public Query AroundLatitudeLongitudeViaIP(int radius, int precision)
        {
            aroundLatLong = "aroundRadius=" + radius + "&aroundPrecision=" + precision;
        aroundLatLongViaIP = true;
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
        /// Suported operand are &lt;, &lt;=, =, &gt; and &gt;=
        /// You can have multiple conditions on one attribute like for example `numerics=price&gt;100,price&lt;1000`
        /// </summary>
        public Query SetNumericFilters(string value)
        {
            this.numerics = value;
            return this;
        }

        /// <summary>
        /// Set the list of words that should be considered as optional when found in the query.
        /// </summary>
        /// <param name="words">The list of optional words, comma separated.</param>
        /// <returns></returns>
        public Query SetOptionalWords(string words)
        {
            this.optionalWords = words;
            return this;
        }

        /// <summary>
        /// Filter the query by a list of facets.
        /// </summary>
        /// <param name="facets">Each facet is encoded as `attributeName:value`. For example: `["category:Book","author:John%20Doe"].</param>
        /// <returns></returns>
        public Query SetFacetFilters(IEnumerable<string> facets) {
            this.facetFilters = string.Join(",", facets);
            return this;
        }

        /// <summary>
        /// Filter the query by a facet.
        /// </summary>
        /// <param name="facets">The facet is encoded as `attributeName:value`.</param>
        /// <returns></returns>
        public Query SetFacetFilters(string facets)
        {
            this.facetFilters = facets;
            return this;
        }

        /// <summary>
        /// Filter the query by a list of facets.
        /// </summary>
        /// <param name="facets">Each facet is encoded as `attributeName:value`. For example: `["category:Book","author:John%20Doe"].</param>
        /// <returns></returns>
        public Query SetFacetFilters(JArray facets)
        {
            this.facetFilters = Newtonsoft.Json.JsonConvert.SerializeObject(facets);
            return this;
        }

        /// <summary>
        /// Set the max value per facet.
        /// </summary>
        /// <param name="numbers">The number to limit it by.</param>
        /// <returns></returns>
        public Query SetMaxValuesPerFacets(int numbers)
        {
            this.maxValuesPerFacets = numbers;
            return this;
        }

        /// <summary>
        /// List of attributes you want to use for textual search (must be a subset of the attributesToIndex index setting).
        /// </summary>
        /// <param name="attributes">Attributes are separated with a comma (for example @"name,address"). You can also use a JSON string array encoding (for example encodeURIComponent("[\"name\",\"address\"]")). By default, all attributes specified in attributesToIndex settings are used to search.</param>
        /// <returns></returns>
        public Query RestrictSearchableAttributes(String attributes)
        {
            this.restrictSearchableAttributes = attributes;
            return this;
        }

        /// <summary>
        /// Allows enabling of advanced syntax.
        /// </summary>
        /// <param name="enabled">Turn it on or off</param>
        /// <returns></returns>
        public Query EnableAdvancedSyntax(bool enabled)
        {
            this.advancedSyntax = enabled;
            return this;
        }

        /// <summary>
        /// Set the object attributes that you want to use for faceting.
        /// </summary>
        /// <param name="facets">List of object attributes that you want to use for faceting. Only attributes that have been added in **attributesForFaceting** index setting can be used in this parameter. You can also use `*` to perform faceting on all attributes specified in **attributesForFaceting**.</param>
        /// <returns></returns>
        public Query SetFacets(IEnumerable<string> facets) {
            this.facets = facets;
            return this;
        }

        /// <summary>
        /// Get out the query as a string
        /// </summary>
        /// <returns></returns>
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
                if (this.attributes.Count() == 0)
                    stringBuilder += "[]";
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
                if (this.attributesToHighlight.Count() == 0)
                    stringBuilder += "[]";
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
                if (this.attributesToSnippet.Count() == 0)
                    stringBuilder += "[]";
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
            if (facetFilters != null && facetFilters.Length > 0)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "facetFilters=";
                stringBuilder += Uri.EscapeDataString(facetFilters);
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
            if (ignorePlural) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "ignorePlural=true";
            }
            if (distinct)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "distinct=1";
            }
            if (!analytics)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "analytics=0";
            }
            if (!synonyms)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "synonyms=0";
            }
            if (!replaceSynonyms)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "replaceSynonymsInHighlight=0";
            }
            if (typoTolerance != TypoTolerance.TYPO_TRUE)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "typoTolerance=";
                switch (typoTolerance) {
                    case TypoTolerance.TYPO_FALSE:
                        stringBuilder += "false";
                        break;
                    case TypoTolerance.TYPO_MIN:
                        stringBuilder += "min";
                        break;
                    case TypoTolerance.TYPO_STRICT:
                        stringBuilder += "strict";
                        break;
                    case TypoTolerance.TYPO_TRUE:
                        stringBuilder += "true";
                        break;
                }
            }
            if (!allowTyposOnNumericTokens)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "allowTyposOnNumericTokens=false";
            }
            if (advancedSyntax)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "advancedSyntax=1";
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
        if (aroundLatLongViaIP) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "aroundLatLngViaIP=true";
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
            if (restrictSearchableAttributes != null)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "restrictSearchableAttributes=";
                stringBuilder += Uri.EscapeDataString(restrictSearchableAttributes);
            }
            switch (removeWordsIfNoResult)
            {
                case RemoveWordsIfNoResult.NONE:
                    break;
                case RemoveWordsIfNoResult.FIRST_WORDS:
                    if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                    stringBuilder += "removeWordsIfNoResult=FirstWords";
                    break;
                case RemoveWordsIfNoResult.LAST_WORDS:
                    if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                    stringBuilder += "removeWordsIfNoResult=LastWords";
                    break;
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
        private bool ignorePlural;
        private bool distinct;
        private bool advancedSyntax;
        private bool analytics;
        private bool synonyms;
        private bool replaceSynonyms;
        private TypoTolerance typoTolerance;
        private bool allowTyposOnNumericTokens;
        private int page;
        private int hitsPerPage;
        private string tags;
        private string numerics;
        private string insideBoundingBox;
        private string aroundLatLong;
        private bool aroundLatLongViaIP;
        private string query;
        private string optionalWords;
        private QueryType queryType;
        private RemoveWordsIfNoResult removeWordsIfNoResult;
        private IEnumerable<string> facets;
        private string facetFilters;
        private int maxValuesPerFacets;
        private string restrictSearchableAttributes;
    }
}
