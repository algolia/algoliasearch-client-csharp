# Algolia Search API Client for .NET 🔎 

[Algolia Search](https://www.algolia.com) is a hosted full-text, numerical,
and faceted search engine capable of delivering realtime results from the first keystroke.

The **Algolia Search API Client for .NET** lets
you easily use the [Algolia Search REST API](https://www.algolia.com/doc/rest-api/search) from
your C#/F#/VB code.

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

Compatibilities:
 * `.NET Standard 1.3` to `.NET Standard 2.0`,
 * `.NET Core 1.0` to `.NET Core 2.2`,
 * `.NET Framework 4.5` to `.NET Framework 4.7.1`
  

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

Without any prior configuration, you can start indexing [500 contacts](https://github.com/algolia/datasets/blob/master/movies/actors.json) in the ```contacts``` index using the following code:

```csharp
  SearchIndex index = client.InitIndex("actors");

  using (StreamReader re = File.OpenText("actors.json"))
  using (JsonTextReader reader = new JsonTextReader(re))
  {
      JArray batch = JArray.Load(reader);
      index.SaveObjects(batch);
      // Asynchronous
      // index.SaveObjectsAsync(batch);
  }
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

You can now search for contacts using `firstname`, `lastname`, `company`, etc. (even with typos):

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
  index.Search<Contact>( new Query { "jimmie paint"" });

  // Asynchronous
  await index.SearchAsync<Contact>(new Query { "jimmie paint" });
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

## Getting Help

- **Need help**? Ask a question to the [Algolia Community](https://discourse.algolia.com/) or on [Stack Overflow](http://stackoverflow.com/questions/tagged/algolia).
- **Found a bug?** You can open a [GitHub issue](https://github.com/algolia/algoliasearch-client-csharp/issues).
