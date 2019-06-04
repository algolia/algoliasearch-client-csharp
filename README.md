
<p align="center">
  <a href="https://www.algolia.com">
    <img alt="Algolia for C#" src="https://raw.githubusercontent.com/algolia/algoliasearch-client-common/master/readme-banner.png" >
  </a>

  <h4 align="center">The perfect starting point to integrate <a href="https://algolia.com" target="_blank">Algolia</a> within your .NET project</h4>

  <p align="center">
    <a href="https://www.nuget.org/packages/Algolia.Search/"><img src="https://img.shields.io/nuget/v/Algolia.Search.svg?style=flat-square" alt="Nuget"></img></a>
    <a href="https://dev.azure.com/algolia-api-clients/dotnet/_build/latest?definitionId=2&branchName=master"><img src="https://dev.azure.com/algolia-api-clients/dotnet/_apis/build/status/Algolia.Search.CI?branchName=master" alt="Build status"></img></a>
    <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/License-MIT-yellow.svg" alt="Licence"></img></a>
  </p>
</p>

<p align="center">
  <a href="https://www.algolia.com/doc/api-client/getting-started/install/csharp/" target="_blank">Documentation</a>  •
  <a href="https://discourse.algolia.com" target="_blank">Community Forum</a>  •
  <a href="http://stackoverflow.com/questions/tagged/algolia" target="_blank">Stack Overflow</a>  •
  <a href="https://github.com/algolia/algoliasearch-client-csharp/issues" target="_blank">Report a bug</a>  •
  <a href="https://www.algolia.com/support" target="_blank">Support</a>
</p>

## ✨ Features

* Compatible with all .NET platforms:
  * `.NET Standard 1.3` to `.NET Standard 2.0`,
  * `.NET Core 1.0` to `.NET Core 2.2`,
  * `.NET Framework 4.5` to `.NET Framework 4.7.1`
* Asynchronous and synchronous methods to interact with Algolia's API
* Thread-safe clients
* Typed requests and responses
* Injectable HTTP client

 **Migration note from v5.x to v6.x**
>
> In January 2019, we released v6 of our .NET client. If you are using version 5.x of the client, read the [migration guide to version 6.x](https://www.algolia.com/doc/api-client/getting-started/upgrade-guides/csharp/).
Version 5.x will **no longer** be under active development.

## 💡 Getting Started

 Install the library with the `.NET CLI`:

```sh*
dotnet add package Algolia.Search
```

or with the `Nuget Package Manager Console`:

```sh*
Install-Package Algolia.Search
```

 In 30 seconds, this quick start tutorial will show you how to index and search objects.

#### Initialize the cient

To start, you need to initialize the client. To do this, you need your **Application ID** and **API Key**.
You can find both on [your Algolia account](https://www.algolia.com/api-keys).

```csharp
SearchClient client = new SearchClient("YourApplicationID", "YourAPIKey");
SearchIndex index = client.InitIndex("your_index_name");
```

#### Push data

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

#### Configure

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

#### Search

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

// Search for a first name and a company
index.Search<Contact>( new Query { "jimmie paint" });
// Asynchronous
await index.SearchAsync<Contact>(new Query { "jimmie paint" });
```

## 🎓 Client Philosophy

#### POCOs, Types and Json.NET

The Client is meant to be used with POCOs and Types to improve type safety and developer experience. You can directly index your POCOs if they follow the [.NET naming convention](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/capitalization-conventions) for class and properties:

* PascalCase for property names
* PascalCase for class name

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

#### ASP.NET
If you're using ASP.NET, checkout the [following tutorial](https://www.algolia.com/doc/api-client/getting-started/tutorials/asp.net/csharp/). 

For full documentation, visit the **[Algolia .NET API Client documentation](https://www.algolia.com/doc/api-client/getting-started/install/csharp/)**.

## 📄 License

Algolia .NET API Client is an open-sourced software licensed under the [MIT license](LICENSE.md).
