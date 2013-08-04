Algolia Search API Client for C#
==================

This C# client let you easily use the Algolia Search API from your App (Compatible with .NET 4.5, SL4+, WP7.5+, Windows Store).
The service is currently in Beta, you can request an invite on our [website](http://www.algolia.com/pricing/).

Table of Content
-------------
**Get started**

1. [Setup](#setup) 
1. [Quick Start](#quick-start)

**Commands reference**

1. [Search](#search)
1. [Add a new object](#add-a-new-object-in-the-index)
1. [Update an object](#update-an-existing-object-in-the-index)
1. [Get an object](#get-an-object)
1. [Delete an object](#delete-an-object)
1. [Index settings](#index-settings)
1. [List indexes](#list-indexes)
1. [Delete an index](#delete-an-index)
1. [Wait indexing](#wait-indexing)
1. [Batch writes](#batch-writes)
1. [Security / User API Keys](#security--user-api-keys)

Setup
-------------
To setup your project, follow these steps:

 1. In you project, open the "Package Manager Console" (Tools → Library Package Manager → Package Manager Console)
 2. Enter `Install-Package Algolia.Search` in the Package Manager Console
 2. Initialize the client with your ApplicationID and API-Key (you can find all of them on your Algolia account)

```csharp
using Algolia.Search;

AlgoliaClient client = new AlgoliaClient("YourApplicationID", "YourAPIKey");
```


Quick Start
-------------
This quick start is a 30 seconds tutorial where you can discover how to index and search objects.

Without any prior-configuration, you can index the 1000 world's biggest cities in the ```cities``` index with the following code:
```csharp
// Load JSON file
StreamReader re = File.OpenText("1000-cities.json");
JsonTextReader reader = new JsonTextReader(re);
JObject jsonObject = JObject.Load(reader);
// Add objects 
Index index = client.InitIndex("cities");
await index.AddObjects((JArray)jsonObject["objects"]);
```
The [1000-cities.json](https://github.com/algolia/algoliasearch-client-csharp/blob/master/1000-cities.json) file contains city names extracted from [Geonames](http://www.geonames.org).

You can then start to search for a city name (even with typos):
```csharp
JObject res = await index.Search(new Query("san fran"));
System.Diagnostics.Debug.WriteLine(res["hits"]);
res = await index.Search(new Query("loz qngel"));
System.Diagnostics.Debug.WriteLine(res["hits"]);
```

Settings can be customized to tune the index behavior. For example you can add a custom sort by population to the already good out-of-the-box relevance to raise bigger cities above smaller ones. To update the settings, use the following code:
```csharp
await index.SetSettings(JObject.Parse(@"{""customRanking"":[""desc(population)"", ""asc(name)""]}"));
```

And then search for all cities that start with an "s":
```csharp
var res = await index.Search(new Query("s"));
System.Diagnostics.Debug.WriteLine(res["hits"]);
```

Search
-------------
 **Opening note:** If you are building a web application, you may be more interested in using our [javascript client](https://github.com/algolia/algoliasearch-client-js) to send queries. It brings two benefits: (i) your users get a better response time by avoiding to go threw your servers, and (ii) it will offload your servers of unnecessary tasks.

To perform a search, you just need to initialize the index and perform a call to the search function.<br/>
You can use the following optional arguments on Query class:

 * **SetAttributesToRetrieve**: specify the list of attribute names to retrieve.<br/>By default all attributes are retrieved.
 * **SetAttributesToHighlight**: specify the list of attribute names to highlight.<br/>By default indexed attributes are highlighted. Numerical attributes cannot be highlighted. A **matchLevel** is returned for each highlighted attribute and can contain: "full" if all the query terms were found in the attribute, "partial" if only some of the query terms were found, or "none" if none of the query terms were found.
 * **SetAttributesToSnippet**: specify the list of attributes to snippet alongside the number of words to return (syntax is 'attributeName:nbWords'). <br/>By default no snippet is computed.
 * **SetMinWordSizeToAllowOneTypo**: the minimum number of characters in a query word to accept one typo in this word.<br/>Defaults to 3.
 * **SetMinWordSizeToAllowTwoTypos**: the minimum number of characters in a query word to accept two typos in this word.<br/>Defaults to 7.
 * **GetRankingInfo**: if set, the result hits will contain ranking information in _rankingInfo attribute.
 * **SetPage**: *(pagination parameter)* page to retrieve (zero base).<br/>Defaults to 0.
 * **SetNbHitsPerPage**: *(pagination parameter)* number of hits per page.<br/>Defaults to 10.
 * **AroundLatitudeLongitude**: search for entries around a given latitude/longitude.<br/>You specify the maximum distance in meters with the **radius** parameter (in meters).<br/>At indexing, you should specify geoloc of an object with the _geoloc attribute (in the form `{"_geoloc":{"lat":48.853409, "lng":2.348800}}`)
 * **InsideBoundingBox**: search entries inside a given area defined by the two extreme points of a rectangle.<br/>At indexing, you should specify geoloc of an object with the _geoloc attribute (in the form `{"_geoloc":{"lat":48.853409, "lng":2.348800}}`)
 * **SetQueryType**: select how the query words are interpreted:
  * **PREFIX_ALL**: all query words are interpreted as prefixes (default behavior).
  * **PREFIX_LAST**: only the last word is interpreted as a prefix. This option is recommended if you have a lot of content to speedup the processing.
  * **PREFIX_NONE**: no query word is interpreted as a prefix. This option is not recommended.
 * **SetTags**: filter the query by a set of tags. You can AND tags by separating them by commas. To OR tags, you must add parentheses. For example, `tag1,(tag2,tag3)` means *tag1 AND (tag2 OR tag3)*.<br/>At indexing, tags should be added in the _tags attribute of objects (for example `{"_tags":["tag1","tag2"]}` )


```csharp
Index index = client.InitIndex("MyIndexName");
res = await index.Search(new Query("query string"));
System.Diagnostics.Debug.WriteLine(res);
res = await index.Search(new Query("query string").
    SetAttributesToRetrieve(new string[] {"population"}).
    SetNbHitsPerPage(50));
System.Diagnostics.Debug.WriteLine(res);
```

The server response will look like:

```javascript
{
    "hits":[
            { "name": "Betty Jane Mccamey",
              "company": "Vita Foods Inc.",
              "email": "betty@mccamey.com",
              "objectID": "6891Y2usk0",
              "_highlightResult": {"name": {"value": "Betty <em>Jan</em>e Mccamey", "matchLevel": "full"}, 
                                   "company": {"value": "Vita Foods Inc.", "matchLevel": "none"},
                                   "email": {"value": "betty@mccamey.com", "matchLevel": "none"} }
            },
            { "name": "Gayla Geimer Dan", 
              "company": "Ortman Mccain Co", 
              "email": "gayla@geimer.com", 
              "objectID": "ap78784310" 
              "_highlightResult": {"name": {"value": "Gayla Geimer <em>Dan</em>", "matchLevel": "full" },
                                   "company": {"value": "Ortman Mccain Co", "matchLevel": "none" },
                                   "email": {"highlighted": "gayla@geimer.com", "matchLevel": "none" } }
            }],
    "page":0,
    "nbHits":2,
    "nbPages":1,
    "hitsPerPage:":20,
    "processingTimeMS":1,
    "query":"jan"
}
```

Add a new object in the Index
-------------

Each entry in an index has a unique identifier called `objectID`. You have two ways to add en entry in the index:

 1. Using automatic `objectID` assignement, you will be able to retrieve it in the answer.
 2. Passing your own `objectID`

You don't need to explicitely create an index, it will be automatically created the first time you add an object.
Objects are schema less, you don't need any configuration to start indexing. The settings section provide details about advanced settings.

Example with automatic `objectID` assignement:

```csharp
var res = await index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", 
                                                 ""population"":805235}"));
System.Diagnostics.Debug.WriteLine("objectID=" + res["objectID"]);           
```

Example with manual `objectID` assignement:

```charp
var res = await index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", 
                                                 ""population"":805235}"), "myID");
System.Diagnostics.Debug.WriteLine("objectID=" + res["objectID"]);
```

Update an existing object in the Index
-------------

You have two options to update an existing object:

 1. Replace all its attributes.
 2. Replace only some attributes.

Example to replace all the content of an existing object:

```csharp
await index.SaveObject(JObject.Parse(@"{""name"":""Los Angeles"", 
                                        ""population"":3792621, 
                                        ""objectID"":""myID""}"));
```

Example to update only the population attribute of an existing object:

```csharp
await index.PartialUpdateObject(JObject.Parse(@"{""population"":3792621, 
                                                 ""objectID"":""myID""}"));
```

Get an object
-------------

You can easily retrieve an object using its `objectID` and optionnaly a list of attributes you want to retrieve (using comma as separator):

```csharp
// Retrieves all attributes
var res = await index.GetObject("myID");
System.Diagnostics.Debug.WriteLine(res);
// Retrieves name and population attributes
res = await index.GetObject("myID", new String[] {"name", "population"});
System.Diagnostics.Debug.WriteLine(res);
// Retrieves only the name attribute
res = await index.GetObject("myID", new String[] { "name" });
System.Diagnostics.Debug.WriteLine(res);
```

Delete an object
-------------

You can delete an object using its `objectID`:

```csharp
await index.DeleteObject("myID");
```

Index Settings
-------------

You can retrieve all settings using the `GetSettings` function. The result will contains the following attributes:

 * **minWordSizeForApprox1**: (integer) the minimum number of characters to accept one typo (default = 3).
 * **minWordSizeForApprox2**: (integer) the minimum number of characters to accept two typos (default = 7).
 * **hitsPerPage**: (integer) the number of hits per page (default = 10).
 * **attributesToRetrieve**: (array of strings) default list of attributes to retrieve in objects.
 * **attributesToHighlight**: (array of strings) default list of attributes to highlight.
 * **attributesToSnippet**: (array of strings) default list of attributes to snippet alongside the number of words to return (syntax is 'attributeName:nbWords')<br/>By default no snippet is computed.
 * **attributesToIndex**: (array of strings) the list of fields you want to index.<br/>By default all textual attributes of your objects are indexed, but you should update it to get optimal results.<br/>This parameter has two important uses:
  * *Limits the attributes to index*.<br/>For example if you store a binary image in base64, you want to store it and be able to retrieve it but you don't want to search in the base64 string.
  * *Controls part of the ranking*.<br/>Matches in attributes at the beginning of the list will be considered more important than matches in attributes further down the list. 
 * **ranking**: (array of strings) controls the way hits are sorted.<br/>We have five available criteria:
  * **typo**: sort according to number of typos,
  * **geo**: sort according to decreasing distance when performing a geo-location based search,
  * **proximity**: sort according to the proximity of query words in hits, 
  * **attribute**: sort according to the order of attributes defined by **attributesToIndex**,
  * **exact**: sort according to the number of words that are matched identical to query word (and not as a prefix),
  * **custom**: sort according to a user defined formula set in **customRanking** attribute.
  <br/>The default order is `["typo", "geo", "proximity", "attribute", "exact", "custom"]`. We strongly recommend to keep this configuration.
 * **customRanking**: (array of strings) lets you specify part of the ranking.<br/>The syntax of this condition is an array of strings containing attributes prefixed by asc (ascending order) or desc (descending order) operator.
 For example `"customRanking" => ["desc(population)", "asc(name)"]`
 * **queryType**: select how the query words are interpreted:
  * **prefixAll**: all query words are interpreted as prefixes (default behavior).
  * **prefixLast**: only the last word is interpreted as a prefix. This option is recommended if you have a lot of content to speedup the processing.
  * **prefixNone**: no query word is interpreted as a prefix. This option is not recommended.

You can easily retrieve settings or update them:

```csharp
var res = await index.GetSettings();
System.Diagnostics.Debug.WriteLine(res);
```

```csharp
await index.SetSettings(JObject.Parse(@"{""customRanking"":[""desc(population)"", ""asc(name)""]}"));
```

List indexes
-------------
You can list all your indexes with their associated information (number of entries, disk size, etc.) with the `listIndexes` method:

```csharp
var result = await client.ListIndexes();
System.Diagnostics.Debug.WriteLine(res);
```

Delete an index
-------------
You can delete an index using its name:

```php
await client.DeleteIndex("cities");
```

Wait indexing
-------------

All write operations return a `taskID` when the job is securely stored on our infrastructure but not when the job is published in your index. Even if it's extremely fast, you can easily ensure indexing is complete using the `waitTask` method on the `taskID` returned by a write operation.

For example, to wait for indexing of a new object:
```csharp
var res = await index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", 
                                                 ""population"":805235}"), "myID");
await index.WaitTask(res["taskID"].ToString());
```

If you want to ensure multiple objects have been indexed, you can only check the biggest taskID.

Batch writes
-------------

You may want to perform multiple operations with one API call to reduce latency.
We expose two methods to perform batch:
 * `AddObjects`: add an array of object using automatic `objectID` assignement
 * `SaveObjects`: add or update an array of object that contains an `objectID` attribute

Example using automatic `objectID` assignement:
```csharp
List<JObject> objs = new List<JObject>();
objs.Add(JObject.Parse(@"{""name"":""San Francisco"", 
                          ""population"":805235}"));
objs.Add(JObject.Parse(@"{""name"":""Los Angeles"", 
                          ""population"":3792621}"));
var res = await index.AddObjects(objs);
System.Diagnostics.Debug.WriteLine(res);
```

Example with user defined `objectID` (add or update):
```csharp
List<JObject> objs = new List<JObject>();
objs.Add(JObject.Parse(@"{""name"":""San Francisco"", 
                          ""population"": 805235,
                          ""objectID"":""SFO""}"));
objs.Add(JObject.Parse(@"{""name"":""Los Angeles"", 
                          ""population"": 3792621,
                          ""objectID"": ""LA""}"));
var res = await index.SaveObjects(objs);
System.Diagnostics.Debug.WriteLine(res);
```

Security / User API Keys
-------------

The admin API key provides full control of all your indexes. 
You can also generate user API keys to control security. 
These API keys can be restricted to a set of operations or/and restricted to a given index.

To list existing keys, you can use `listUserKeys` method:
```csharp
// Lists global API Keys
var keys = await client.ListUserKeys();
// Lists API Keys that can access only to this index
keys = await index.ListUserKeys();
```

Each key is defined by a set of rights that specify the authorized actions. The different rights are:
 * **search**: allows to search,
 * **addObject**: allows to add/update an object in the index,
 * **deleteObject**: allows to delete an existing object,
 * **deleteIndex**: allows to delete index content,
 * **settings**: allows to get index settings,
 * **editSettings**: allows to change index settings.

Example of API Key creation:
```csharp
// Creates a new global API key that can only perform search actions
var res = await client.AddUserKey(new String[] { "search" });
System.Diagnostics.Debug.WriteLine("Key: " + res["key"]);
// Creates a new API key that can only perform search action on this index
res = await index.AddUserKey(new String[] { "search" });
System.Diagnostics.Debug.WriteLine("Key: " + res["key"]);
```
You can also create a temporary API key that will be valid only for a specific period of time (in seconds):
```csharp
// Creates a new global API key that is valid for 300 seconds
var res = await client.AddUserKey(new String[] { "search" }, 300);
System.Diagnostics.Debug.WriteLine("Key: " + res["key"]);
// Creates a new index specific API key valid for 300 seconds
res = await index.AddUserKey(new String[] { "search" }, 300);
System.Diagnostics.Debug.WriteLine("Key: " + res["key"]);
```

Get the rights of a given key:
```csharp
// Gets the rights of a global key
var res = await client.GetUserKeyACL("f420238212c54dcfad07ea0aa6d5c45f");
// Gets the rights of an index specific key
res = await index.GetUserKeyACL("71671c38001bf3ac857bc82052485107");
```

Delete an existing key:
```csharp
// Deletes a global key
await client.DeleteUserKey("f420238212c54dcfad07ea0aa6d5c45f");
// Deletes an index specific key
await index.DeleteUserKey("71671c38001bf3ac857bc82052485107");
```
