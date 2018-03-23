# Algolia Search API Client for C#

[Algolia Search](https://www.algolia.com) is a hosted full-text, numerical,
and faceted search engine capable of delivering realtime results from the first keystroke.

The **Algolia Search API Client for C#** lets
you easily use the [Algolia Search REST API](https://www.algolia.com/doc/rest-api/search) from
your C# code.

[![Build status](https://ci.appveyor.com/api/projects/status/r4c5ld2wh6bkvu7s?svg=true)](https://ci.appveyor.com/project/Algolia/algoliasearch-client-csharp)






## API Documentation

You can find the full reference on [Algolia's website](https://www.algolia.com/doc/api-client/csharp/).


## In this page



1. **[Install](#install)**


1. **[Quick Start](#quick-start)**


1. **[Push data](#push-data)**


1. **[Configure](#configure)**


1. **[Search](#search)**


1. **[Search UI](#search-ui)**


1. **[List of available methods](#list-of-available-methods)**


# Getting Started



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


































## Getting Help

- **Need help**? Ask a question to the [Algolia Community](https://discourse.algolia.com/) or on [Stack Overflow](http://stackoverflow.com/questions/tagged/algolia).
- **Found a bug?** You can open a [GitHub issue](https://github.com/algolia/algoliasearch-client-csharp/issues).

