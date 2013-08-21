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

Without any prior-configuration, you can index [500 contacts](https://github.com/algolia/algoliasearch-client-csharp/blob/master/contacts.json) in the ```contacts``` index with the following code:
```csharp
// Load JSON file
StreamReader re = File.OpenText("contacts.json");
JsonTextReader reader = new JsonTextReader(re);
JArray batch = JArray.Load(reader);
// Add objects 
Index index = client.InitIndex("contacts");
await index.AddObjects(batch);
```

You can then start to search for a contact firstname, lastname, company, ... (even with typos):
```csharp
// search by firstname
System.Diagnostics.Debug.WriteLine(await index.Search(new Query("jimmie")));
// search a firstname with typo
System.Diagnostics.Debug.WriteLine(await index.Search(new Query("jimie")));
// search for a company
System.Diagnostics.Debug.WriteLine(await index.Search(new Query("california paint")));
// search for a firstname & company
System.Diagnostics.Debug.WriteLine(await index.Search(new Query("jimmie paint")));
```

Settings can be customized to tune the search behavior. For example you can add a custom sort by number of followers to the already good out-of-the-box relevance:
```csharp
await index.SetSettings(JObject.Parse(@"{""customRanking"":[""desc(followers)""]}"));
```
You can also configure the list of attributes you want to index by order of importance (first = most important):
```csharp
await index.SetSettings(JObject.Parse(@"{""attributesToIndex"":[""lastname"", ""firstname"",
                                                                ""company"", ""email"", ""city""]}"));
```

Since the engine is designed to suggest results as you type, you'll generally search by prefix. In this case the order of attributes is very important to decide which hit is the best:
```csharp
System.Diagnostics.Debug.WriteLine(await index.Search(new Query("or")));
System.Diagnostics.Debug.WriteLine(await index.Search(new Query("jim")));
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
Index index = client.InitIndex("contacts");
res = await index.Search(new Query("query string"));
System.Diagnostics.Debug.WriteLine(res);
res = await index.Search(new Query("query string").
    SetAttributesToRetrieve(new string[] {"firstname","lastname"}).
    SetNbHitsPerPage(50));
System.Diagnostics.Debug.WriteLine(res);
```

The server response will look like:

```javascript
{
  "hits": [
    {
      "firstname": "Jimmie",
      "lastname": "Barninger",
      "company": "California Paint & Wlpaper Str",
      "address": "Box #-4038",
      "city": "Modesto",
      "county": "Stanislaus",
      "state": "CA",
      "zip": "95352",
      "phone": "209-525-7568",
      "fax": "209-525-4389",
      "email": "jimmie@barninger.com",
      "web": "http://www.jimmiebarninger.com",
      "followers": 3947,
      "objectID": "433",
      "_highlightResult": {
        "firstname": {
          "value": "<em>Jimmie</em>",
          "matchLevel": "partial"
        },
        "lastname": {
          "value": "Barninger",
          "matchLevel": "none"
        },
        "company": {
          "value": "California <em>Paint</em> & Wlpaper Str",
          "matchLevel": "partial"
        },
        "address": {
          "value": "Box #-4038",
          "matchLevel": "none"
        },
        "city": {
          "value": "Modesto",
          "matchLevel": "none"
        },
        "email": {
          "value": "<em>jimmie</em>@barninger.com",
          "matchLevel": "partial"
        }
      }
    }
  ],
  "page": 0,
  "nbHits": 1,
  "nbPages": 1,
  "hitsPerPage": 20,
  "processingTimeMS": 1,
  "query": "jimmie paint",
  "params": "query=jimmie+paint&"
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
var res = await index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", 
                                                 ""lastname"":""Barninger""}"));
System.Diagnostics.Debug.WriteLine("objectID=" + res["objectID"]);           
```

Example with manual `objectID` assignement:

```charp
var res = await index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", 
                                                 ""lastname"":""Barninger""}"), "myID");
System.Diagnostics.Debug.WriteLine("objectID=" + res["objectID"]);
```

Update an existing object in the Index
-------------

You have two options to update an existing object:

 1. Replace all its attributes.
 2. Replace only some attributes.

Example to replace all the content of an existing object:

```csharp
await index.SaveObject(JObject.Parse(@"{""firstname"":""Jimmie"", 
                                        ""lastname"":""Barninger"", 
                                        ""city"":""New York"",
                                        ""objectID"":""myID""}"));
```

Example to update only the city attribute of an existing object:

```csharp
await index.PartialUpdateObject(JObject.Parse(@"{""city"":""San Francisco"", 
                                                 ""objectID"":""myID""}"));
```

Get an object
-------------

You can easily retrieve an object using its `objectID` and optionnaly a list of attributes you want to retrieve (using comma as separator):

```csharp
// Retrieves all attributes
var res = await index.GetObject("myID");
System.Diagnostics.Debug.WriteLine(res);
// Retrieves firstname and lastname attributes
res = await index.GetObject("myID", new String[] {"firstname", "lastname"});
System.Diagnostics.Debug.WriteLine(res);
// Retrieves only the firstname attribute
res = await index.GetObject("myID", new String[] { "firstname" });
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
 * **ranking**: (array of strings) controls the way hits are sorted.<br/>We have six available criteria:
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
await index.SetSettings(JObject.Parse(@"{""customRanking"":[""desc(followers)""]}"));
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
await client.DeleteIndex("contacts");
```

Wait indexing
-------------

All write operations return a `taskID` when the job is securely stored on our infrastructure but not when the job is published in your index. Even if it's extremely fast, you can easily ensure indexing is complete using the `waitTask` method on the `taskID` returned by a write operation.

For example, to wait for indexing of a new object:
```csharp
var res = await index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", 
                                                 ""lastname"":""Barninger""}"), "myID");
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
objs.Add(JObject.Parse(@"{""firstname"":""Jimmie"", 
                          ""lastname"":""Barninger""}"));
objs.Add(JObject.Parse(@"{""firstname"":""Warren"", 
                          ""lastname"":""Speach""}"));
var res = await index.AddObjects(objs);
System.Diagnostics.Debug.WriteLine(res);
```

Example with user defined `objectID` (add or update):
```csharp
List<JObject> objs = new List<JObject>();
objs.Add(JObject.Parse(@"{""firstname"":""Jimmie"", 
                          ""lastname"":""Barninger"",
                          ""objectID"":""myID1""}"));
objs.Add(JObject.Parse(@"{""firstname"":""Warren"", 
                          ""lastname"":""Speach"",
                          ""objectID"": ""myID2""}"));
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
