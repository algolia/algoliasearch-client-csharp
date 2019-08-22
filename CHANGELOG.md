# ChangeLog

## [6.3.0](https://github.com/algolia/algoliasearch-client-csharp/compare/6.2.0...6.3.0) (2019-08-22)

### Fix

- JsonReader for legacy filters format ([a2a987c](https://github.com/algolia/algoliasearch-client-csharp/commit/a2a987c))
- Missing catch clause in httpRequester ([626d19d](https://github.com/algolia/algoliasearch-client-csharp/commit/626d19d))
    The requester was not catching correctly all exceptions, making the
    retry strategy to fail instead of retrying for network issues such as DNS resolution.

### Feat

- Index.FindFirstObject() method ([4989f48](https://github.com/algolia/algoliasearch-client-csharp/commit/4989f48))
- GetObjectIDPosition() method ([9aacbf2](https://github.com/algolia/algoliasearch-client-csharp/commit/9aacbf2))
- GetSecuredApiKeyRemainingValidity ([aa5d5f0](https://github.com/algolia/algoliasearch-client-csharp/commit/aa5d5f0))
    New SearchClient method to get the remaining validity (seconds)
    of a securedAPIKey

- Alternatives in QueryRule Conditions ([5d34c97](https://github.com/algolia/algoliasearch-client-csharp/commit/5d34c97))
- indexLanguages settings properties ([adb7ba3](https://github.com/algolia/algoliasearch-client-csharp/commit/adb7ba3))
- IndexExists() method ([951ea87](https://github.com/algolia/algoliasearch-client-csharp/commit/951ea87))

6.2.0 (2019-06-25)

New Features
------------
Added: UserToken property in Query
Added: HighlightResult class for SearchResponse
Added: SnippetResult class for SearchResponse
Added: personalisation impact in Query
Added: CustomSettings Property in Settings. This property allows to pass and retrieve advanced parameters to Algolia.
Added: CustomParameters Property in Query. This property allows to pass advanced parameters to Algolia.
Added: AdvancedSyntaxFeatures in Query.
Added: SimilarQuery in Query.
Added: AttributeCriteriaComputedByMinProximity in IndexSettings.
Added: Primary parameter in IndexSettings.
Added: UserData in IndexSettings.

Bug Fix
---------

(Soft BC)
Fixed: TagFilters Property in Query. TagFilters is now List<List<string>> instead of string

(soft BC)
Updated: AroundPrecision Query Parameter. The query parameter AroundPrecision can now handle a JSON format:
{
"aroundPrecision":
[ {"from":0, "value":10}, {"from":100, "value":1000}, ... ]
}

(soft BC)
Removed: RestrictSources parameter from APIKey. RestrictSources should not be a param of APIKey but a search parameter of Query.

Fixed: Use long instead of Int in IndicesResponse. Causing overflow error.
Renamed ABtestReponse to ABTestResponse. (BC)

Fixed: The ToQueryString() method was not writing nested arrays as expected
causing MultipleQueries with nested arrays to return wrong responses.

Misc & Internals
-----------------
Updated NewtonSoft.Json to 12.0.2.
Remove unused IHttpTransport interface.
Removed unused IRetryStrategy interface.
Updated test dependencies.
Refactoring conveter's code.

6.1.3 (2019-04-03)

- Added: ToQueryString() in Query class - Also added support for list and nested list in QueryStringHelper.
- Fixed: removed Wait() method for MCM. Methods were unreliable on the engine side, so we decided to remove them.
  You can still find the implementation of the Wait() methods in the MCM tests.
- Fixed: removed unsued priority property in StatefulHost

6.1.2 (2019-03-14)

- Fixed: Package icon URL
- Fixed: Missing encoding of URL parameters

6.1.1 (2019-02-26)

- Fixed wrong JsonPropertyName for facets_stats

6.1.0 (2019-02-26)

- Added: SearchClient.RestoreAPIKey method
- Added: Support for AA Testing (AB testing on the same index with custom parameters)
- Fixed properties type in the ABTest/Variant class
- Fixed wrong JsonProperty name of SearchForFacetRequest
- Fixed wrong calltype for Insights/SendEvents

6.0.3 (2019-02-13)

- Added: Missing parameters in the SearchResponse object: Facets, FacetsStats, UserData, AppliedRules, Length, Offset
- Fixed: AutomaticRadius type in SearchResponse float => string

6.0.1 (2019-02-11)

- Fixed: DeleteObject not working properly because of wrong URI
- Fixed: SaveObject not working properly with JsonPropertyAttribute
- Fixed: SaveObject not working properly with POCO that have ObjectID property in base class

6.0.0 (2019-02-07)

Big major release for the .NET API Client.
There are breaking changes on signatures because every requests/responses are not typed.

* .NET related:
- Total rewrite of the code base
- Implementing .NET Standard 1.3 and .NET Standard 2.0
- Backward compatible until .NET 4.5 framework
- Project building cross platform

* Internals related:
- Async/sync methods with Cancellation token for async
- Thread safe clients
- Injectable HTTPClient
- Types for every request/response
- Documented types and requests
- Isolated  and testable Retry Strategy
- Payload logger in debug mode
- Implemented the Algolia’s API Client Test suite
- Parallel test suite

* Algolia related:
- AutoSplit objects in Save methods
- SaveRules, SaveSynonyms, SaveObjects now accept iterators
- New class: AccountClient
- New Method: AccountClient.CopyIndex allow you to copy and index from an application to another
- New method: ReplaceAllRules
- New method: ReplaceAllSynonyms
- New method: ReplaceAllObjects
- New method: CopySettings
- New method: CopySynonyms
- New method: CopyRules

5.3.1 (2018-12-18)
- Added enablePersonalization on SearchQuery

5.3.0 (2018-12-17)
- Added Insights feature
- Added Personalization feature

5.2.1 (2018-12-03)
- Added HTTP code in AlgoliaException
- Fixed browse post URL
- Fixed .csproj for OSX dev env

5.2.0 (2018-10-10)
- Added Query Rules v2
- Added queryLanguages as a search parameter
- Added clickAnalytics as a search parameter

5.1.0 (2018-6-25)
- Add ABTest APis. Found in the `Analytics` class.

5.0.0 (2018-4-23)
- Add Multi Cluster Management methods
- Add support for .NET 4.7 and .NET 4.7.1
- Update some API methods to most recent signature: `SaveSynonym` and `AddObject`
- Remove deprecated methods:`DeleteByQuery`, `ListUserKeys`, `GetUserKeyACL`, `GetAPIKeyACL`, `DeleteUserKey`, `AddUserKey`, `UpdateUserKey` and `ApiKey` methods of the `Index` class.

4.2.2 (2018-1-31)
- Deprecate GetApiKeyACL and rename to GetApiKey
- Add validUntil parameter
- Add RequestOptions to BrowseAll method

4.2.1 (2017-12-7)
- Add optional `scopes` parameter to `CopyIndex` method to scope the copy to settings/rules/synonyms.
- Add support for .Net 4.5 and 4.6.1.

4.2.0 (2017-10-23)
- Add new `deleteBy` method that hits the deleteBy endpoints.
- Add `sortFacetValuesBy` query parameter.
- Add `RulesIterator` and `SynonymsIterator`.

4.1.2 (2017-10-02)
- Add support for .NET core 2 and .NET standard 2.

4.1.1 (2017-09-13)
- Adding overload methods everywhere to ensure binary compatibility with previous versions of the client. This is due to adding the RequestOptions optional parameter.

4.1.0 (2017-09-13)
- Support for Rules.
- Support for per-request options parameters.
**Warning**: This version will be source-code compatible with the previous one but not binary compatible. In the next version, we will fix that by adding overload methods.

4.0 (2017-05-24)
- Support to .NET Core 1.0 and 1.1, .Net Framework 4.6 and 4.6.2, .NETStandard 1.3 and 1.6
- Dropping support to .NET Framework 4.0, 4.5 and PCL

3.7.5 (2017-04-24)
- Support to percentileComputation

3.7.4 (2017-03-21)
- Replace UserKey by ApiKey

3.7.3 (2017-02-07)
- Support for facetingAfterDistinct

3.7.2 (2017-01-30)
- Support for responseFields

3.7.1 (2017-01-03)
- Rename attributesToIndex to searchableAttributes
= Rename forwardToSlaves to forwardToReplicas

3.7.0 (2016-12-12)
- Fix Naming SearchForFacetValues
- Implement new DSN retry strategy

3.6.10 (2016-11-17)
- Fix issue on restrictIndices

3.6.9 (2016-10-28)
- Adding support to comma separated list of language for ignorePlural

3.6.8 (2016-10-20)
- Changing Project release configuration
- Adding support to restrictSources and restrictIndices
- Adding support to SearchFacet

3.6.7 (2016-10-19)
- Fixing project configuration

3.6.6 (2016-09-01)
- Support to attributeToRetrieve in GetObjects

3.6.5 (2016-07-07)
- Support to length and offset
- Fix strategy multiple query

3.6.4 (2016-06-23)
- Support to removeStopWords and radius
- Support to exactOnSingleWordQuery and alternativesAsExact
- Support to forwardToSlaves in SetSettings

3.6.3 (2016-05-02)
- New synonyms endpoints

3.6.2 (2016-05-19)
- Shuffling array of hosts
- InvariantCulture on Float string conversion

3.6.1 (2016-04-14)
- Fix the retry strategy in case of timeout

3.6.0 (2016-04-12)
- Added way to specify custom query parameter for a query

3.5.0 (2016-03-25)
- Added analyticsTags query parameter

3.4.4 (2016-02-10)
- Allow to set a timeout lower than 1s

3.4.3 (2016-02-05)
- Added new SetSnippetEllipsisText query parameter to configure snippet ellipsis text

3.4.2 (2015-11-11)
- Fixed a bug in the browse method when the cursor contains a '+'

3.4.0 (2015-10-16)
- Added new secured api key
- Added cancellation token

3.3.3 (2015-10-13)
- Fixed a typo in EnableRemoveStopWords method

3.3.2 (2015-10-12)
------------------
- Added remove stop words query parameter
- Added support of similar queries

3.3.1 (2015-10-09)
------------------

- Added support of multiple bounding box for geo-search
- Added support of polygon for geo-search
- Added support of automatic radius computation for geo-search
- Added support of disableTypoToleranceOnAttributes
- Fix generation of secured api key
- Added support of unified filters


3.3.0 (2015-09-08)
------------------

- Add new parameter in the batched partial update method to avoid the automatic creation of the object.

3.2.0 (2015-09-08)
------------------

- Add new parameter in the partial update method to avoid the automatic creation of the object.

3.1.7 (2015-08-31)
------------------
- Fixed bug with the DeleteByQuery, it now uses the same HTTP layer for the query and the waitTask

3.1.6 (2015-08-30)
------------------
- Fixed a bug in DeleteByQuery and distinct>1

3.1.5 (2015-08-28)
------------------
- Fixed a bug with query parameters and the default query settings override.

3.1.4 (2015-06-30)
------------------
- Added support of grouping (distinct=3 for example keep the 3 best hits for a distinct key)
- Added new browse methods

3.1.3 (2015-06-04)
------------------
- Fixed a bug in SetHighlightingTags support

3.1.2 (2015-06-03)
------------------
- Added SetMinProximity support on query
- Added SetHighlightingTags support on query

3.1.1 (2015-05-04)
------------------
- Fix retry logic to handle new error codes
- Add new methods to add/update api key
- Add batch method to target multiple indices
- Add strategy parameter for the multipleQueries
- Add new method to generate secured api key from query parameters
- Update dependencies


3.1.0 (2015-04-10)
------------------
- Added new retry logic

3.0.5 (2015-02-18)
------------------
- Added support of allOptional in removeWordsIfNoResult

3.0.4 (2015-01-21)
------------------
- Added a IndexHelper class to ease add/deletion/reindexing of generic objects

3.0.3 (2015-01-09)
------------------
- Switched to base HttpMessageHandler for mocking (for Windows Phone 8.1)

3.0.2 (2015-01-04)
------------------
- Added Windows Phone 8.1 support

3.0.1 (2015-01-03)
------------------
- Fix multi-arch issue (version 3.0.0 was not installable on most architecture)
- Added read timeout of 30s and put a host and the end of the list in case of error

3.0.0 (2015-01-02)
------------------

- Add the index names restriction parameter for the add and update ACL methods
- Removed separate .NET 4.0 Project
- Added GenerateSecuredApiKey to Portable Class Library
- Now supports .NET 4.0, .NET 4.5, ASP.NET vNext 1.0, Mono 4.5, Windows 8, Windows 8.1, Windows Phone 8.1, Xamarin iOS, and Xamarin Android
- Updated code comments
- Switch from Travis to Appveyor for CI

2.4.1 (2014-12-04)
------------------

- Add SearchDisjunctiveFacetingAsync. [Xavier Grand]

2.4.0 (2014-11-30)
------------------

- Bump to 2.4.0. [Xavier Grand]

- Increase sleep. [Xavier Grand]

- Switch to .net. [Xavier Grand]

- Fix multiple travis builds in parallel. [Xavier Grand]

2.3.3 (2014-11-25)
------------------

- Bump to 2.3.3. [Xavier Grand]

- Fix Query.cs. [Xavier Grand]

2.3.2 (2014-11-24)
------------------

- Bump to 2.3.2. [Xavier Grand]

- Add Disjunctive Faceting. [Xavier Grand]

2.3.1 (2014-10-22)
------------------

- Bump to 2.3.1. [Xavier Grand]

- Improve retrying. [Xavier Grand]

2.3.0 (2014-10-14)
------------------

- Add setExtraHeader and set the User-Agent. [Xavier Grand]

2.2.0 (2014-09-14)
------------------

- Version 2.2. [Julien Lemoine]

- Add the documentation about the update of an APIKey. [Xavier Grand]

- Added update key. [Xavier Grand]

- Updated default typoTolerance setting & updated removedWordsIfNoResult
  documentation. [Julien Lemoine]

- Add a note about the ConfigureAwait. [Xavier Grand]

- Allow to configure the await. [Xavier Grand]

- Update the assembly version to 2.0.0. [Xavier Grand]

- Add missing reference. [Xavier Grand]

- Update target of Algolia.Search.Test. [Xavier Grand]

- Merge branch 'master' of https://github.com/algolia/algoliasearch-
  client-csharp. [Xavier Grand]

- Remove useless reference. [Xavier Grand]

- Trying to fix build on travis v4. [Xavier Grand]

- Trying to fix build on travis v3. [Xavier Grand]

- Add the new project in the configuration file of travis. [Xavier
  Grand]

- Trying to fix build on travis v2. [Xavier Grand]

- Trying to fix build on travis. [Xavier Grand]

- Resolve #5: Add a wrapper for the method GenerateSecuredApiKey.
  [Xavier Grand]

- Change the unit testing framework. [Xavier Grand]

- Add documentation about removeWordsIfNoResult. [Xavier Grand]

- Update snippet. [Xavier Grand]

2.0.0 (2014-08-13)
------------------

- Added support of removeWordsIfNoResults. [Xavier Grand]

- Merge branch 'master' of https://github.com/algolia/algoliasearch-
  client-csharp. [Xavier Grand]

- Resolve #7: Add ability to mock AlgoliaClient. [Xavier Grand]

- Resolve #6: Rename method names following TAP name convention. [Xavier
  Grand]

- Throw an exception in the not implemented method GenerateSecuredApiKey
  #5. [Xavier Grand]

- Update dependencies. [Xavier Grand]

- Added aroundLatLngViaIP documentation. [Julien Lemoine]

1.16.0 (2014-08-04)
-------------------

- Added aroundLatitudeLongitudeViaIP. [Julien Lemoine]

- Add notes about the JS API client. [Sylvain UTARD]

- Add tutorial links + minor enhancements. [Sylvain UTARD]

- Added documentation of suffix/prefix index name matching in API key.
  [Julien Lemoine]

- Change the cluster. [Xavier Grand]

- Fixed doc. [Julien Lemoine]

1.15.0 (2014-07-24)
-------------------

- Added RestrictSearchableAttribute doc. [Julien Lemoine]

- Added RestrictSearchableAttributes : [Julien Lemoine]

1.14.0 (2014-07-24)
-------------------

- Added SetSecurityTags & setUserToken methods. [Julien Lemoine]

- Merge branch 'master' of https://github.com/algolia/algoliasearch-
  client-csharp. [Xavier Grand]

- Fix analytics, synonyms and replace synonums. [Xavier Grand]

- Fix travis. [Xavier Grand]

- Trying to fix travis. [Xavier Grand]

- Add DeleteByQuery and GetObjects. [Xavier Grand]

- Add missing waitTask. [Xavier Grand]

- Fix test-suite. [Xavier Grand]

- Added disableTypoToleranceOn & altCorrections index settings. [Julien
  Lemoine]

1.12.0 (2014-06-10)
-------------------

- Add a new facet filters helper. [Xavier Grand]

- Fix retry. [Xavier Grand]

- Add test for facetting and some minor fixes. [Xavier Grand]

- Merge pull request #4 from james-andrewsmith/FacetFilter-Fixes.
  [Xavier Grand]

  Facet Filter Fixes

- Add tests for typo tolerance parametter. [Xavier Grand]

- Fix ACL tests. [Xavier Grand]

- Fix retry for an host unreachable. [Xavier Grand]

- Fix query with huge parametters. [Xavier Grand]

- Add typoTolerance & allowsTyposOnNumericTokens query parameters.
  [Xavier Grand]

- Add typoTolerance & allowsTyposOnNumericTokens query parameters.
  [Sylvain UTARD]

- Documentation: Added words ranking parameter. [Julien Lemoine]

- Added asc(attributeName) & desc(attributeName) documentation in index
  settings. [Julien Lemoine]

- Updated synonyms examples. [Xavier Grand]

- Fix typo. [Xavier Grand]

- Add a note about distinct and the empty queries. [Xavier Grand]

1.11.0 (2014-05-02)
-------------------

- Added EnableSynonyms, EnableAnalytics &
  EnableReplaceSynonymsInHighlight. [Julien Lemoine]

- Added analytics,synonyms,enableSynonymsInHighlight query parameters.
  [Julien Lemoine]

- Test if the objectID exists. [Xavier Grand]

- Add badge of travis. [Xavier Grand]

- Trying to fix travis (simplify tests) [Xavier Grand]

- New numericFilters documentation. [Julien Lemoine]

- Improve handling f http errors. [Xavier Grand]

- Fix test ACL. [Xavier Grand]

- Add advanced syntax. [Xavier Grand]

1.10.0 (2014-03-27)
-------------------

- Update README.md. [Xavier Grand]

- Add test on MultipleQueries. [Xavier Grand]

- Add MultipleQueries. [Xavier Grand]

- Added DeleteObjects. [Xavier Grand]

- Trying to add Travis CI. [Xavier Grand]

- Uncomment the deletion of the index. [Xavier Grand]

- Switched to NUnit. [Xavier Grand]

- Updated Nuget.exe. [Xavier Grand]

- Trying to add Travis CI. [Xavier Grand]

- Added a new feature : maxValuesPerFacet. [Xavier Grand]

- Updated dependencies. [Xavier Grand]

- Improved test suite v2. [Xavier Grand]

- Improved test suite. [Xavier Grand]

- Updated README. [Sylvain UTARD]

- Fixed typo (output of doc generator) Added documentation of slaves.
  [Julien Lemoine]

1.7.0 (2014-01-02)
------------------

- Added support of distinct feature. [Julien Lemoine]

- Fixed copy/paste error. [Julien Lemoine]

- Improved readability of search & settings parameters. [Julien Lemoine]

1.6.0 (2013-12-06)
------------------

- Released version 1.6.0 Added Browse() method Added
  partialUpdateObjects() method. [Julien Lemoine]

- Fixed typo. [Julien Lemoine]

1.5.0 (2013-11-08)
------------------

- Version 1.5.0. [Julien Lemoine]

1.4.0 (2013-11-08)
------------------

- Documented new features. Version 1.4.0. [Julien Lemoine]

- Improve move & copy doc. [Nicolas Dessaigne]

- New method that expose geoPrecision in aroundLatLng. [Julien Lemoine]

- Renamed parameter to configure typo tolerance. [Julien Lemoine]

- QueryType=prefixLast by default documented unordered(attributeName)
  [Julien Lemoine]

- Moved numerics close to tags in doc other minor corrections. [Nicolas
  Dessaigne]

- Updated attributesToIndes doc. [Julien Lemoine]

- Fixed numeric doc. [Julien Lemoine]

- Fixed language. [Julien Lemoine]

- Added missing await keywords. [Julien Lemoine]

- Documented new features: move/copy/numerics/logs. [Julien Lemoine]

- Added support of new features: move/copy/numerics/logs. [Julien
  Lemoine]

- Added a check to avoid empty objectID. [Julien Lemoine]

- Added "your account" link. [Julien Lemoine]

- Correction. [Nicolas Dessaigne]

- Replaced cities par contacts. [Julien Lemoine]

- New constructor that automatically set hostnames. [Julien Lemoine]

- Updated ranking doc. [Julien Lemoine]

- Updated Ranking doc. [Julien Lemoine]

- Fixed typo. [Julien Lemoine]

- Added right arrow character. [Julien Lemoine]

- Added Nuget package and updated documentation. [Julien Lemoine]

- Initial import of Algolia search c# client based on Christopher Maneu
  client. [Julien Lemoine]
