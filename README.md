# Algolia Search API Client for .NET 🔎 

[Algolia Search](https://www.algolia.com) is a hosted full-text, numerical,
and faceted search engine capable of delivering realtime results from the first keystroke.

The **Algolia Search API Client for .NET** lets
you easily use the [Algolia Search REST API](https://www.algolia.com/doc/rest-api/search) from
your C#/F#/VB code.

  [![NuGet version (Algolia.Search](https://img.shields.io/nuget/v/Algolia.Search.svg?style=flat-square)](https://www.nuget.org/packages/Algolia.Search/)
  [![Build Status](https://dev.azure.com/algolia-api-clients/dotnet/_apis/build/status/Algolia.Search?branchName=client-csharp-v2)](https://dev.azure.com/algolia-api-clients/dotnet/_build/latest?definitionId=2?branchName=client-csharp-v2)

## API Documentation

You can find the full reference on [Algolia's website](https://deploy-preview-2388--algolia-doc.netlify.com/doc/api-client/getting-started/install/csharp/).

1. **[Supported platforms](#supported-platforms)**


2. **[Install](#install)**


3. **[Quick Start](#quick-start)**


4. **[Push data](#push-data)**


5. **[Configure](#configure)**


6. **[Search](#search)**


7. **[Search UI](#search-ui)**

# Getting Started

## Supported platforms

<<<<<<< HEAD
The API client is compatible with:
  * `.NET Framework 4.6`
  * `.NET Framework 4.6.2`
  * `.NET Framework 4.7`
  * `.NET Framework 4.7.1`
  * `.NET Core 1.0`
  * `.NET Core 1.1`
  * `.NET Core 2.0`
  * `.NETStandard 1.6`
  * `.NETStandard 1.3`
  * `.NETStandard 2.0`
=======
Compatibilities:
 * `.NET Standard 1.3` to `.NET Standard 2.0`,
 * `.NET Core 1.0` to `.NET Core 2.2`,
 * `.NET Framework 4.5` to `.NET Framework 4.7.1`
  
>>>>>>> client-csharp-v2

## Install

For the moment this version is not published on Nuget. You'lll have to clone the repo and build the `client-csharp-v2` branch.
## Quick Start

In 30 seconds, this quick start tutorial will show you how to index and search objects.

### Initialize the client

To begin, you will need to initialize the client. In order to do this you will need your **Application ID** and **API Key**.
You can find both on [your Algolia account](https://www.algolia.com/api-keys).

```csharp
  SearchClient client = new SearchClient("YourApplicationID", "YourAPIKey");
  SearchIndex index = client.InitIndex("your_index_name");
```

## Push data

Without any prior configuration, you can start indexing [500 contacts](https://github.com/algolia/datasets/blob/master/contacts/contacts.json) in the ```contacts``` index using the following code:

```csharp
<<<<<<< HEAD
// Load JSON file
using(StreamReader re = File.OpenText("contacts.json"))
using(JsonTextReader reader = new JsonTextReader(re))
{
   JArray batch = JArray.Load(reader);
};
// Add objects
Index index = client.InitIndex("contacts");
index.AddObjects(batch);
// Asynchronous
// await index.AddObjectsAsync(batch);
=======
 SearchIndex index = client.InitIndex("contacts");

  using (StreamReader re = File.OpenText("contacts.json"))
  using (JsonTextReader reader = new JsonTextReader(re))
  {
      JArray batch = JArray.Load(reader);
      index.SaveObjects(batch, autoGenerateObjectId: true);
      // Asynchronous
      // index.SaveObjectsAsync(batch, autoGenerateObjectId: true);
  }
>>>>>>> client-csharp-v2
```

## Configure

Settings can be customized to fine tune the search behavior. For example, you can add a custom sort by number of followers to further enhance the built-in relevance:

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

**Note:** The Algolia engine is designed to suggest results as you type, which means you'll generally search by prefix.
In this case, the order of attributes is very important to decide which hit is the best:

```csharp
<<<<<<< HEAD
var settings = new JObject
{
  { "searchableAttributes", new JArray { "lastname", "firstname", "company", "email", "city" }}
};

index.SetSettings(settings);

// Asynchronous
// await index.SetSettingsAsync(settings);
=======
  IndexSettings settings = new IndexSettings
  {
      SearchableAttributes = new List<string>
          {"lastname", "firstname", "company", "email", "city"}
  };

  // Synchronous
  index.SetSettings(settings);

  // Asynchronous
  await index.SetSettingsAsync(settings);
>>>>>>> client-csharp-v2
```

## Search

You can now search for contacts using `firstname`, `lastname`, `company`, etc. (even with typos):

```csharp
<<<<<<< HEAD
// Search for a first name
System.Diagnostics.Debug.WriteLine(index.Search(new Query("jimmie")));
// Asynchronous
// System.Diagnostics.Debug.WriteLine(await index.SearchAsync(new Query("jimmie")));
// Search for a first name with typo
System.Diagnostics.Debug.WriteLine(index.Search(new Query("jimie")));
// Asynchronous
// System.Diagnostics.Debug.WriteLine(await index.SearchAsync(new Query("jimie")));
// Search for a company
System.Diagnostics.Debug.WriteLine(index.Search(new Query("california paint")));
// Asynchronous
// System.Diagnostics.Debug.WriteLine(await index.SearchAsync(new Query("california paint")));
// Search for a first name and a company
System.Diagnostics.Debug.WriteLine(index.Search(new Query("jimmie paint")));
// Asynchronous
// System.Diagnostics.Debug.WriteLine(await index.SearchAsync(new Query("jimmie paint")));
=======
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
  await index.SearchAsync<Contact>(new Query { "california paint" });
  
  // Search for a first name and a company
  index.Search<Contact>(new Query { "jimmie paint" });

  // Asynchronous
  await index.SearchAsync<Contact>(new Query { "jimmie paint" });
>>>>>>> client-csharp-v2
```

## Search UI

**Warning:** If you are building a web application, you may be more interested in using one of our
[frontend search UI libraries](https://www.algolia.com/doc/guides/search-ui/search-libraries/)

The following example shows how to build a front-end search quickly using
[InstantSearch.js](https://community.algolia.com/instantsearch.js/)

### index.html

```html
<!doctype html>
<head>
  <meta charset="UTF-8">
  <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/instantsearch.js@2.3/dist/instantsearch.min.css">
  <!-- Always use `2.x` versions in production rather than `2` to mitigate any side effects on your website,
  Find the latest version on InstantSearch.js website: https://community.algolia.com/instantsearch.js/v2/guides/usage.html -->
</head>
<body>
  <header>
    <div>
       <input id="search-input" placeholder="Search for products">
       <!-- We use a specific placeholder in the input to guides users in their search. -->
    
  </header>
  <main>
      
      
  </main>

  <script type="text/html" id="hit-template">
    
      <p class="hit-name">{{{_highlightResult.firstname.value}}} {{{_highlightResult.lastname.value}}}</p>
    
  </script>

  <script src="https://cdn.jsdelivr.net/npm/instantsearch.js@2.3/dist/instantsearch.min.js"></script>
  <script src="app.js"></script>
</body>
```

### app.js

```js
var search = instantsearch({
  // Replace with your own values
  appId: 'YourApplicationID',
  apiKey: 'YourSearchOnlyAPIKey', // search only API key, no ADMIN key
  indexName: 'contacts',
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

<<<<<<< HEAD



## List of available methods





### Personalization





### Search

- [Search index](https://algolia.com/doc/api-reference/api-methods/search/?language=csharp)
- [Search for facet values](https://algolia.com/doc/api-reference/api-methods/search-for-facet-values/?language=csharp)
- [Search multiple indices](https://algolia.com/doc/api-reference/api-methods/multiple-queries/?language=csharp)
- [Browse index](https://algolia.com/doc/api-reference/api-methods/browse/?language=csharp)




### Indexing

- [Add objects](https://algolia.com/doc/api-reference/api-methods/add-objects/?language=csharp)
- [Update objects](https://algolia.com/doc/api-reference/api-methods/save-objects/?language=csharp)
- [Partial update objects](https://algolia.com/doc/api-reference/api-methods/partial-update-objects/?language=csharp)
- [Delete objects](https://algolia.com/doc/api-reference/api-methods/delete-objects/?language=csharp)
- [Delete by](https://algolia.com/doc/api-reference/api-methods/delete-by/?language=csharp)
- [Get objects](https://algolia.com/doc/api-reference/api-methods/get-objects/?language=csharp)
- [Custom batch](https://algolia.com/doc/api-reference/api-methods/batch/?language=csharp)




### Settings

- [Get settings](https://algolia.com/doc/api-reference/api-methods/get-settings/?language=csharp)
- [Set settings](https://algolia.com/doc/api-reference/api-methods/set-settings/?language=csharp)




### Manage indices

- [List indexes](https://algolia.com/doc/api-reference/api-methods/list-indices/?language=csharp)
- [Delete index](https://algolia.com/doc/api-reference/api-methods/delete-index/?language=csharp)
- [Copy index](https://algolia.com/doc/api-reference/api-methods/copy-index/?language=csharp)
- [Move index](https://algolia.com/doc/api-reference/api-methods/move-index/?language=csharp)
- [Clear index](https://algolia.com/doc/api-reference/api-methods/clear-index/?language=csharp)




### API Keys

- [Create secured API Key](https://algolia.com/doc/api-reference/api-methods/generate-secured-api-key/?language=csharp)
- [Add API Key](https://algolia.com/doc/api-reference/api-methods/add-api-key/?language=csharp)
- [Update API Key](https://algolia.com/doc/api-reference/api-methods/update-api-key/?language=csharp)
- [Delete API Key](https://algolia.com/doc/api-reference/api-methods/delete-api-key/?language=csharp)
- [Get API Key permissions](https://algolia.com/doc/api-reference/api-methods/get-api-key/?language=csharp)
- [List API Keys](https://algolia.com/doc/api-reference/api-methods/list-api-keys/?language=csharp)




### Synonyms

- [Save synonym](https://algolia.com/doc/api-reference/api-methods/save-synonym/?language=csharp)
- [Batch synonyms](https://algolia.com/doc/api-reference/api-methods/batch-synonyms/?language=csharp)
- [Delete synonym](https://algolia.com/doc/api-reference/api-methods/delete-synonym/?language=csharp)
- [Clear all synonyms](https://algolia.com/doc/api-reference/api-methods/clear-synonyms/?language=csharp)
- [Get synonym](https://algolia.com/doc/api-reference/api-methods/get-synonym/?language=csharp)
- [Search synonyms](https://algolia.com/doc/api-reference/api-methods/search-synonyms/?language=csharp)
- [Export Synonyms](https://algolia.com/doc/api-reference/api-methods/export-synonyms/?language=csharp)




### Query rules

- [Save rule](https://algolia.com/doc/api-reference/api-methods/save-rule/?language=csharp)
- [Batch rules](https://algolia.com/doc/api-reference/api-methods/batch-rules/?language=csharp)
- [Get rule](https://algolia.com/doc/api-reference/api-methods/get-rule/?language=csharp)
- [Delete rule](https://algolia.com/doc/api-reference/api-methods/delete-rule/?language=csharp)
- [Clear rules](https://algolia.com/doc/api-reference/api-methods/clear-rules/?language=csharp)
- [Search rules](https://algolia.com/doc/api-reference/api-methods/search-rules/?language=csharp)
- [Export rules](https://algolia.com/doc/api-reference/api-methods/export-rules/?language=csharp)




### A/B Test

- [Add A/B test](https://algolia.com/doc/api-reference/api-methods/add-ab-test/?language=csharp)
- [Get A/B test](https://algolia.com/doc/api-reference/api-methods/get-ab-test/?language=csharp)
- [List A/B tests](https://algolia.com/doc/api-reference/api-methods/list-ab-tests/?language=csharp)
- [Stop A/B test](https://algolia.com/doc/api-reference/api-methods/stop-ab-test/?language=csharp)
- [Delete A/B test](https://algolia.com/doc/api-reference/api-methods/delete-ab-test/?language=csharp)




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





=======
>>>>>>> client-csharp-v2
## Getting Help

- **Need help**? Ask a question to the [Algolia Community](https://discourse.algolia.com/) or on [Stack Overflow](http://stackoverflow.com/questions/tagged/algolia).
- **Found a bug?** You can open a [GitHub issue](https://github.com/algolia/algoliasearch-client-csharp/issues).
