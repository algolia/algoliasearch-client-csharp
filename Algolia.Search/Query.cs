using Newtonsoft.Json.Linq;
/*
 * Copyright (c) 2018 Algolia
 * https://www.algolia.com/
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
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using Algolia.Search.Models;
using System.Net;

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
			FIRST_WORDS,
			/// <summary>
			/// When a query does not return any result, a second trial will be made with all words as optional (which is equivalent to transforming the AND operand between query terms in a OR operand) 
			/// </summary>
			ALL_OPTIONAL

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
		/// Values applicable to the sortFacetValuesBy parameter.
		/// </summary>
		public enum SortFacetValuesBy
		{
			/// <summary>
			/// Facet values are sorted by decreasing count, the count being the number of records containing this facet value in the results of the query (default behavior).
			/// </summary>
			COUNT,
			/// <summary>
			/// /// Facet values are sorted by increasing alphabetical order.
			/// </summary>
			ALPHA,
		}

		/// <summary>
		/// Create a new query.
		/// </summary>
		/// <param name="query">The query.</param>
		public Query(string query)
		{
			this.query = query;
			customParameters = new Dictionary<string, string>();
		}

		/// <summary>
		/// Create a new query.
		/// </summary>
		public Query()
		{
			customParameters = new Dictionary<string, string>();
		}

		/// <summary>
		/// Clone this query to a new query.
		/// </summary>
		/// <returns>The cloned query.</returns>
		public Query clone()
		{
			Query q = new Query();
			q.advancedSyntax = advancedSyntax;
			q.removeStopWords = removeStopWords;
			q.allowTyposOnNumericTokens = allowTyposOnNumericTokens;
			q.analytics = analytics;
			q.analyticsTags = analyticsTags;
			q.percentileComputation = percentileComputation;
			q.aroundLatLong = aroundLatLong;
			q.aroundLatLongViaIP = aroundLatLongViaIP;
			q.attributes = attributes;
			q.attributesToHighlight = attributesToHighlight;
			q.noTypoToleranceOn = noTypoToleranceOn;
			q.attributesToSnippet = attributesToSnippet;
			q.responseFields = responseFields;
			q.distinct = distinct;
			q.facetingAfterDistinct = facetingAfterDistinct;
			q.facetFilters = facetFilters;
			q.facets = facets;
			q.getRankingInfo = getRankingInfo;
			q.hitsPerPage = hitsPerPage;
			q.offset = offset;
			q.validUntil = validUntil;
			q.length = length;
			q.ignorePlural = ignorePlural;
			q.insideBoundingBox = insideBoundingBox;
			q.insidePolygon = insidePolygon;
			q.maxValuesPerFacets = maxValuesPerFacets;
			q.maxFacetHits = maxFacetHits;
			q.minWordSizeForApprox1 = minWordSizeForApprox1;
			q.minWordSizeForApprox2 = minWordSizeForApprox2;
			q.numerics = numerics;
			q.optionalWords = optionalWords;
			q.page = page;
			q.query = query;
			q.similarQuery = similarQuery;
			q.queryType = queryType;
			q.sortFacetValuesBy = sortFacetValuesBy;
			q.removeWordsIfNoResult = removeWordsIfNoResult;
			q.replaceSynonyms = replaceSynonyms;
			q.restrictSearchableAttributes = restrictSearchableAttributes;
			q.synonyms = synonyms;
			q.tags = tags;
			q.typoTolerance = typoTolerance;
			q.filters = filters;
			q.aroundRadius = aroundRadius;
			q.aroundPrecision = aroundPrecision;
			q.customParameters = customParameters;
			q.enableRules = enableRules;
			q.ruleContexts = ruleContexts;

			return q;
		}

        /// <summary>
        /// Select how the query words are interpreted.
        /// </summary>
        /// <param name="type"></param>
        public Query SetQueryType(QueryType type)
		{
			queryType = type;
			return this;
		}

        /// <summary>
        /// Controls how the facet values are sorted within each faceted attribute.
        /// </summary>
        /// <param name="sortFacetValuesBy"></param>
        public Query SetSortFacetValuesBy(SortFacetValuesBy sortFacetValuesBy)
		{
			this.sortFacetValuesBy = sortFacetValuesBy;
			return this;
		}

        /// <summary>
        /// Select the spécific processing for the query.
        /// </summary>
        /// <param name="type"></param>
        public Query SetRemoveWordsIfNoResult(RemoveWordsIfNoResult type)
		{
			removeWordsIfNoResult = type;
			return this;
		}

        /// <summary>
        /// Set the full text query.
        /// </summary>
        /// <param name="query"></param>
        public Query SetQueryString(string query)
		{
			this.query = query;
			return this;
		}

        /// <summary>
        /// Set the full text similar query.
        /// </summary>
        /// <param name="query"></param>
        public Query SetSimilarQueryString(string query)
		{
			similarQuery = query;
			return this;
		}

        /// <summary>
        /// Configure the precision of the proximity ranking criterion. By default, the minimum (and best) proximity value distance between 2 matching words is 1. Setting it to 2 (or 3) would allow 1 (or 2) words to be found between the matching words without degrading the proximity ranking value.
        ///
        /// Considering the query "javascript framework", if you set minProximity=2 the records "JavaScript framework" and "JavaScript charting framework" will get the same proximity score, even if the second one contains a word between the 2 matching words. Default to 1.
        /// </summary>
        /// <param name="value"></param>
        public Query SetMinProximity(int value)
		{
			minProximity = value;
			return this;
		}

        /// <summary>
        /// Specify the string that is inserted before/after the highlighted parts in the query result (default to "<em>" / "</em>").
        /// </summary>
        /// <param name="preTag"></param>
        /// <param name="postTag"></param>
        public Query SetHighlightingTags(string preTag, string postTag)
		{
			highlightPreTag = preTag;
			highlightPostTag = postTag;
			return this;
		}

        /// <summary>
        /// Specify the string that is used as an ellipsis indicator when a snippet is truncated (defaults to the empty string).
        /// </summary>
        /// <param name="snippetEllipsisText"></param>
        public Query SetSnippetEllipsisText(string snippetEllipsisText)
		{
			this.snippetEllipsisText = snippetEllipsisText;
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
		/// List of attributes on which you want to disable typo tolerance (must be a subset of the searchableAttributes index setting).
		/// </summary>
		/// <param name="attributes">The list of attributes.</param>
		public Query DisableTypoToleranceOnAttributes(IEnumerable<string> attributes)
		{
			noTypoToleranceOn = attributes;
			return this;
		}

		/// <summary>
		/// Specify the list of attribute names to highlight.
		/// </summary>
		/// <param name="attributes">The attributes to highlight.</param>
		/// <returns>Query for the attributes.</returns>
		public Query SetAttributesToHighlight(IEnumerable<string> attributes)
		{
			attributesToHighlight = attributes;
			return this;
		}

		/// <summary>
		/// Specify the list of attribute names to Snippet alongside the number of words to return (syntax is 'attributeName:nbWords'). By default no snippet is computed.
		/// </summary>
		/// <param name="attributes">The attributes to Snippet.</param>
		/// <returns>Query for the attributes.</returns>
		public Query SetAttributesToSnippet(IEnumerable<string> attributes)
		{
			attributesToSnippet = attributes;
			return this;
		}

		/// <summary>
		/// Choose which fields the response will contain
		/// </summary>
		/// <param name="fields">Fields to retrieve</param>
		/// <returns>Query for the attributes.</returns>
		public Query SetFieldsToRetrieve(IEnumerable<string> fields)
		{
			responseFields = fields;
			return this;
		}

        /// <summary>
        /// Specify the minimum number of characters in a query word to accept one typo in this word. Defaults to 3.
        /// </summary>
        /// <param name="nbChars"></param>
        public Query SetMinWordSizeToAllowOneTypo(int nbChars)
		{
			minWordSizeForApprox1 = nbChars;
			return this;
		}

        /// <summary>
        /// Specify the minimum number of characters in a query word to accept two typos in this word. Defaults to 7.
        /// </summary>
        /// <param name="nbChars"></param>
        public Query SetMinWordSizeToAllowTwoTypos(int nbChars)
		{
			minWordSizeForApprox2 = nbChars;
			return this;
		}

        /// <summary>
        /// If set, the result hits will contain ranking information in _rankingInfo attribute.
        /// </summary>
        /// <param name="enabled"></param>
        public Query GetRankingInfo(bool enabled)
		{
			getRankingInfo = enabled;
			return this;
		}

        /// <summary>
        /// If set to true, plural won't be considered as a typo (for example car/cars will be considered as equals). Defaults to false.
        /// </summary>
        /// <param name="ignorePluralsBool"></param>
        public Query IgnorePlural(bool ignorePluralsBool)
		{
			var ignorePlurals = new IgnorePluralsBool { Ignored = ignorePluralsBool };
			return IgnorePlural(ignorePlurals);
		}

        /// <summary>
        /// ignorePlural accept a comma separated string of languages "af,ar,az"
        /// </summary>
        /// <param name="ignorePlurals"></param>
        public Query IgnorePlural(IIgnorePlurals ignorePlurals)
		{
			ignorePlural = ignorePlurals.GetValue();
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
			distinct = enabled ? 1 : 0;
			return this;
		}

		/// <summary>
		/// Enables the computation of the processing time percentile
		/// </summary>
		/// <param name="enabled"></param>When true, the API records the processing time of the search query and includes it when computing the 90% and 99% percentiles, available in your Algolia dashboard.
		/// <returns></returns>
		public Query EnablePercentileComputation(bool enabled)
		{
			percentileComputation = enabled;
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
			facetingAfterDistinct = enabled ? 1 : 0;
			return this;
		}

		/// <summary>
		/// This feature is similar to the distinct just before but instead of keeping the best value per value of attributeForDistinct, it allows to keep N values.
		/// </summary>
		/// <param name="nbHitsToKeep">Specify the maximum number of hits to keep for each distinct value</param>
		/// <returns></returns>
		public Query EnableDistinct(int nbHitsToKeep)
		{
			distinct = nbHitsToKeep;
			return this;
		}

        /// <summary>
        /// If set to false, this query will not be taken into account in analytics feature. Default to true.
        /// </summary>
        /// <param name="enabled"></param>
        public Query EnableAnalytics(bool enabled)
		{
			analytics = enabled;
			return this;
		}

        /// <summary>
        /// Tag the query with the specified identifiers
        /// </summary>
        /// <param name="tags"></param>
        public Query SetAnalyticsTags(IEnumerable<string> tags)
		{
			analyticsTags = tags;
			return this;
		}

        /// <summary>
        ///  If set to false, this query will not use synonyms defined in configuration. Default to true.
        /// </summary>
        /// <param name="enabled"></param>
        public Query EnableSynonyms(bool enabled)
		{
			synonyms = enabled;
			return this;
		}

        /// <summary>
        /// If set to false, words matched via synonyms expansion will not be replaced by the matched synonym in highlight result. Default to true.
        /// </summary>
        /// <param name="enabled"></param>
        public Query EnableReplaceSynonymsInHighlight(bool enabled)
		{
			replaceSynonyms = enabled;
			return this;
		}

        /// <summary>
        /// If set to false, disable typo-tolerance. Default to true.
        /// </summary>
        /// <param name="enabled"></param>
        public Query EnableTypoTolerance(bool enabled)
		{
		    typoTolerance = enabled ? TypoTolerance.TYPO_TRUE : TypoTolerance.TYPO_FALSE;
		    return this;
		}

        /// <summary>
        /// This option allows you to control the number of typos in the result set.
        /// </summary>
        /// <param name="typoTolerance">If set to false, disable typo-tolerance. Default to true.</param>
        public Query SetTypoTolerance(TypoTolerance typoTolerance)
		{
			this.typoTolerance = typoTolerance;
			return this;
		}

        /// <summary>
        /// If set to false, disable typo-tolerance on numeric tokens. Default to true.
        /// </summary>
        /// <param name="enabled"></param> 
        public Query EnableTyposOnNumericTokens(bool enabled)
		{
			allowTyposOnNumericTokens = enabled;
			return this;
		}

        /// <summary>
        /// Set the page to retrieve (zero base). Defaults to 0.
        /// </summary>
        /// <param name="page"></param>
        public Query SetPage(int page)
		{
			this.page = page;
			return this;
		}

        /// <summary>
        /// Set the number of hits per page. Defaults to 10.
        /// </summary>
        /// <param name="nbHitsPerPage"></param>
        public Query SetNbHitsPerPage(int nbHitsPerPage)
		{
			hitsPerPage = nbHitsPerPage;
			return this;
		}

        /// <summary>
        /// Set the offset for the pagination.
        /// </summary>
        /// <param name="offset"></param>
        public Query SetOffset(int? offset)
		{
			this.offset = offset;
			return this;
		}

        /// <summary>
        /// Unix timestamp used to define the expiration date of the API key.
        /// </summary>
        /// <param name="validUntil"></param>
        public Query SetValidUntil(int? validUntil)
		{
			this.validUntil = validUntil;
			return this;
		}

        /// <summary>
        /// Set the length for the pagination.
        /// </summary>
        /// <param name="length"></param>
        public Query SetLength(int? length)
		{
			this.length = length;
			return this;
		}

        /// <summary>
        /// Set the the parameter that controls how the `exact` ranking criterion is computed when the query contains one word
        /// </summary>
        /// <param name="singleWordQuery"></param>
        public Query ExactOnSingleWordQuery(string singleWordQuery)
		{
			exactOnSingleWordQuery = singleWordQuery;
			return this;
		}

        /// <summary>
        ///Specify the list of approximation that should be considered as an exact match in the ranking formula
        /// </summary>
        /// <param name="altExact"></param>
        public Query AlternativesAsExact(string altExact)
		{
			alternativesAsExact = altExact;
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
			aroundLatLong = $"aroundLatLng={latitude.ToString(CultureInfo.InvariantCulture)},{longitude.ToString(CultureInfo.InvariantCulture)}";
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
			aroundLatLong = $"aroundLatLng={latitude.ToString(CultureInfo.InvariantCulture)},{longitude.ToString(CultureInfo.InvariantCulture)}";
			aroundRadius = radius.GetValue();
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
            return AroundLatitudeLongitude(latitude, longitude, new AllRadiusInt { Radius = radius });
		}

		/// <summary>
		/// Search for entries around a given latitude/longitude (using IP geolocation) with an automatic radius depending of the density of the area
		/// Note: at indexing, geoloc of an object should be set with _geoloc attribute containing lat and lng attributes (for example {"_geoloc":{"lat":48.853409, "lng":2.348800}})
		/// </summary>
		/// <param name="radius">set the maximum distance in meters.</param>
		/// <returns></returns>
		public Query AroundLatitudeLongitudeViaIP()
		{
			aroundLatLongViaIP = true;
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
			aroundLatLong = $"aroundLatLng={latitude.ToString(CultureInfo.InvariantCulture)},{longitude.ToString(CultureInfo.InvariantCulture)}";
			aroundRadius = radius.GetValue();
			aroundPrecision = precision;
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
            return AroundLatitudeLongitude(latitude, longitude, new AllRadiusInt { Radius = radius }, precision);
		}

		/// <summary>
		/// Search for entries around a given latitude/longitude (using IP geolocation).
		/// Note: at indexing, geoloc of an object should be set with _geoloc attribute containing lat and lng attributes (for example {"_geoloc":{"lat":48.853409, "lng":2.348800}})
		/// </summary>
		/// <param name="radius">set the maximum distance in meters.</param>
		/// <returns></returns>
		public Query AroundLatitudeLongitudeViaIP(IAllRadius radius)
		{
			aroundRadius = radius.GetValue();
			aroundLatLongViaIP = true;
			return this;
		}

		/// <summary>
		///(BACKWARD COMPATIBILITY) Search for entries around a given latitude/longitude (using IP geolocation).
		/// Note: at indexing, geoloc of an object should be set with _geoloc attribute containing lat and lng attributes (for example {"_geoloc":{"lat":48.853409, "lng":2.348800}})
		/// </summary>
		/// <param name="radius">set the maximum distance in meters.</param>
		/// <returns></returns>
		public Query AroundLatitudeLongitudeViaIP(int radius)
		{
            return AroundLatitudeLongitudeViaIP(new AllRadiusInt { Radius = radius });
		}

		/// <summary>
		/// Search for entries around a given latitude/longitude (using IP geolocation).
		/// Note: at indexing, geoloc of an object should be set with _geoloc attribute containing lat and lng attributes (for example {"_geoloc":{"lat":48.853409, "lng":2.348800}})
		/// </summary>
		/// <param name="radius">set the maximum distance in meters.</param>
		/// <param name="precision">set the precision for ranking (for example if you set precision=100, two objects that are distant of less than 100m will be considered as identical for "geo" ranking parameter).</param>
		/// <returns></returns>
		public Query AroundLatitudeLongitudeViaIP(IAllRadius radius, int precision)
		{
			aroundRadius = radius.GetValue();
			aroundPrecision = precision;
			aroundLatLongViaIP = true;
			return this;
		}

		/// <summary>
		/// (BACKWARD COMPATIBILITY) Search for entries around a given latitude/longitude (using IP geolocation).
		/// Note: at indexing, geoloc of an object should be set with _geoloc attribute containing lat and lng attributes (for example {"_geoloc":{"lat":48.853409, "lng":2.348800}})
		/// </summary>
		/// <param name="radius">set the maximum distance in meters.</param>
		/// <param name="precision">set the precision for ranking (for example if you set precision=100, two objects that are distant of less than 100m will be considered as identical for "geo" ranking parameter).</param>
		/// <returns></returns>
		public Query AroundLatitudeLongitudeViaIP(int radius, int precision)
		{
			return AroundLatitudeLongitudeViaIP(radius, precision);
		}

		/// <summary>
		/// <para>
		/// Search for entries inside a given area defined by the two extreme points of a rectangle.
		/// At indexing, you should specify geoloc of an object with the _geoloc attribute (in the form "_geoloc":{"lat":48.853409, "lng":2.348800} or 
		/// "_geoloc":[{"lat":48.853409, "lng":2.348800},{"lat":48.547456, "lng":2.972075}] if you have several geo-locations in your record).
		/// </para>
		/// <para>You can use several bounding boxes (OR) by calling this method several times.        </para>
		/// </summary>
		/// <param name="latitudeP1"></param>
		/// <param name="longitudeP1"></param>
		/// <param name="latitudeP2"></param>
		/// <param name="longitudeP2"></param>
		/// <returns></returns>
		public Query InsideBoundingBox(float latitudeP1, float longitudeP1, float latitudeP2, float longitudeP2)
		{
			if (insideBoundingBox != null)
			{
				insideBoundingBox += $"{latitudeP1.ToString(CultureInfo.InvariantCulture)},{longitudeP1.ToString(CultureInfo.InvariantCulture)},{latitudeP2.ToString(CultureInfo.InvariantCulture)},{longitudeP2.ToString(CultureInfo.InvariantCulture)}";
			}
			else
			{
				insideBoundingBox = $"insideBoundingBox={latitudeP1.ToString(CultureInfo.InvariantCulture)},{longitudeP1.ToString(CultureInfo.InvariantCulture)},{latitudeP2.ToString(CultureInfo.InvariantCulture)},{longitudeP2.ToString(CultureInfo.InvariantCulture)}";
			}
			return this;
		}

        /// <summary>
        /// Add a point to the polygon of geo-search (requires a minimum of three points to define a valid polygon)
        /// At indexing, you should specify geoloc of an object with the _geoloc attribute (in the form "_geoloc":{"lat":48.853409, "lng":2.348800} or 
        /// "_geoloc":[{"lat":48.853409, "lng":2.348800},{"lat":48.547456, "lng":2.972075}] if you have several geo-locations in your record).
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        public Query AddInsidePolygon(float latitude, float longitude)
		{
			if (insidePolygon != null)
			{
				insidePolygon += $",{latitude.ToString(CultureInfo.InvariantCulture)},{longitude.ToString(CultureInfo.InvariantCulture)}";
			}
			else
			{
				insidePolygon = $"insidePolygon={latitude.ToString(CultureInfo.InvariantCulture)},{longitude.ToString(CultureInfo.InvariantCulture)}";
			}
			return this;
		}

        /// <summary>
        /// Change the radius or around latitude/longitude query
        /// </summary>
        public Query SetAroundRadius(IAllRadius radius)
		{
			aroundRadius = radius.GetValue();
			return this;
		}

        /// <summary>
        /// (BACKWARD COMPATIBILITY) Change the radius or around latitude/longitude query
        /// </summary>
        public Query SetAroundRadius(int radius)
		{
            return SetAroundRadius(new AllRadiusInt { Radius = radius });
		}

        /// <summary>
        /// Change the precision or around latitude/longitude query
        /// </summary>
        public Query setAroundPrecision(int precision)
		{
			aroundPrecision = precision;
			return this;
		}

        /// <summary>
        /// Filter the query with numeric, facet or/and tag filters. The syntax is a SQL like syntax, you can use the OR and AND keywords.
        /// The syntax for the underlying numeric, facet and tag filters is the same than in the other filters:
        /// available=1 AND (category:Book OR NOT category:Ebook) AND public
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public Query SetFilters(string filters)
		{
			this.filters = filters;
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
        /// <param name="value"></param>
        public Query SetNumericFilters(string value)
		{
			numerics = value;
			return this;
		}

		/// <summary>
		/// Set the list of words that should be considered as optional when found in the query.
		/// </summary>
		/// <param name="words">The list of optional words, comma separated.</param>
		/// <returns></returns>
		public Query SetOptionalWords(string words)
		{
			optionalWords = words;
			return this;
		}

		/// <summary>
		/// Filter the query by a list of facets.
		/// </summary>
		/// <param name="facets">Each facet is encoded as `attributeName:value`. For example: `["category:Book","author:John%20Doe"].</param>
		/// <returns></returns>
		public Query SetFacetFilters(IEnumerable<string> facets)
		{
			facetFilters = string.Join(",", facets);
			return this;
		}

		/// <summary>
		/// Filter the query by a facet.
		/// </summary>
		/// <param name="facets">The facet is encoded as `attributeName:value`.</param>
		/// <returns></returns>
		public Query SetFacetFilters(string facets)
		{
			facetFilters = facets;
			return this;
		}

		/// <summary>
		/// Filter the query by a list of facets.
		/// </summary>
		/// <param name="facets">Each facet is encoded as `attributeName:value`. For example: `["category:Book","author:John%20Doe"].</param>
		/// <returns></returns>
		public Query SetFacetFilters(JArray facets)
		{
			facetFilters = Newtonsoft.Json.JsonConvert.SerializeObject(facets);
			return this;
		}

		/// <summary>
		/// Set the max value per facet.
		/// </summary>
		/// <param name="numbers">The number to limit it by.</param>
		/// <returns></returns>
		public Query SetMaxValuesPerFacets(int numbers)
		{
			maxValuesPerFacets = numbers;
			return this;
		}

		/// <summary>
		/// Set the max number of facet hits to return during a search for facet values.
		/// </summary>
		/// <param name="numbers">The number to limit it by.</param>
		/// <returns></returns>
		public Query SetMaxFacetHits(int numbers)
		{
			maxFacetHits = numbers;
			return this;
		}

		/// <summary>
		/// List of attributes you want to use for textual search (must be a subset of the searchableAttributes index setting).
		/// </summary>
		/// <param name="attributes">Attributes are separated with a comma (for example @"name,address"). You can also use a JSON string array encoding (for example encodeURIComponent("[\"name\",\"address\"]")). By default, all attributes specified in searchableAttributes settings are used to search.</param>
		/// <returns></returns>
		public Query RestrictSearchableAttributes(string attributes)
		{
			restrictSearchableAttributes = attributes;
			return this;
		}

		/// <summary>
		/// Allows enabling of advanced syntax.
		/// </summary>
		/// <param name="enabled">Turn it on or off</param>
		/// <returns></returns>
		public Query EnableAdvancedSyntax(bool enabled)
		{
			advancedSyntax = enabled;
			return this;
		}

		/// <summary>
		/// Allows enabling of rules.
		/// </summary>
		/// <param name="enabled">Turn it on or off</param>
		/// <returns></returns>
		public Query EnableRules(bool enabled)
		{
			enableRules = enabled;
			return this;
		}

		/// <summary>
		/// List of contexts for which rules are enabled.
		/// Contextual rules matching any of these contexts are eligible, as well a s generic rules.
		/// When empty, only generic rules are eligible.
		/// </summary>
		/// <param name="ruleContexts"></param>
		/// <returns></returns>
		public Query SetRuleContexts(IEnumerable<string> ruleContexts)
		{
			this.ruleContexts = ruleContexts;
			return this;
		}

		/// <summary>
		/// Allows enabling of stop words removal.
		/// </summary>
		/// <param name="enabled">Turn it on or/off or providing a list of keywords</param>
		/// <returns></returns>
		public Query EnableRemoveStopWords(IEnabledRemoveStopWords enabled)
		{
			removeStopWords = enabled.GetValue();
			return this;
		}

		/// <summary>
		/// Allows enabling of stop words removal.
		/// </summary>
		/// <param name="enabled">Turn it on or/off or providing a list of keywords</param>
		/// <returns></returns>
		public Query EnableRemoveStopWords(bool enabled)
		{
            return EnableRemoveStopWords(new EnabledRemoveStopWordsBool { Enabled = enabled });
		}

	    /// <summary>
	    /// Limit the search from a referer pattern. Only works on HTTPS
	    /// </summary>
	    /// <param name="referers"></param>
	    /// <returns></returns>
	    public Query SetReferers(string referers)
		{
			this.referers = referers;
			return this;
		}

		/// <summary>
		/// Set the key used for the rate-limit
		/// </summary>
		/// <param name="userToken">Identifier used for the rate-limit</param>
		/// <returns></returns>
		public Query SetUserToken(string userToken)
		{
			this.userToken = userToken;
			return this;
		}

		/// <summary>
		/// Restrict an API key to a list of indices
		/// </summary>
		/// <param name="indices"></param>
		/// <returns></returns>
		public Query SetRestrictIndices(IEnumerable<string> indices)
		{
			restrictIndices = indices;
			return this;
		}
		
		/// <summary>
		/// Specify the query language(s) (using language ISO code) for the
		/// given query. It enables language-specific features that may have an
		/// effect on `removeStopWords` or `ignorePlurals` for instance.
		/// </summary>
		/// <param name="queryLanguages">
		/// List of query languages (using language
		/// ISO code such as "de" or "fi".
		/// </param>
		/// <returns></returns>
		public Query SetQueryLanguages(IEnumerable<string> queryLanguages)
		{
			this.queryLanguages = queryLanguages;
			return this;
		}

		/// <summary>
		/// Restrict an API key to a specific IPv4
		/// </summary>
		/// <param name="sources"></param>
		/// <returns></returns>
		public Query SetRestrictSources(string sources)
		{
			restrictSources = sources;
			return this;
		}

		/// <summary>
		/// Set the object attributes that you want to use for faceting.
		/// </summary>
		/// <param name="facets">List of object attributes that you want to use for faceting. Only attributes that have been added in **attributesForFaceting** index setting can be used in this parameter. You can also use `*` to perform faceting on all attributes specified in **attributesForFaceting**.</param>
		/// <returns></returns>
		public Query SetFacets(IEnumerable<string> facets)
		{
			this.facets = facets;
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
			customParameters.Add(name, value);
			return this;
		}

	    private bool EmptyEnumerable(IEnumerable<string> en) => en.Count() == 0;

		/// <summary>
		/// Get out the query as a string
		/// </summary>
		/// <returns></returns>
		public string GetQueryString()
		{
			string stringBuilder = string.Empty;

			if (attributes != null)
			{
				stringBuilder += "attributesToRetrieve=";
				bool first = true;
				foreach (string attr in attributes)
				{
					if (!first)
						stringBuilder += Constants.Comma;
					stringBuilder += WebUtility.UrlEncode(attr);
					first = false;
				}
				if (EmptyEnumerable(attributes))
					stringBuilder += Constants.EmptyArrayString;
			}
			if (attributesToHighlight != null)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "attributesToHighlight=";
				bool first = true;
				foreach (string attr in attributesToHighlight)
				{
					if (!first)
						stringBuilder += Constants.Comma;
					stringBuilder += WebUtility.UrlEncode(attr);
					first = false;
				}
				if (EmptyEnumerable(attributesToHighlight))
					stringBuilder += Constants.EmptyArrayString;
			}
			if (noTypoToleranceOn != null)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "disableTypoToleranceOnAttributes=";
				bool first = true;
				foreach (string attr in noTypoToleranceOn)
				{
					if (!first)
						stringBuilder += Constants.Comma;
					stringBuilder += WebUtility.UrlEncode(attr);
					first = false;
				}
				if (EmptyEnumerable(noTypoToleranceOn))
					stringBuilder += Constants.EmptyArrayString;
			}

			if (attributesToSnippet != null)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "attributesToSnippet=";
				bool first = true;
				foreach (string attr in attributesToSnippet)
				{
					if (!first)
						stringBuilder += Constants.Comma;
					stringBuilder += WebUtility.UrlEncode(attr);
					first = false;
				}
				if (EmptyEnumerable(attributesToSnippet))
					stringBuilder += Constants.EmptyArrayString;
			}
			if (facets != null)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "facets=";
				bool first = true;
				foreach (string attr in facets)
				{
					if (!first)
						stringBuilder += Constants.Comma;
					stringBuilder += WebUtility.UrlEncode(attr);
					first = false;
				}
			}
			if (!string.IsNullOrEmpty(facetFilters))
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "facetFilters=";
				stringBuilder += WebUtility.UrlEncode(facetFilters);
			}
			if (filters != null)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "filters=";
				stringBuilder += WebUtility.UrlEncode(filters);
			}
			if (maxValuesPerFacets.HasValue)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "maxValuesPerFacet=";
				stringBuilder += Newtonsoft.Json.JsonConvert.SerializeObject(maxValuesPerFacets.Value);
			}
			if (maxFacetHits.HasValue)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "maxFacetHits=";
				stringBuilder += Newtonsoft.Json.JsonConvert.SerializeObject(maxFacetHits.Value);
			}
			if (attributesToSnippet != null)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "attributesToSnippet=";
				bool first = true;
				foreach (string attr in attributesToSnippet)
				{
					if (!first)
						stringBuilder += Constants.Comma;
					stringBuilder += WebUtility.UrlEncode(attr);
					first = false;
				}
			}
			if (responseFields != null)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "responseFields=";
				bool first = true;
				foreach (string attr in responseFields)
				{
					if (!first)
						stringBuilder += Constants.Comma;
					stringBuilder += WebUtility.UrlEncode(attr);
					first = false;
				}
				if (EmptyEnumerable(responseFields))
					stringBuilder += Constants.EmptyArrayString;
			}

			if (!string.IsNullOrEmpty(aroundRadius))
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "aroundRadius=";
				stringBuilder += aroundRadius;

			}
			if (aroundPrecision.HasValue)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "aroundPrecision=";
				stringBuilder += aroundPrecision.Value.ToString();
			}
			if (minWordSizeForApprox1.HasValue)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "minWordSizefor1Typo=";
				stringBuilder += minWordSizeForApprox1.Value.ToString();
			}
			if (minWordSizeForApprox2.HasValue)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "minWordSizefor2Typos=";
				stringBuilder += minWordSizeForApprox2.Value.ToString();
			}
			if (getRankingInfo.HasValue)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "getRankingInfo=";
				stringBuilder += getRankingInfo.Value ? "true" : "false";
			}

			if (!string.IsNullOrEmpty(ignorePlural))
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "ignorePlural=";
				stringBuilder += ignorePlural;
			}

			if (distinct.HasValue)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "distinct=";
				stringBuilder += distinct.Value.ToString();
			}

			if (facetingAfterDistinct.HasValue)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "facetingAfterDistinct=";
				stringBuilder += facetingAfterDistinct.Value.ToString();
			}

			if (analytics.HasValue)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "analytics=";
				stringBuilder += analytics.Value ? "true" : "false";
			}

			if (percentileComputation.HasValue)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "percentileComputation=";
				stringBuilder += percentileComputation.Value ? "true" : "false";
			}

			if (analyticsTags != null)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "analyticsTags=";
				bool first = true;
				foreach (string attr in analyticsTags)
				{
					if (!first)
						stringBuilder += Constants.Comma;
					stringBuilder += WebUtility.UrlEncode(attr);
					first = false;
				}
			}
			if (synonyms.HasValue)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "synonyms=";
				stringBuilder += synonyms.Value ? "true" : "false";
			}
			if (replaceSynonyms.HasValue)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "replaceSynonymsInHighlight=";
				stringBuilder += replaceSynonyms.Value ? "true" : "false";
			}
			if (typoTolerance.HasValue)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "typoTolerance=";
				switch (typoTolerance)
				{
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
			if (allowTyposOnNumericTokens.HasValue)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "allowTyposOnNumericTokens=";
				stringBuilder += allowTyposOnNumericTokens.Value ? "true" : "false";
			}
			if (advancedSyntax.HasValue)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "advancedSyntax=";
				stringBuilder += advancedSyntax.Value ? "1" : "0";
			}
			if (enableRules.HasValue)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "enableRules=";
				stringBuilder += enableRules.Value ? "true" : "false";
			}
			if (ruleContexts != null)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "ruleContexts=";
				bool first = true;
				foreach (string attr in ruleContexts)
				{
					if (!first)
						stringBuilder += Constants.Comma;
					stringBuilder += WebUtility.UrlEncode(attr);
					first = false;
				}
			}
			if (!string.IsNullOrEmpty(removeStopWords))
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "removeStopWords=";
				stringBuilder += removeStopWords;
			}
			if (page.HasValue)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "page=";
				stringBuilder += page.Value.ToString();
			}
			if (hitsPerPage.HasValue)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "hitsPerPage=";
				stringBuilder += hitsPerPage.Value.ToString();
			}
			if (length.HasValue)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "length=";
				stringBuilder += length.Value.ToString();
			}
			if (offset.HasValue)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "offset=";
				stringBuilder += offset.Value.ToString();
			}
			if (validUntil.HasValue)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "validUntil=";
				stringBuilder += validUntil.Value.ToString();
			}
			if (tags != null)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "tagFilters=";
				stringBuilder += WebUtility.UrlEncode(tags);
			}
			if (numerics != null)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "numericFilters=";
				stringBuilder += WebUtility.UrlEncode(numerics);
			}
			if (insideBoundingBox != null)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += insideBoundingBox;
			}
			else if (aroundLatLong != null)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += aroundLatLong;
			}
			else if (insidePolygon != null)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += insidePolygon;
			}
			if (aroundLatLongViaIP.HasValue)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "aroundLatLngViaIP=";
				stringBuilder += aroundLatLongViaIP.Value ? "true" : "false";
			}
			if (minProximity.HasValue)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "minProximity=";
				stringBuilder += minProximity.Value.ToString();
			}
			if (highlightPreTag != null && highlightPostTag != null)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "highlightPreTag=";
				stringBuilder += highlightPreTag;
				stringBuilder += "&highlightPostTag=";
				stringBuilder += highlightPostTag;
			}
			if (snippetEllipsisText != null)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "snippetEllipsisText=";
				stringBuilder += WebUtility.UrlEncode(snippetEllipsisText);
			}
			if (query != null)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "query=";
				stringBuilder += WebUtility.UrlEncode(query);
			}
			if (similarQuery != null)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "similarQuery=";
				stringBuilder += WebUtility.UrlEncode(similarQuery);
			}

			if (optionalWords != null)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "optionalWords=";
				stringBuilder += WebUtility.UrlEncode(optionalWords);
			}

			if (!string.IsNullOrEmpty(exactOnSingleWordQuery))
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "exactOnSingleWordQuery=";
				stringBuilder += exactOnSingleWordQuery;
			}

			if (!string.IsNullOrEmpty(alternativesAsExact))
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "alternativesAsExact=";
				stringBuilder += alternativesAsExact;
			}

			if (restrictSearchableAttributes != null)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "restrictSearchableAttributes=";
				stringBuilder += WebUtility.UrlEncode(restrictSearchableAttributes);
			}
			if (removeWordsIfNoResult.HasValue)
			{
				switch (removeWordsIfNoResult)
				{
					case RemoveWordsIfNoResult.NONE:
						if (stringBuilder.Length > 0)
							stringBuilder += Constants.Ampersand;
						stringBuilder += "removeWordsIfNoResult=None";
						break;
					case RemoveWordsIfNoResult.FIRST_WORDS:
						if (stringBuilder.Length > 0)
							stringBuilder += Constants.Ampersand;
						stringBuilder += "removeWordsIfNoResult=FirstWords";
						break;
					case RemoveWordsIfNoResult.LAST_WORDS:
						if (stringBuilder.Length > 0)
							stringBuilder += Constants.Ampersand;
						stringBuilder += "removeWordsIfNoResult=LastWords";
						break;
					case RemoveWordsIfNoResult.ALL_OPTIONAL:
						if (stringBuilder.Length > 0)
							stringBuilder += Constants.Ampersand;
						stringBuilder += "removeWordsIfNoResult=allOptional";
						break;
				}
			}
			if (queryType.HasValue)
			{
				switch (queryType)
				{
					case QueryType.PREFIX_ALL:
						if (stringBuilder.Length > 0)
							stringBuilder += Constants.Ampersand;
						stringBuilder += "queryType=prefixAll";
						break;
					case QueryType.PREFIX_LAST:
						if (stringBuilder.Length > 0)
							stringBuilder += Constants.Ampersand;
						stringBuilder += "queryType=prefixLast";
						break;
					case QueryType.PREFIX_NONE:
						if (stringBuilder.Length > 0)
							stringBuilder += Constants.Ampersand;
						stringBuilder += "queryType=prefixNone";
						break;
				}
			}
			if (sortFacetValuesBy.HasValue)
			{
				switch (sortFacetValuesBy)
				{
					case SortFacetValuesBy.COUNT:
						if (stringBuilder.Length > 0)
							stringBuilder += Constants.Ampersand;
						stringBuilder += "sortFacetValuesBy=count";
						break;
					case SortFacetValuesBy.ALPHA:
						if (stringBuilder.Length > 0)
							stringBuilder += Constants.Ampersand;
						stringBuilder += "sortFacetValuesBy=alpha";
						break;
				}
			}
			if (userToken != null)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "userToken=";
				stringBuilder += WebUtility.UrlEncode(userToken);
			}
			if (restrictIndices != null)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "restrictIndices=";
				bool first = true;
				foreach (string attr in restrictIndices)
				{
					if (!first)
						stringBuilder += Constants.Comma;
					stringBuilder += WebUtility.UrlEncode(attr);
					first = false;
				}
			}
			if (queryLanguages != null)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += '&';
				stringBuilder += "queryLanguages=";
				bool first = true;
				foreach (string lang in this.queryLanguages)
				{
					if (!first)
						stringBuilder += ',';
					stringBuilder += WebUtility.UrlEncode(lang);
					first = false;
				}
			}
			if (restrictSources != null)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "restrictSources=";
				stringBuilder += WebUtility.UrlEncode(restrictSources);
			}
			if (referers != null)
			{
				if (stringBuilder.Length > 0)
					stringBuilder += Constants.Ampersand;
				stringBuilder += "referer=";
				stringBuilder += WebUtility.UrlEncode(referers);
			}
			if (customParameters.Count > 0)
			{
				foreach (KeyValuePair<string, string> elt in customParameters)
				{
					if (stringBuilder.Length > 0)
						stringBuilder += Constants.Ampersand;
					stringBuilder += elt.Key;
					stringBuilder += "=";
					stringBuilder += WebUtility.UrlEncode(elt.Value);
				}
			}
			return stringBuilder;
		}

		private IDictionary<string, string> customParameters;
		private IEnumerable<string> attributes;
		private IEnumerable<string> attributesToHighlight;
		private IEnumerable<string> noTypoToleranceOn;
		private IEnumerable<string> attributesToSnippet;
		private IEnumerable<string> responseFields;
		private IEnumerable<string> analyticsTags;
		private string exactOnSingleWordQuery;
		private string alternativesAsExact;
		private int? minWordSizeForApprox1;
		private int? aroundPrecision;
		private string aroundRadius;
		private int? minWordSizeForApprox2;
		private bool? getRankingInfo;
		private string ignorePlural;
		private int? distinct;
		private int? facetingAfterDistinct;
		private bool? advancedSyntax;
		private string removeStopWords;
		private bool? analytics;
		private bool? synonyms;
		private bool? replaceSynonyms;
		private TypoTolerance? typoTolerance;
		private bool? allowTyposOnNumericTokens;
		private bool? percentileComputation;
		private int? page;
		private int? hitsPerPage;
		private int? offset;
		private int? validUntil;
		private int? length;
		private string filters;
		private string tags;
		private string numerics;
		private string insideBoundingBox;
		private string insidePolygon;
		private string aroundLatLong;
		private bool? aroundLatLongViaIP;
		private string query;
		private string similarQuery;
		private string highlightPreTag;
		private string highlightPostTag;
		private string snippetEllipsisText;
		private int? minProximity;
		private string optionalWords;
		private QueryType? queryType;
		private SortFacetValuesBy? sortFacetValuesBy;
		private RemoveWordsIfNoResult? removeWordsIfNoResult;
		private IEnumerable<string> facets;
		private string facetFilters;
		private int? maxValuesPerFacets;
		private int? maxFacetHits;
		private string restrictSearchableAttributes;
		private string userToken;
		private IEnumerable<string> restrictIndices;
		private string restrictSources;
		private string referers;
		private bool? enableRules;
		private IEnumerable<string> ruleContexts;
		private IEnumerable<string> queryLanguages;
	}
}
