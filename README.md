# Algolia Search API Client for .NET

[Algolia Search](https://www.algolia.com) is a hosted search engine capable of delivering realtime results from the first keystroke.

The **Algolia Search API Client for .NET** lets
you easily use the [Algolia Search REST API](https://www.algolia.com/doc/rest-api/search) from
your .NET code.

[![NuGet version (Algolia.Search](https://img.shields.io/nuget/v/Algolia.Search.svg?style=flat-square)](https://www.nuget.org/packages/Algolia.Search/)
[![Build Status](https://dev.azure.com/algolia-api-clients/dotnet/_apis/build/status/Algolia.Search.CI?branchName=master)](https://dev.azure.com/algolia-api-clients/dotnet/_build/latest?definitionId=2&branchName=master)


### Migration note from v5.x to v6.x

In January 2019, we released v6 of our .NET client. If you are using version 5.x of the client, read the [migration guide to version 6.x](https://www.algolia.com/doc/api-client/getting-started/upgrade-guides/csharp/).
Version 5.x will **no longer** be under active development.

**Note:** If you're using ASP.NET, checkout the [following tutorial](https://www.algolia.com/doc/api-client/getting-started/tutorials/asp.net/csharp/).




## API Documentation

You can find the full reference on [Algolia's website](https://www.algolia.com/doc/api-client/csharp/).



1. **[Supported platforms](#supported-platforms)**


1. **[Install](#install)**


1. **[Quick Start](#quick-start)**


1. **[Push data](#push-data)**


1. **[Configure](#configure)**


1. **[Search](#search)**


1. **[Search UI](#search-ui)**


1. **[List of available methods](#list-of-available-methods)**


# Getting Started



## Supported platforms

The API client follows **.NET Standard** thus it's compatible with:
  * `.NET Standard 1.3` to `.NET Standard 2.0`,
  * `.NET Core 1.0` to `.NET Core 2.2`,
  * `.NET Framework 4.5` to `.NET Framework 4.7.1`

## Install

### With the `.NET CLI`:

```sh*
dotnet add package Algolia.Search
```

### With the `Nuget Package Manager Console`:

```sh*
Install-Package Algolia.Search
```

### With [Nuget.org](https://www.nuget.org/packages/Algolia.Search/)

Download the package on [Nuget.org](https://www.nuget.org/packages/Algolia.Search/).

#### POCO, Types and Json.NET

The API client is using [Json.NET](https://www.newtonsoft.com/json) as serializer.

**The Client is meant to be used with POCOs and Types to improve type safety and developer experience. You can directly index your POCOs if they follow the [.NET naming convention](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/capitalization-conventions) for class and properties:**
* PascalCase for property names
* PascalCase for class name

Example:

```csharp
public class Contact
{
  public string ObjectID { get; set; }
  public string Name { get; set; }
  public int Age { get; set; }
}

SearchClient client = new SearchClient("YourApplicationID", "YourAPIKey");
SearchIndex index = client.InitIndex("contact");

IEnumerable<Contact> contacts; // Fetch from DB or a Json file
index.SaveObjects(contacts);

// Retrieve one typed Contact
Contact contact = index.GetObject<Contact>("myId");

// Search one typed Contact
var result = index.Search<Contact>(new Query("contact"));
```

**Note:** If you can't follow the convention, you can still override the naming strategy with the following attribute `[JsonProperty(PropertyName = "propertyName")]`

However, it's still possible to use `JObject` to add and retrieve records.

```csharp
using (StreamReader re = File.OpenText("contacts.json"))
using (JsonTextReader reader = new JsonTextReader(re))
{
  JArray batch = JArray.Load(reader);
  index.SaveObjects(batch).Wait();
}

// Retrieve one JObject Contact
JObject contact = index.GetObject<JObject>("myId");
```

 Algolia objects such as `Rule`, `Synonym`, `Settings`, etc., are now typed. You can enjoy the completion of your favorite IDE while developing with the library.

Example with the `Settings` class:

```csharp
IndexSettings settings = new IndexSettings
{
    SearchableAttributes = new List<string> {"attribute1", "attribute2"},
    AttributesForFaceting = new List<string> {"filterOnly(attribute2)"},
    UnretrievableAttributes = new List<string> {"attribute1", "attribute2"},
    AttributesToRetrieve = new List<string> {"attribute3", "attribute4"}
    // etc.
};

index.SetSettings(settings);
```

#### Asynchronous & Synchronous Methods
The API client provides both `Async` and `Sync` methods for every API endpoint. Asynchronous methods are suffixed with the `Async` keyword.
You can use any of them depending on your needs.

```csharp
// Synchronous
Contact res = index.GetObject<Contact>("myId");

// Asynchronous
Contact res = await index.GetObjectAsync<Contact>("myId");
```

#### HttpClient Injection
The API client is using the built-in `HttpClient` of the .NET Framework. 

The `HttpClient` is wrapped in an interface: `IHttpRequester`.
If you wish to use another `HttpClient`, you can inject it through the constructor while instantiating a `SearchClient`, `AnalyticsClient`, and `InsightsClient`.

Example:

```csharp
IHttpRequester myCustomHttpClient = new MyCustomHttpClient();

SearchClient client = new SearchClient(
    new SearchConfig("YourApplicationId", "YourAdminAPIKey"),
    myCustomHttpClient
);
```

#### Multithreading
The client is designed to be thread-safe. You can use `SearchClient`, `AnalyticsClient`, and `InsightsClient` in a multithreaded environment.

#### Cross-Platform
As the API client is following `.NET Standard`, it can be used on **Windows, Linux, or MacOS**.
The library is continuously tested in all three environments. If you want more information about `.NET Standard`, you can visit [the official page](https://dotnet.microsoft.com/).

## Quick Start

In 30 seconds, this quick start tutorial will show you how to index and search objects.

### Initialize the client

To start, you need to initialize the client. To do this, you need your **Application ID** and **API Key**.
You can find both on [your Algolia account](https://www.algolia.com/api-keys).

```csharp
SearchClient client = new SearchClient("YourApplicationID", "YourAPIKey");
SearchIndex index = client.InitIndex("your_index_name");
```

## Push data

Without any prior configuration, you can start indexing [500 contacts](https://github.com/algolia/datasets/blob/master/contacts/contacts.json) in the ```contacts``` index using the following code:
```csharp
SearchIndex index = client.InitIndex("contacts");

using (StreamReader re = File.OpenText("contacts.json"))
using (JsonTextReader reader = new JsonTextReader(re))
{
    JArray batch = JArray.Load(reader);
    index.SaveObjects(batch, autoGenerateObjectId: true);
    // Asynchronous
    // index.SaveObjectsAsync(batch, autoGenerateObjectId: true);
}
```

## Configure

You can customize settings to fine tune the search behavior. For example, you can add a custom ranking by number of followers to further enhance the built-in relevance:

```csharp
IndexSettings settings = new IndexSettings
{
    CustomRanking = new List<string> { "desc(followers)"},
};

index.SetSettings(settings);

// Asynchronous
await index.SetSettingsAsync(settings);
```

You can also configure the list of attributes you want to index by order of importance (most important first).

**Note:** Algolia is designed to suggest results as you type, which means you'll generally search by prefix.
In this case, the order of attributes is crucial to decide which hit is the best.

```csharp
IndexSettings settings = new IndexSettings
{
    SearchableAttributes = new List<string>
        {"lastname", "firstname", "company", "email", "city"}
};

// Synchronous
index.SetSettings(settings);

// Asynchronous
await index.SetSettingsAsync(settings);
```

## Search

You can now search for contacts by `firstname`, `lastname`, `company`, etc. (even with typos):

```csharp
// Search for a first name
index.Search<Contact>(new Query { "jimmie" });
// Asynchronous
await index.SearchAsync<Contact>(new Query { "jimmie" });

// Search for a first name with typo
index.Search<Contact>(new Query { "jimie" });
// Asynchronous
await index.SearchAsync<Contact>( new Query { "jimie" });

// Search for a company
index.Search<Contact>( new Query { "california paint" });
// Asynchronous
await index.SearchAsync<Contact>( new Query { "california paint" });

// Search for a first name and a company
index.Search<Contact>( new Query { "jimmie paint" });
// Asynchronous
await index.SearchAsync<Contact>(new Query { "jimmie paint" });
```

## Search UI

**Warning:** If you're building a web application, you may be interested in using one of our
[front-end search UI libraries](https://www.algolia.com/doc/guides/building-search-ui/what-is-instantsearch/js/).

The following example shows how to quickly build a front-end search using
[InstantSearch.js](https://www.algolia.com/doc/guides/building-search-ui/what-is-instantsearch/js/)

### index.html

```html
<!doctype html>
<head>
  <meta charset="UTF-8">
  <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/instantsearch.css@7.1.0/themes/algolia.css" />
</head>
<body>
  <header>
    <div>
       <input id="search-input" placeholder="Search for products">
       <!-- We use a specific placeholder in the input to guide users in their search. -->
    
  </header>
  <main>
      
      
  </main>

  <script type="text/html" id="hit-template">
    
      <p class="hit-name">
        {}{ "attribute": "firstname" }{{/helpers.highlight}}
        {}{ "attribute": "lastname" }{{/helpers.highlight}}
      </p>
    
  </script>

  <script src="https://cdn.jsdelivr.net/npm/instantsearch.js@3.0.0"></script>
  <script src="app.js"></script>
</body>
```

### app.js

```js
// Replace with your own values
var searchClient = algoliasearch(
  'YourApplicationID',
  'YourAPIKey' // search only API key, no ADMIN key
);

var search = instantsearch({
  indexName: 'instant_search',
  searchClient: searchClient,
  routing: true,
  searchParameters: {
    hitsPerPage: 10
  }
});

search.addWidget(
  instantsearch.widgets.searchBox({
    container: '#search-input'
  })
);

search.addWidget(
  instantsearch.widgets.hits({
    container: '#hits',
    templates: {
      item: document.getElementById('hit-template').innerHTML,
      empty: "We didn't find any results for the search <em>\"{{query}}\"</em>"
    }
  })
);

search.start();
```




## List of available methods





### Personalization

- [Add strategy](https://algolia.com/doc/api-reference/api-methods/add-strategy/?language=csharp)
- [Get strategy](https://algolia.com/doc/api-reference/api-methods/get-strategy/?language=csharp)




### Search

- [Search index](https://algolia.com/doc/api-reference/api-methods/search/?language=csharp)
- [Search for facet values](https://algolia.com/doc/api-reference/api-methods/search-for-facet-values/?language=csharp)
- [Search multiple indices](https://algolia.com/doc/api-reference/api-methods/multiple-queries/?language=csharp)
- [Browse index](https://algolia.com/doc/api-reference/api-methods/browse/?language=csharp)




### Indexing

- [Add objects](https://algolia.com/doc/api-reference/api-methods/add-objects/?language=csharp)
- [Save objects](https://algolia.com/doc/api-reference/api-methods/save-objects/?language=csharp)
- [Partial update objects](https://algolia.com/doc/api-reference/api-methods/partial-update-objects/?language=csharp)
- [Delete objects](https://algolia.com/doc/api-reference/api-methods/delete-objects/?language=csharp)
- [Replace all objects](https://algolia.com/doc/api-reference/api-methods/replace-all-objects/?language=csharp)
- [Delete by](https://algolia.com/doc/api-reference/api-methods/delete-by/?language=csharp)
- [Clear objects](https://algolia.com/doc/api-reference/api-methods/clear-objects/?language=csharp)
- [Get objects](https://algolia.com/doc/api-reference/api-methods/get-objects/?language=csharp)
- [Custom batch](https://algolia.com/doc/api-reference/api-methods/batch/?language=csharp)




### Settings

- [Get settings](https://algolia.com/doc/api-reference/api-methods/get-settings/?language=csharp)
- [Set settings](https://algolia.com/doc/api-reference/api-methods/set-settings/?language=csharp)
- [Copy settings](https://algolia.com/doc/api-reference/api-methods/copy-settings/?language=csharp)




### Manage indices

- [List indices](https://algolia.com/doc/api-reference/api-methods/list-indices/?language=csharp)
- [Delete index](https://algolia.com/doc/api-reference/api-methods/delete-index/?language=csharp)
- [Copy index](https://algolia.com/doc/api-reference/api-methods/copy-index/?language=csharp)
- [Move index](https://algolia.com/doc/api-reference/api-methods/move-index/?language=csharp)




### API keys

- [Create secured API Key](https://algolia.com/doc/api-reference/api-methods/generate-secured-api-key/?language=csharp)
- [Add API Key](https://algolia.com/doc/api-reference/api-methods/add-api-key/?language=csharp)
- [Update API Key](https://algolia.com/doc/api-reference/api-methods/update-api-key/?language=csharp)
- [Delete API Key](https://algolia.com/doc/api-reference/api-methods/delete-api-key/?language=csharp)
- [Restore API Key](https://algolia.com/doc/api-reference/api-methods/restore-api-key/?language=csharp)
- [Get API Key permissions](https://algolia.com/doc/api-reference/api-methods/get-api-key/?language=csharp)
- [List API Keys](https://algolia.com/doc/api-reference/api-methods/list-api-keys/?language=csharp)




### Synonyms

- [Save synonym](https://algolia.com/doc/api-reference/api-methods/save-synonym/?language=csharp)
- [Batch synonyms](https://algolia.com/doc/api-reference/api-methods/batch-synonyms/?language=csharp)
- [Delete synonym](https://algolia.com/doc/api-reference/api-methods/delete-synonym/?language=csharp)
- [Clear all synonyms](https://algolia.com/doc/api-reference/api-methods/clear-synonyms/?language=csharp)
- [Get synonym](https://algolia.com/doc/api-reference/api-methods/get-synonym/?language=csharp)
- [Search synonyms](https://algolia.com/doc/api-reference/api-methods/search-synonyms/?language=csharp)
- [Replace all synonyms](https://algolia.com/doc/api-reference/api-methods/replace-all-synonyms/?language=csharp)
- [Copy synonyms](https://algolia.com/doc/api-reference/api-methods/copy-synonyms/?language=csharp)
- [Export Synonyms](https://algolia.com/doc/api-reference/api-methods/export-synonyms/?language=csharp)




### Query rules

- [Save rule](https://algolia.com/doc/api-reference/api-methods/save-rule/?language=csharp)
- [Batch rules](https://algolia.com/doc/api-reference/api-methods/batch-rules/?language=csharp)
- [Get rule](https://algolia.com/doc/api-reference/api-methods/get-rule/?language=csharp)
- [Delete rule](https://algolia.com/doc/api-reference/api-methods/delete-rule/?language=csharp)
- [Clear rules](https://algolia.com/doc/api-reference/api-methods/clear-rules/?language=csharp)
- [Search rules](https://algolia.com/doc/api-reference/api-methods/search-rules/?language=csharp)
- [Replace all rules](https://algolia.com/doc/api-reference/api-methods/replace-all-rules/?language=csharp)
- [Copy rules](https://algolia.com/doc/api-reference/api-methods/copy-rules/?language=csharp)
- [Export rules](https://algolia.com/doc/api-reference/api-methods/export-rules/?language=csharp)




### A/B Test

- [Add A/B test](https://algolia.com/doc/api-reference/api-methods/add-ab-test/?language=csharp)
- [Get A/B test](https://algolia.com/doc/api-reference/api-methods/get-ab-test/?language=csharp)
- [List A/B tests](https://algolia.com/doc/api-reference/api-methods/list-ab-tests/?language=csharp)
- [Stop A/B test](https://algolia.com/doc/api-reference/api-methods/stop-ab-test/?language=csharp)
- [Delete A/B test](https://algolia.com/doc/api-reference/api-methods/delete-ab-test/?language=csharp)




### Insights

- [Clicked Object IDs After Search](https://algolia.com/doc/api-reference/api-methods/clicked-object-ids-after-search/?language=csharp)
- [Clicked Object IDs](https://algolia.com/doc/api-reference/api-methods/clicked-object-ids/?language=csharp)
- [Clicked Filters](https://algolia.com/doc/api-reference/api-methods/clicked-filters/?language=csharp)
- [Converted Objects IDs After Search](https://algolia.com/doc/api-reference/api-methods/converted-object-ids-after-search/?language=csharp)
- [Converted Object IDs](https://algolia.com/doc/api-reference/api-methods/converted-object-ids/?language=csharp)
- [Converted Filters](https://algolia.com/doc/api-reference/api-methods/converted-filters/?language=csharp)
- [Viewed Object IDs](https://algolia.com/doc/api-reference/api-methods/viewed-object-ids/?language=csharp)
- [Viewed Filters](https://algolia.com/doc/api-reference/api-methods/viewed-filters/?language=csharp)




### MultiClusters

- [Assign or Move userID](https://algolia.com/doc/api-reference/api-methods/assign-user-id/?language=csharp)
- [Get top userID](https://algolia.com/doc/api-reference/api-methods/get-top-user-id/?language=csharp)
- [Get userID](https://algolia.com/doc/api-reference/api-methods/get-user-id/?language=csharp)
- [List clusters](https://algolia.com/doc/api-reference/api-methods/list-clusters/?language=csharp)
- [List userIDs](https://algolia.com/doc/api-reference/api-methods/list-user-id/?language=csharp)
- [Remove userID](https://algolia.com/doc/api-reference/api-methods/remove-user-id/?language=csharp)
- [Search userID](https://algolia.com/doc/api-reference/api-methods/search-user-id/?language=csharp)




### Advanced

- [Get logs](https://algolia.com/doc/api-reference/api-methods/get-logs/?language=csharp)
- [Configuring timeouts](https://algolia.com/doc/api-reference/api-methods/configuring-timeouts/?language=csharp)
- [Set extra header](https://algolia.com/doc/api-reference/api-methods/set-extra-header/?language=csharp)
- [Wait for operations](https://algolia.com/doc/api-reference/api-methods/wait-task/?language=csharp)





## Getting Help

- **Need help**? Ask a question to the [Algolia Community](https://discourse.algolia.com/) or on [Stack Overflow](http://stackoverflow.com/questions/tagged/algolia).
- **Found a bug?** You can open a [GitHub issue](https://github.com/algolia/algoliasearch-client-csharp/issues).

