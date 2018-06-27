# Algolia Search API Client for C#

[Algolia Search](https://www.algolia.com) is a hosted full-text, numerical,
and faceted search engine capable of delivering realtime results from the first keystroke.

The **Algolia Search API Client for C#** lets
you easily use the [Algolia Search REST API](https://www.algolia.com/doc/rest-api/search) from
your C# code.

[![Build status](https://ci.appveyor.com/api/projects/status/r4c5ld2wh6bkvu7s?svg=true)](https://ci.appveyor.com/project/Algolia/algoliasearch-client-csharp)






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

Compatibilities:
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
  

## Install

* In your project, open the `Package Manager Console` (`Tools` → `Library Package Manager` → `Package Manager Console`)
* Enter `Install-Package Algolia.Search` in the `Package Manager Console`

## Quick Start

In 30 seconds, this quick start tutorial will show you how to index and search objects.

### Initialize the client

To begin, you will need to initialize the client. In order to do this you will need your **Application ID** and **API Key**.
You can find both on [your Algolia account](https://www.algolia.com/api-keys).

```csharp
AlgoliaClient client = new AlgoliaClient("YourApplicationID", "YourAPIKey");
Index index = client.InitIndex("your_index_name");
```

## Push data

Without any prior configuration, you can start indexing [500 contacts](https://github.com/algolia/datasets/blob/master/contacts/contacts.json) in the ```contacts``` index using the following code:
```csharp
// Load JSON file
StreamReader re = File.OpenText("contacts.json");
JsonTextReader reader = new JsonTextReader(re);
JArray batch = JArray.Load(reader);
// Add objects
Index index = client.InitIndex("contacts");
index.AddObjects(batch);
// Asynchronous
// await index.AddObjectsAsync(batch);
```

## Configure

Settings can be customized to fine tune the search behavior. For example, you can add a custom sort by number of followers to further enhance the built-in relevance:

```csharp
index.SetSettings(JObject.Parse(@"{""customRanking"":[""desc(followers)""]}"));
// Asynchronous
// await index.SetSettingsAsync(JObject.Parse(@"{""customRanking"":[""desc(followers)""]}"));
```

You can also configure the list of attributes you want to index by order of importance (most important first).

**Note:** The Algolia engine is designed to suggest results as you type, which means you'll generally search by prefix.
In this case, the order of attributes is very important to decide which hit is the best:

```csharp
index.SetSettings(JObject.Parse(@"{""searchableAttributes"":[""lastname"", ""firstname"",
                                                          ""company"", ""email"", ""city""]}"));
// Asynchronous
//await index.SetSettingsAsync(JObject.Parse(@"{""searchableAttributes"":[""lastname"", ""firstname"",
//                                                                     ""company"", ""email"", ""city""]}"));
```

## Search

You can now search for contacts using `firstname`, `lastname`, `company`, etc. (even with typos):

```csharp
// search by firstname
System.Diagnostics.Debug.WriteLine(index.Search(new Query("jimmie")));
// Asynchronous
// System.Diagnostics.Debug.WriteLine(await index.SearchAsync(new Query("jimmie")));
// search a firstname with typo
System.Diagnostics.Debug.WriteLine(index.Search(new Query("jimie")));
// Asynchronous
// System.Diagnostics.Debug.WriteLine(await index.SearchAsync(new Query("jimie")));
// search for a company
System.Diagnostics.Debug.WriteLine(index.Search(new Query("california paint")));
// Asynchronous
// System.Diagnostics.Debug.WriteLine(await index.SearchAsync(new Query("california paint")));
// search for a firstname & company
System.Diagnostics.Debug.WriteLine(index.Search(new Query("jimmie paint")));
// Asynchronous
// System.Diagnostics.Debug.WriteLine(await index.SearchAsync(new Query("jimmie paint")));
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
  urlSync: true,
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





### Search

- [Search index](https://algolia.com/doc/api-reference/api-methods/search/?language=csharp)
- [Search for facet values](https://algolia.com/doc/api-reference/api-methods/search-for-facet-values/?language=csharp)
- [Search multiple indexes](https://algolia.com/doc/api-reference/api-methods/multiple-queries/?language=csharp)
- [Browse index](https://algolia.com/doc/api-reference/api-methods/browse/?language=csharp)




### Indexing

- [Add objects](https://algolia.com/doc/api-reference/api-methods/add-objects/?language=csharp)
- [Update objects](https://algolia.com/doc/api-reference/api-methods/update-objects/?language=csharp)
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

- [Save rule](https://algolia.com/doc/api-reference/api-methods/rules-save/?language=csharp)
- [Batch rules](https://algolia.com/doc/api-reference/api-methods/rules-save-batch/?language=csharp)
- [Get rule](https://algolia.com/doc/api-reference/api-methods/rules-get/?language=csharp)
- [Delete rule](https://algolia.com/doc/api-reference/api-methods/rules-delete/?language=csharp)
- [Clear rules](https://algolia.com/doc/api-reference/api-methods/rules-clear/?language=csharp)
- [Search rules](https://algolia.com/doc/api-reference/api-methods/rules-search/?language=csharp)
- [Export rules](https://algolia.com/doc/api-reference/api-methods/rules-export/?language=csharp)




### A/B Test

- [Add A/B test](https://algolia.com/doc/api-reference/api-methods/add-ab-test/?language=csharp)
- [Get A/B test](https://algolia.com/doc/api-reference/api-methods/get-ab-test/?language=csharp)
- [List A/B tests](https://algolia.com/doc/api-reference/api-methods/get-ab-tests/?language=csharp)
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





## Getting Help

- **Need help**? Ask a question to the [Algolia Community](https://discourse.algolia.com/) or on [Stack Overflow](http://stackoverflow.com/questions/tagged/algolia).
- **Found a bug?** You can open a [GitHub issue](https://github.com/algolia/algoliasearch-client-csharp/issues).

