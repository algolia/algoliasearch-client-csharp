using Newtonsoft.Json.Linq;
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
using System.Globalization;
using Algolia.Search.Models;

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
            PrefixAll,
            /// <summary>
            /// Only the last word is interpreted as a prefix (default behavior).
            /// </summary>
            PrefixLast,
            /// <summary>
            /// No query word is interpreted as a prefix. This option is not recommended.
            /// </summary>
            PrefixNone
        }

        /// <summary>
        /// Remove words if no result.
        /// </summary>
        public enum RemoveWordsIfNoResult
        {
            /// <summary>
            /// No specific processing is done when a query does not return any result.
            /// </summary>
            None,
            /// <summary>
            /// When a query does not return any result, the final word will be removed until there is results.
            /// </summary>
            LastWords,
            /// <summary>
            /// When a query does not return any result, the first word will be removed until there is results.
            /// </summary>
            FirstWords,
            /// <summary>
            /// When a query does not return any result, a second trial will be made with all words as optional (which is equivalent to transforming the AND operand between query terms in a OR operand) 
            /// </summary>
            AllOptional

        }

        /// <summary>
        /// Typo tolerance.
        /// </summary>
        public enum TypoTolerance
        {
            /// <summary>
            /// the typo-tolerance is enabled and all matching hits are retrieved. (Default behavior)
            /// </summary>
            TypoTrue,
            /// <summary>
            /// the typo-tolerance is disabled.
            /// </summary>
            TypoFalse,
            /// <summary>
            ///  only keep the results with the minimum number of typos.
            /// </summary>
            TypoMin,
            /// <summary>
            /// hits matching with 2 typos are not retrieved if there are some matching without typos.
            /// </summary>
            TypoStrict
        }

        /// <summary>
        /// Create a new query.
        /// </summary>
        /// <param name="query">The query.</param>
        public Query(string query)
        {
            this._query = query;
            _customParameters = new Dictionary<string, string>();
        }

        /// <summary>
        /// Create a new query.
        /// </summary>
        public Query()
        {
            _customParameters = new Dictionary<string, string>();
        }

        /// <summary>
        /// Clone this query to a new query.
        /// </summary>
        /// <returns>The cloned query.</returns>
        public Query Clone()
        {
            Query q = new Query();
            q._advancedSyntax = _advancedSyntax;
	    q._removeStopWords = _removeStopWords;
            q._allowTyposOnNumericTokens = _allowTyposOnNumericTokens;
            q._analytics = _analytics;
            q._analyticsTags = _analyticsTags;
            q._aroundLatLong = _aroundLatLong;
            q._aroundLatLongViaIp = _aroundLatLongViaIp;
            q._attributes = _attributes;
            q._attributesToHighlight = _attributesToHighlight;
            q._noTypoToleranceOn = _noTypoToleranceOn;
            q._attributesToSnippet = _attributesToSnippet;
            q._responseFields = _responseFields;
            q._distinct = _distinct;
            q._facetingAfterDistinct = _facetingAfterDistinct;
            q._facetFilters = _facetFilters;
            q._facets = _facets;
            q._getRankingInfo = _getRankingInfo;
            q._hitsPerPage = _hitsPerPage;
            q._offset = _offset;
            q._length = _length;
            q._ignorePlural = _ignorePlural;
            q._insideBoundingBox = _insideBoundingBox;
            q._insidePolygon = _insidePolygon;
            q._maxValuesPerFacets = _maxValuesPerFacets;
            q._minWordSizeForApprox1 = _minWordSizeForApprox1;
            q._minWordSizeForApprox2 = _minWordSizeForApprox2;
            q._numerics = _numerics;
            q._optionalWords = _optionalWords;
            q._page = _page;
            q._query = _query;
	    q._similarQuery = _similarQuery;
            q._queryType = _queryType;
            q._removeWordsIfNoResult = _removeWordsIfNoResult;
            q._replaceSynonyms = _replaceSynonyms;
            q._restrictSearchableAttributes = _restrictSearchableAttributes;
            q._synonyms = _synonyms;
            q._tags = _tags;
            q._typoTolerance = _typoTolerance;
            q._filters = _filters;
            q._aroundRadius = _aroundRadius;
            q._aroundPrecision = _aroundPrecision;
            q._customParameters = _customParameters;
            return q;
        }

        /// <summary>
        /// Select how the query words are interpreted.
        /// </summary>
        public Query SetQueryType(QueryType type)
        {
            this._queryType = type;
            return this;
        }

        /// <summary>
        /// Select the spécific processing for the query.
        /// </summary>
        public Query SetRemoveWordsIfNoResult(RemoveWordsIfNoResult type)
        {
            this._removeWordsIfNoResult = type;
            return this;
        }

        /// <summary>
        /// Set the full text query.
        /// </summary>
        public Query SetQueryString(string query)
        {
            this._query = query;
            return this;
        }

        /// <summary>
        /// Set the full text similar query.
        /// </summary>
        public Query SetSimilarQueryString(string query)
        {
            this._similarQuery = query;
            return this;
	}

        /// <summary>
        /// Configure the precision of the proximity ranking criterion. By default, the minimum (and best) proximity value distance between 2 matching words is 1. Setting it to 2 (or 3) would allow 1 (or 2) words to be found between the matching words without degrading the proximity ranking value.
        ///
        /// Considering the query "javascript framework", if you set minProximity=2 the records "JavaScript framework" and "JavaScript charting framework" will get the same proximity score, even if the second one contains a word between the 2 matching words. Default to 1.
        /// </summary>
        public Query SetMinProximity(int value)
        {
            this._minProximity = value;
            return this;
        }

        /// <summary>
        /// Specify the string that is inserted before/after the highlighted parts in the query result (default to "<em>" / "</em>").
        /// </summary>
        public Query SetHighlightingTags(string preTag, string postTag)
        {
            this._highlightPreTag = preTag;
            this._highlightPostTag = postTag;
            return this;
        }

        /// <summary>
        /// Specify the string that is used as an ellipsis indicator when a snippet is truncated (defaults to the empty string).
        /// </summary>
        public Query SetSnippetEllipsisText(string snippetEllipsisText)
        {
            this._snippetEllipsisText = snippetEllipsisText;
            return this;
        }

        /// <summary>
        /// Specify the list of attribute names to retrieve.
        /// </summary>
        /// <param name="attributes">The attributes to retrieve.</param>
        /// <returns>Query for the attributes.</returns>
        public Query SetAttributesToRetrieve(IEnumerable<string> attributes)
        {
            this._attributes = attributes;
            return this;
        }

        /// <summary>
        /// List of attributes on which you want to disable typo tolerance (must be a subset of the searchableAttributes index setting).
        /// </summary>
        /// <param name="attributes">The list of attributes.</param>
        public Query DisableTypoToleranceOnAttributes(IEnumerable<string> attributes)
        {
            this._noTypoToleranceOn = attributes;
            return this;
        }

        /// <summary>
        /// Specify the list of attribute names to highlight.
        /// </summary>
        /// <param name="attributes">The attributes to highlight.</param>
        /// <returns>Query for the attributes.</returns>
        public Query SetAttributesToHighlight(IEnumerable<string> attributes)
        {
            this._attributesToHighlight = attributes;
            return this;
        }

        /// <summary>
        /// Specify the list of attribute names to Snippet alongside the number of words to return (syntax is 'attributeName:nbWords'). By default no snippet is computed.
        /// </summary>
        /// <param name="attributes">The attributes to Snippet.</param>
        /// <returns>Query for the attributes.</returns>
        public Query SetAttributesToSnippet(IEnumerable<string> attributes)
        {
            this._attributesToSnippet = attributes;
            return this;
        }

        /// <summary>
        /// Choose which fields the response will contain
        /// </summary>
        /// <param name="fields">Fields to retrieve</param>
        /// <returns>Query for the attributes.</returns>
        public Query SetFieldsToRetrieve(IEnumerable<string> fields)
        {
            this._responseFields = fields;
            return this;
        }

        /// <summary>
        /// Specify the minimum number of characters in a query word to accept one typo in this word. Defaults to 3.
        /// </summary>
        public Query SetMinWordSizeToAllowOneTypo(int nbChars)
        {
            _minWordSizeForApprox1 = nbChars;
            return this;
        }

        /// <summary>
        /// Specify the minimum number of characters in a query word to accept two typos in this word. Defaults to 7.
        /// </summary>
        public Query SetMinWordSizeToAllowTwoTypos(int nbChars)
        {
            _minWordSizeForApprox2 = nbChars;
            return this;
        }

        /// <summary>
        /// If set, the result hits will contain ranking information in _rankingInfo attribute.
        /// </summary>
        public Query GetRankingInfo(bool enabled)
        {
            _getRankingInfo = enabled;
            return this;
        }

        /// <summary>
        /// If set to true, plural won't be considered as a typo (for example car/cars will be considered as equals). Defaults to false.
        /// </summary>
        public Query IgnorePlural(bool ignorePluralsBool)
        {
            var ignorePlurals = new IgnorePluralsBool { Ignored = ignorePluralsBool };
            return IgnorePlural(ignorePlurals);
        }

        /// <summary>
        /// ignorePlural accept a comma separated string of languages "af,ar,az"
        /// </summary>
        public Query IgnorePlural(IIgnorePlurals ignorePlurals)
        {
            this._ignorePlural = ignorePlurals.GetValue();
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
            _distinct = enabled ? 1 : 0;
            return this;
        }

        /// <summary>
        /// Force faceting to be applied after de-duplication, 
        /// When using the distinct setting in combination with faceting, facet counts may be higher than expected. 
        /// This is because the engine computes faceting before applying de-duplication (distinct)
        /// https://www.algolia.com/doc/rest-api/search/#facetingafterdistinct
        /// </summary>
        /// <param name="enabled">If set to true, enable the facetingAfterDistinct feature (disabled by default)</param>
        /// <returns></returns>
        public Query EnableFacetingAfterDistinct(bool enabled)
        {
            _facetingAfterDistinct = enabled ? 1 : 0;
            return this;
        }

        /// <summary>
        /// This feature is similar to the distinct just before but instead of keeping the best value per value of attributeForDistinct, it allows to keep N values.
        /// </summary>
        /// <param name="nbHitsToKeep">Specify the maximum number of hits to keep for each distinct value
        /// <returns></returns>
        public Query EnableDistinct(int nbHitsToKeep)
        {
            _distinct = nbHitsToKeep;
            return this;
        }

        /// <summary>
        /// If set to false, this query will not be taken into account in analytics feature. Default to true.
        /// </summary>
        public Query EnableAnalytics(bool enabled)
        {
            _analytics = enabled;
            return this;
        }

        /// <summary>
        /// Tag the query with the specified identifiers
        /// </summary>
        public Query SetAnalyticsTags(IEnumerable<string> tags)
        {
            _analyticsTags = tags;
            return this;
        }

        /// <summary>
        ///  If set to false, this query will not use synonyms defined in configuration. Default to true.
        /// </summary>
        public Query EnableSynonyms(bool enabled)
        {
            _synonyms = enabled;
            return this;
        }

        /// <summary>
        /// If set to false, words matched via synonyms expansion will not be replaced by the matched synonym in highlight result. Default to true.
        /// </summary>
        public Query EnableReplaceSynonymsInHighlight(bool enabled)
        {
            _replaceSynonyms = enabled;
            return this;
        }

        /// <summary>
        /// If set to false, disable typo-tolerance. Default to true.
        /// </summary>
        public Query EnableTypoTolerance(bool enabled)
        {
            if (enabled)
            {
                _typoTolerance = TypoTolerance.TypoTrue;
            }
            else
            {
                _typoTolerance = TypoTolerance.TypoFalse;
            }
            return this;
        }

        /// <summary>
        /// This option allows you to control the number of typos in the result set.
        /// </summary>
        public Query SetTypoTolerance(TypoTolerance typoTolerance)
        {
            this._typoTolerance = typoTolerance;
            return this;
        }

        /// <summary>
        /// If set to false, disable typo-tolerance on numeric tokens. Default to true.
        /// </summary> 
        public Query EnableTyposOnNumericTokens(bool enabled)
        {
            _allowTyposOnNumericTokens = enabled;
            return this;
        }

        /// <summary>
        /// Set the page to retrieve (zero base). Defaults to 0.
        /// </summary>
        public Query SetPage(int page)
        {
            this._page = page;
            return this;
        }

        /// <summary>
        /// Set the number of hits per page. Defaults to 10.
        /// </summary>
        public Query SetNbHitsPerPage(int nbHitsPerPage)
        {
            this._hitsPerPage = nbHitsPerPage;
            return this;
        }

        /// <summary>
        /// Set the offset for the pagination.
        /// </summary>
        public Query SetOffset(int? offset)
        {
            this._offset = offset;
            return this;
        }

        /// <summary>
        /// Set the length for the pagination.
        /// </summary>
        public Query SetLength(int? length)
        {
            this._length = length;
            return this;
        }

        /// <summary>
        /// Set the the parameter that controls how the `exact` ranking criterion is computed when the query contains one word
        /// </summary>
        public Query ExactOnSingleWordQuery(string singleWordQuery)
        {
            this._exactOnSingleWordQuery = singleWordQuery;
            return this;
        }

        /// <summary>
        ///Specify the list of approximation that should be considered as an exact match in the ranking formula
        /// </summary>
        public Query AlternativesAsExact(string altExact)
        {
            this._alternativesAsExact = altExact;
            return this;
        }
        /// <summary>
        /// Search for entries around a given latitude/longitude with an automatic guessing of the radius depending of the area density 
        /// Note: at indexing, geoloc of an object should be set with _geoloc attribute containing lat and lng attributes (for example {"_geoloc":{"lat":48.853409, "lng":2.348800}})
        /// </summary>
        /// <param name="latitude">The latitude</param>
        /// <param name="longitude">The longitude</param>
        /// <returns></returns>
        public Query AroundLatitudeLongitude(float latitude, float longitude)
        {
            _aroundLatLong = "aroundLatLng=" + latitude.ToString(CultureInfo.InvariantCulture) + "," + longitude.ToString(CultureInfo.InvariantCulture);
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
        public Query AroundLatitudeLongitude(float latitude, float longitude, IAllRadius radius)
        {
            _aroundLatLong = "aroundLatLng=" + latitude.ToString(CultureInfo.InvariantCulture) + "," + longitude.ToString(CultureInfo.InvariantCulture);
            _aroundRadius = radius.GetValue();
            return this;
        }

        /// <summary>
        ///(BACKWARD COMPATIBILITY)Search for entries around a given latitude/longitude. 
        /// Note: at indexing, geoloc of an object should be set with _geoloc attribute containing lat and lng attributes (for example {"_geoloc":{"lat":48.853409, "lng":2.348800}})
        /// </summary>
        /// <param name="latitude">The latitude</param>
        /// <param name="longitude">The longitude</param>
        /// <param name="radius">set the maximum distance in meters.</param>
        /// <returns></returns>
        public Query AroundLatitudeLongitude(float latitude, float longitude, int radius)
        {
            var allRadius = new AllRadiusInt { Radius = radius };
            return AroundLatitudeLongitude(latitude, longitude, allRadius);
        }

        /// <summary>
        /// Search for entries around a given latitude/longitude (using IP geolocation) with an automatic radius depending of the density of the area
        /// Note: at indexing, geoloc of an object should be set with _geoloc attribute containing lat and lng attributes (for example {"_geoloc":{"lat":48.853409, "lng":2.348800}})
        /// </summary>
        /// <param name="radius">set the maximum distance in meters.</param>
        /// <returns></returns>
        public Query AroundLatitudeLongitudeViaIp()
        {
            _aroundLatLongViaIp = true;
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
        public Query AroundLatitudeLongitude(float latitude, float longitude, IAllRadius radius, int precision)
        {
            _aroundLatLong = "aroundLatLng=" + latitude.ToString(CultureInfo.InvariantCulture) + "," + longitude.ToString(CultureInfo.InvariantCulture);
            _aroundRadius = radius.GetValue();
            _aroundPrecision = precision;
            return this;
        }

        /// <summary>
        /// (BACKWARD COMPATIBILITY)Search for entries around a given latitude/longitude. 
        /// Note: at indexing, geoloc of an object should be set with _geoloc attribute containing lat and lng attributes (for example {"_geoloc":{"lat":48.853409, "lng":2.348800}})
        /// </summary>
        /// <param name="latitude">The latitude</param>
        /// <param name="longitude">The longitude</param>
        /// <param name="radius">set the maximum distance in meters.</param>
        /// <param name="precision">set the precision for ranking (for example if you set precision=100, two objects that are distant of less than 100m will be considered as identical for "geo" ranking parameter).</param>
        /// <returns></returns>
        public Query AroundLatitudeLongitude(float latitude, float longitude, int radius, int precision)
        {
            var allRadius = new AllRadiusInt { Radius = radius };
            return AroundLatitudeLongitude(latitude, longitude, allRadius, precision);
        }

        /// <summary>
        /// Search for entries around a given latitude/longitude (using IP geolocation).
        /// Note: at indexing, geoloc of an object should be set with _geoloc attribute containing lat and lng attributes (for example {"_geoloc":{"lat":48.853409, "lng":2.348800}})
        /// </summary>
        /// <param name="radius">set the maximum distance in meters.</param>
        /// <returns></returns>
        public Query AroundLatitudeLongitudeViaIp(IAllRadius radius)
        {
            _aroundRadius = radius.GetValue();
            _aroundLatLongViaIp = true;
            return this;
        }

        /// <summary>
        ///(BACKWARD COMPATIBILITY) Search for entries around a given latitude/longitude (using IP geolocation).
        /// Note: at indexing, geoloc of an object should be set with _geoloc attribute containing lat and lng attributes (for example {"_geoloc":{"lat":48.853409, "lng":2.348800}})
        /// </summary>
        /// <param name="radius">set the maximum distance in meters.</param>
        /// <returns></returns>
        public Query AroundLatitudeLongitudeViaIp(int radius)
        {
            var allRadius = new AllRadiusInt { Radius = radius };
            return AroundLatitudeLongitudeViaIp(allRadius);
        }

        /// <summary>
        /// Search for entries around a given latitude/longitude (using IP geolocation).
        /// Note: at indexing, geoloc of an object should be set with _geoloc attribute containing lat and lng attributes (for example {"_geoloc":{"lat":48.853409, "lng":2.348800}})
        /// </summary>
        /// <param name="radius">set the maximum distance in meters.</param>
        /// <param name="precision">set the precision for ranking (for example if you set precision=100, two objects that are distant of less than 100m will be considered as identical for "geo" ranking parameter).</param>
        /// <returns></returns>
        public Query AroundLatitudeLongitudeViaIp(IAllRadius radius, int precision)
        {
            _aroundRadius = radius.GetValue();
            _aroundPrecision = precision;
            _aroundLatLongViaIp = true;
            return this;
        }

        /// <summary>
        /// (BACKWARD COMPATIBILITY) Search for entries around a given latitude/longitude (using IP geolocation).
        /// Note: at indexing, geoloc of an object should be set with _geoloc attribute containing lat and lng attributes (for example {"_geoloc":{"lat":48.853409, "lng":2.348800}})
        /// </summary>
        /// <param name="radius">set the maximum distance in meters.</param>
        /// <param name="precision">set the precision for ranking (for example if you set precision=100, two objects that are distant of less than 100m will be considered as identical for "geo" ranking parameter).</param>
        /// <returns></returns>
        public Query AroundLatitudeLongitudeViaIp(int radius, int precision)
        {
            var allRadius = new AllRadiusInt { Radius = radius };
            return AroundLatitudeLongitudeViaIp(radius, precision);
        }

        /// <summary>
        /// Search for entries inside a given area defined by the two extreme points of a rectangle.
        /// At indexing, you should specify geoloc of an object with the _geoloc attribute (in the form "_geoloc":{"lat":48.853409, "lng":2.348800} or 
        /// "_geoloc":[{"lat":48.853409, "lng":2.348800},{"lat":48.547456, "lng":2.972075}] if you have several geo-locations in your record).
        /// 
        /// You can use several bounding boxes (OR) by calling this method several times.        
        /// </summary>
        /// <param name="latitudeP1"></param>
        /// <param name="longitudeP1"></param>
        /// <param name="latitudeP2"></param>
        /// <param name="longitudeP2"></param>
        /// <returns></returns>
        public Query InsideBoundingBox(float latitudeP1, float longitudeP1, float latitudeP2, float longitudeP2)
        {
            if (_insideBoundingBox != null) {
                _insideBoundingBox += latitudeP1.ToString(CultureInfo.InvariantCulture) + "," + longitudeP1.ToString(CultureInfo.InvariantCulture) + "," + latitudeP2.ToString(CultureInfo.InvariantCulture) + "," + longitudeP2.ToString(CultureInfo.InvariantCulture);
            } else {
                _insideBoundingBox = "insideBoundingBox=" + latitudeP1.ToString(CultureInfo.InvariantCulture) + "," + longitudeP1.ToString(CultureInfo.InvariantCulture) + "," + latitudeP2.ToString(CultureInfo.InvariantCulture) + "," + longitudeP2.ToString(CultureInfo.InvariantCulture);
            }
            return this;
        }

        /// <summary>
        /// Add a point to the polygon of geo-search (requires a minimum of three points to define a valid polygon)
        /// At indexing, you should specify geoloc of an object with the _geoloc attribute (in the form "_geoloc":{"lat":48.853409, "lng":2.348800} or 
        /// "_geoloc":[{"lat":48.853409, "lng":2.348800},{"lat":48.547456, "lng":2.972075}] if you have several geo-locations in your record).
        /// </summary>
        public Query AddInsidePolygon(float latitude, float longitude)
        {
            if (_insidePolygon != null) {
                _insidePolygon += "," + latitude.ToString(CultureInfo.InvariantCulture) + "," + longitude.ToString(CultureInfo.InvariantCulture);
            } else {
                _insidePolygon = "insidePolygon=" + latitude.ToString(CultureInfo.InvariantCulture) + "," + longitude.ToString(CultureInfo.InvariantCulture);
            }
            return this;
        }

        /// <summary>
        /// Change the radius or around latitude/longitude query
        /// <summary>
        public Query SetAroundRadius(IAllRadius radius)
        {
            _aroundRadius = radius.GetValue();
            return this;
        }

        /// <summary>
        /// (BACKWARD COMPATIBILITY) Change the radius or around latitude/longitude query
        /// <summary>
        public Query SetAroundRadius(int radius)
        {
            var allRadius = new AllRadiusInt { Radius = radius };
            return SetAroundRadius(allRadius);
        }

        /// <summary>
        /// Change the precision or around latitude/longitude query
        /// <summary>
        public Query SetAroundPrecision(int precision)
        {
            _aroundPrecision = precision;
            return this;
        }

        /// <summary>
        /// Filter the query with numeric, facet or/and tag filters. The syntax is a SQL like syntax, you can use the OR and AND keywords.
        /// The syntax for the underlying numeric, facet and tag filters is the same than in the other filters:
        /// available=1 AND (category:Book OR NOT category:Ebook) AND public
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public Query SetFilters(string filters)
        {
            this._filters = filters;
            return this;
        }

        /// <summary>
        /// Filter the query by a set of tags. You can AND tags by separating them by commas. To OR tags, you must add parentheses. For example tag1,(tag2,tag3) means tag1 AND (tag2 OR tag3).
        /// Note: at indexing, tags should be added in the _tags attribute of objects (for example {"_tags":["tag1","tag2"]} )
        /// <summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public Query SetTagFilters(string tags)
        {
            this._tags = tags;
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
            this._numerics = value;
            return this;
        }

        /// <summary>
        /// Set the list of words that should be considered as optional when found in the query.
        /// </summary>
        /// <param name="words">The list of optional words, comma separated.</param>
        /// <returns></returns>
        public Query SetOptionalWords(string words)
        {
            this._optionalWords = words;
            return this;
        }

        /// <summary>
        /// Filter the query by a list of facets.
        /// </summary>
        /// <param name="facets">Each facet is encoded as `attributeName:value`. For example: `["category:Book","author:John%20Doe"].</param>
        /// <returns></returns>
        public Query SetFacetFilters(IEnumerable<string> facets) {
            this._facetFilters = string.Join(",", facets);
            return this;
        }

        /// <summary>
        /// Filter the query by a facet.
        /// </summary>
        /// <param name="facets">The facet is encoded as `attributeName:value`.</param>
        /// <returns></returns>
        public Query SetFacetFilters(string facets)
        {
            this._facetFilters = facets;
            return this;
        }

        /// <summary>
        /// Filter the query by a list of facets.
        /// </summary>
        /// <param name="facets">Each facet is encoded as `attributeName:value`. For example: `["category:Book","author:John%20Doe"].</param>
        /// <returns></returns>
        public Query SetFacetFilters(JArray facets)
        {
            this._facetFilters = Newtonsoft.Json.JsonConvert.SerializeObject(facets);
            return this;
        }

        /// <summary>
        /// Set the max value per facet.
        /// </summary>
        /// <param name="numbers">The number to limit it by.</param>
        /// <returns></returns>
        public Query SetMaxValuesPerFacets(int numbers)
        {
            this._maxValuesPerFacets = numbers;
            return this;
        }

        /// <summary>
        /// List of attributes you want to use for textual search (must be a subset of the searchableAttributes index setting).
        /// </summary>
        /// <param name="attributes">Attributes are separated with a comma (for example @"name,address"). You can also use a JSON string array encoding (for example encodeURIComponent("[\"name\",\"address\"]")). By default, all attributes specified in searchableAttributes settings are used to search.</param>
        /// <returns></returns>
        public Query RestrictSearchableAttributes(string attributes)
        {
            this._restrictSearchableAttributes = attributes;
            return this;
        }

        /// <summary>
        /// Allows enabling of advanced syntax.
        /// </summary>
        /// <param name="enabled">Turn it on or off</param>
        /// <returns></returns>
        public Query EnableAdvancedSyntax(bool enabled)
        {
            this._advancedSyntax = enabled;
            return this;
        }


        /// <summary>
        /// Allows enabling of stop words removal.
        /// </summary>
        /// <param name="enabled">Turn it on or/off or providing a list of keywords</param>
        /// <returns></returns>
        public Query EnableRemoveStopWords(IEnabledRemoveStopWords enabled)
        {
            this._removeStopWords = enabled.GetValue();
            return this;
        }

        /// <summary>
        /// Allows enabling of stop words removal.
        /// </summary>
        /// <param name="enabled">Turn it on or/off or providing a list of keywords</param>
        /// <returns></returns>
        public Query EnableRemoveStopWords(bool enabled)
        {
            var removeStopWord = new EnabledRemoveStopWordsBool { Enabled = enabled };
           return EnableRemoveStopWords(removeStopWord);
        }

        /// <summary>
        /// Limit the search from a referer pattern. Only works on HTTPS
        /// </summary>
        /// <param name="facets">List of referers used to limit the search on a website.</param>
        /// <returns></returns>
        public Query SetReferers(string referers) {
            this._referers = referers;
            return this;
        }

        /// <summary>
        /// Set the key used for the rate-limit
        /// </summary>
        /// <param name="userToken">Identifier used for the rate-limit</param>
        /// <returns></returns>
        public Query SetUserToken(string userToken)
        {
            this._userToken = userToken;
            return this;
        }

        /// <summary>
        /// Restrict an API key to a list of indices
        /// </summary>
        /// <param name="indices"></param>
        /// <returns></returns>
        public Query SetRestrictIndices(IEnumerable<string> indices)
        {
            this._restrictIndices = indices;
            return this;
        }

        /// <summary>
        /// Restrict an API key to a specific IPv4
        /// </summary>
        /// <param name="sources"></param>
        /// <returns></returns>
        public Query SetRestrictSources(string sources)
        {
            this._restrictSources = sources;
            return this;
        }

        /// <summary>
        /// Set the object attributes that you want to use for faceting.
        /// </summary>
        /// <param name="facets">List of object attributes that you want to use for faceting. Only attributes that have been added in **attributesForFaceting** index setting can be used in this parameter. You can also use `*` to perform faceting on all attributes specified in **attributesForFaceting**.</param>
        /// <returns></returns>
        public Query SetFacets(IEnumerable<string> facets)
        {
            this._facets = facets;
            return this;
        }

        /// <summary>
        /// Add a custom query parameter
        /// </summary>
        /// <param name="name">The name of the custom parameter</param>
        /// <param name="value">The associated value</param>
        /// <returns></returns>
        public Query AddCustomParameter(string name, string value)
        {
            this._customParameters.Add(name, value);
            return this;
        }

        /// <summary>
        /// Get out the query as a string
        /// </summary>
        /// <returns></returns>
        public string GetQueryString() {
            string stringBuilder = "";
        
            if (_attributes != null) {
                stringBuilder += "attributesToRetrieve=";
                bool first = true;
                foreach (string attr in this._attributes) {
                    if (!first)
                        stringBuilder += ',';
                    stringBuilder += Uri.EscapeDataString(attr);
                    first = false;
                }
                if (this._attributes.Count() == 0)
                    stringBuilder += "[]";
            }
            if (_attributesToHighlight != null) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "attributesToHighlight=";
                bool first = true;
                foreach (string attr in this._attributesToHighlight) {
                    if (!first)
                        stringBuilder += ',';
                    stringBuilder += Uri.EscapeDataString(attr);
                    first = false;
                }
                if (this._attributesToHighlight.Count() == 0)
                    stringBuilder += "[]";
            }
            if (_noTypoToleranceOn != null) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "disableTypoToleranceOnAttributes=";
                bool first = true;
                foreach (string attr in this._noTypoToleranceOn) {
                    if (!first)
                        stringBuilder += ',';
                    stringBuilder += Uri.EscapeDataString(attr);
                    first = false;
                }
                if (this._noTypoToleranceOn.Count() == 0)
                    stringBuilder += "[]";
            }
            
            if (_attributesToSnippet != null) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "attributesToSnippet=";
                bool first = true;
                foreach (string attr in this._attributesToSnippet) {
                    if (!first)
                        stringBuilder += ',';
                    stringBuilder += Uri.EscapeDataString(attr);
                    first = false;
                }
                if (this._attributesToSnippet.Count() == 0)
                    stringBuilder += "[]";
            }
            if (_facets != null)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "facets=";
                bool first = true;
                foreach (string attr in this._facets)
                {
                    if (!first)
                        stringBuilder += ',';
                    stringBuilder += Uri.EscapeDataString(attr);
                    first = false;
                }
            }
            if (_facetFilters != null && _facetFilters.Length > 0)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "facetFilters=";
                stringBuilder += Uri.EscapeDataString(_facetFilters);
            }
            if (_filters != null)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "filters=";
                stringBuilder += Uri.EscapeDataString(_filters);
            }
            if (_maxValuesPerFacets.HasValue)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "maxValuesPerFacet=";
                stringBuilder += Newtonsoft.Json.JsonConvert.SerializeObject(_maxValuesPerFacets.Value);
            }
            if (_attributesToSnippet != null)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "attributesToSnippet=";
                bool first = true;
                foreach (string attr in this._attributesToSnippet)
                {
                    if (!first)
                        stringBuilder += ',';
                    stringBuilder += Uri.EscapeDataString(attr);
                    first = false;
                }
            }
            if (_responseFields != null)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "responseFields=";
                bool first = true;
                foreach (string attr in this._responseFields)
                {
                    if (!first)
                        stringBuilder += ',';
                    stringBuilder += Uri.EscapeDataString(attr);
                    first = false;
                }
                if (this._responseFields.Count() == 0)
                    stringBuilder += "[]";
            }

            if (!string.IsNullOrEmpty(_aroundRadius)) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "aroundRadius=";
                stringBuilder += _aroundRadius;

            }
            if (_aroundPrecision.HasValue) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "aroundPrecision=";
                stringBuilder += _aroundPrecision.Value.ToString();
            }
            if (_minWordSizeForApprox1.HasValue) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "minWordSizefor1Typo=";
                stringBuilder += _minWordSizeForApprox1.Value.ToString();
            }
            if (_minWordSizeForApprox2.HasValue) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "minWordSizefor2Typos=";
                stringBuilder += _minWordSizeForApprox2.Value.ToString();
            }
            if (_getRankingInfo.HasValue) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "getRankingInfo=";
                stringBuilder += _getRankingInfo.Value ? "true" : "false";
            }

            if (!string.IsNullOrEmpty(_ignorePlural))
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "ignorePlural=";
                stringBuilder += _ignorePlural;
            }

            if (_distinct.HasValue)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "distinct=";
		        stringBuilder += _distinct.Value.ToString();
            }

            if (_facetingAfterDistinct.HasValue)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "facetingAfterDistinct=";
                stringBuilder += _facetingAfterDistinct.Value.ToString();
            }

            if (_analytics.HasValue)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "analytics=";
                stringBuilder += _analytics.Value ? "true" : "false";
            }
            if (_analyticsTags != null)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "analyticsTags=";
                bool first = true;
                foreach (string attr in this._analyticsTags)
                {
                    if (!first)
                        stringBuilder += ',';
                    stringBuilder += Uri.EscapeDataString(attr);
                    first = false;
                }
            }
            if (_synonyms.HasValue)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "synonyms=";
                stringBuilder += _synonyms.Value ? "true" : "false";
            }
            if (_replaceSynonyms.HasValue)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "replaceSynonymsInHighlight=";
                stringBuilder += _replaceSynonyms.Value ? "true" : "false";
            }
            if (_typoTolerance.HasValue)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "typoTolerance=";
                switch (_typoTolerance) {
                    case TypoTolerance.TypoFalse:
                        stringBuilder += "false";
                        break;
                    case TypoTolerance.TypoMin:
                        stringBuilder += "min";
                        break;
                    case TypoTolerance.TypoStrict:
                        stringBuilder += "strict";
                        break;
                    case TypoTolerance.TypoTrue:
                        stringBuilder += "true";
                        break;
                }
            }
            if (_allowTyposOnNumericTokens.HasValue)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "allowTyposOnNumericTokens=";
                stringBuilder += _allowTyposOnNumericTokens.Value ? "true" : "false";
            }
            if (_advancedSyntax.HasValue)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "advancedSyntax=";
                stringBuilder += _advancedSyntax.Value ? "1" : "0";
            }
            if (!string.IsNullOrEmpty(_removeStopWords))
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "removeStopWords=";
                stringBuilder += _removeStopWords;
            }
            if (_page.HasValue) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "page=";
                stringBuilder += _page.Value.ToString();
            }
            if (_hitsPerPage.HasValue) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "hitsPerPage=";
                stringBuilder += _hitsPerPage.Value.ToString();
            }
            if (_length.HasValue)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "length=";
                stringBuilder += _length.Value.ToString();
            }
            if (_offset.HasValue)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "offset=";
                stringBuilder += _offset.Value.ToString();
            }
            if (_tags != null) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "tagFilters=";
                stringBuilder += Uri.EscapeDataString(_tags);
            }
            if (_numerics != null) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "numericFilters=";
                stringBuilder += Uri.EscapeDataString(_numerics);
            }
            if (_insideBoundingBox != null) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += _insideBoundingBox;
            } else if (_aroundLatLong != null) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += _aroundLatLong;
            } else if (_insidePolygon != null) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += _insidePolygon;
            }
            if (_aroundLatLongViaIp.HasValue) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "aroundLatLngViaIP=";
                stringBuilder += _aroundLatLongViaIp.Value ? "true" : "false";
            }
            if (_minProximity.HasValue) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "minProximity=";
                stringBuilder += _minProximity.Value.ToString();
            }
            if (_highlightPreTag != null && _highlightPostTag != null) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "highlightPreTag=";   
                stringBuilder += _highlightPreTag;
                stringBuilder += "&highlightPostTag=";   
                stringBuilder += _highlightPostTag;
            }
            if (_snippetEllipsisText != null) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "snippetEllipsisText=";
                stringBuilder += Uri.EscapeDataString(_snippetEllipsisText);
            }
            if (_query != null) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "query=";
                stringBuilder += Uri.EscapeDataString(_query);
            }
            if (_similarQuery != null) {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "similarQuery=";
                stringBuilder += Uri.EscapeDataString(_similarQuery);
            }

            if (_optionalWords != null)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "optionalWords=";
                stringBuilder += Uri.EscapeDataString(_optionalWords);
            }

            if (!string.IsNullOrEmpty(_exactOnSingleWordQuery))
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "exactOnSingleWordQuery=";
                stringBuilder += _exactOnSingleWordQuery;
            }

            if (!string.IsNullOrEmpty(_alternativesAsExact))
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "alternativesAsExact=";
                stringBuilder += _alternativesAsExact;
            }

            if (_restrictSearchableAttributes != null)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "restrictSearchableAttributes=";
                stringBuilder += Uri.EscapeDataString(_restrictSearchableAttributes);
            }
            if (_removeWordsIfNoResult.HasValue)
            {
                switch (_removeWordsIfNoResult)
                {
                    case RemoveWordsIfNoResult.None:
                        if (stringBuilder.Length > 0)
                            stringBuilder += '&';
                        stringBuilder += "removeWordsIfNoResult=None";
                        break;
                    case RemoveWordsIfNoResult.FirstWords:
                        if (stringBuilder.Length > 0)
                            stringBuilder += '&';
                        stringBuilder += "removeWordsIfNoResult=FirstWords";
                        break;
                    case RemoveWordsIfNoResult.LastWords:
                        if (stringBuilder.Length > 0)
                            stringBuilder += '&';
                        stringBuilder += "removeWordsIfNoResult=LastWords";
                        break;
                    case RemoveWordsIfNoResult.AllOptional:
                        if (stringBuilder.Length > 0)
                            stringBuilder += '&';
                        stringBuilder += "removeWordsIfNoResult=allOptional";
                        break;
                }
            }
            if (_queryType.HasValue)
            {
                switch (_queryType)
                {
                    case QueryType.PrefixAll:
                        if (stringBuilder.Length > 0)
                            stringBuilder += '&';
                        stringBuilder += "queryType=prefixAll";
                        break;
                    case QueryType.PrefixLast:
                        if (stringBuilder.Length > 0)
                            stringBuilder += '&';
                        stringBuilder += "queryType=prefixLast";
                        break;
                    case QueryType.PrefixNone:
                        if (stringBuilder.Length > 0)
                            stringBuilder += '&';
                        stringBuilder += "queryType=prefixNone";
                        break;
                }
            }
            if (_userToken != null)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "userToken=";
                stringBuilder +=  Uri.EscapeDataString(_userToken);
            }
            if (_restrictIndices != null)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "restrictIndices=";
                bool first = true;
                foreach (string attr in this._restrictIndices)
                {
                    if (!first)
                        stringBuilder += ',';
                    stringBuilder += Uri.EscapeDataString(attr);
                    first = false;
                }
            }
            if (_restrictSources != null)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "restrictSources=";
                stringBuilder += Uri.EscapeDataString(_restrictSources);
            }
            if (_referers != null)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder += '&';
                stringBuilder += "referer=";
                stringBuilder +=  Uri.EscapeDataString(_referers);
            }
            if (_customParameters.Count > 0)
            {
                foreach (KeyValuePair<string, string> elt in _customParameters)
                {
                    if (stringBuilder.Length > 0)
                        stringBuilder += '&';
                    stringBuilder += elt.Key;
                    stringBuilder += "=";
                    stringBuilder += Uri.EscapeDataString(elt.Value);
                }
            }
            return stringBuilder;
        }

        private IDictionary<string, string> _customParameters;
        private IEnumerable<string> _attributes;
        private IEnumerable<string> _attributesToHighlight;
        private IEnumerable<string> _noTypoToleranceOn;
        private IEnumerable<string> _attributesToSnippet;
        private IEnumerable<string> _responseFields;
        private IEnumerable<string> _analyticsTags;
        private string _exactOnSingleWordQuery;
        private string _alternativesAsExact;
        private int? _minWordSizeForApprox1;
        private int? _aroundPrecision;
        private string _aroundRadius;
        private int? _minWordSizeForApprox2;
        private bool? _getRankingInfo;
        private string _ignorePlural;
        private int? _distinct;
        private int? _facetingAfterDistinct;
        private bool? _advancedSyntax;
        private string _removeStopWords;
        private bool? _analytics;
        private bool? _synonyms;
        private bool? _replaceSynonyms;
        private TypoTolerance? _typoTolerance;
        private bool? _allowTyposOnNumericTokens;
        private int? _page;
        private int? _hitsPerPage;
        private int? _offset;
        private int? _length;
        private string _filters;
        private string _tags;
        private string _numerics;
        private string _insideBoundingBox;
        private string _insidePolygon;
        private string _aroundLatLong;
        private bool? _aroundLatLongViaIp;
        private string _query;
        private string _similarQuery;
        private string _highlightPreTag;
        private string _highlightPostTag;
        private string _snippetEllipsisText;
        private int? _minProximity;
        private string _optionalWords;
        private QueryType? _queryType;
        private RemoveWordsIfNoResult? _removeWordsIfNoResult;
        private IEnumerable<string> _facets;
        private string _facetFilters;
        private int? _maxValuesPerFacets;
        private string _restrictSearchableAttributes;
        private string _userToken;
        private IEnumerable<string> _restrictIndices;
        private string _restrictSources;
        private string _referers;
    }
}
