
<p align="center">
  <a href="https://www.algolia.com">
    <img alt="Algolia for C#" src="https://user-images.githubusercontent.com/22633119/59595424-10d10880-90f6-11e9-9303-823f70b39d6c.png" >
  </a>

<h4 align="center">The perfect starting point to integrate <a href="https://algolia.com" target="_blank">Algolia</a> within your .NET project</h4>

<p align="center">
  <a href="https://www.nuget.org/packages/Algolia.Search/"><img src="https://img.shields.io/nuget/v/Algolia.Search.svg?style=flat-square" alt="Nuget"></img></a>
  <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/License-MIT-yellow.svg" alt="Licence"></img></a>
</p>

<p align="center">
  <a href="https://www.algolia.com/doc/libraries/csharp/v7/" target="_blank">Documentation</a>  •
  <a href="https://discourse.algolia.com" target="_blank">Community Forum</a>  •
  <a href="http://stackoverflow.com/questions/tagged/algolia" target="_blank">Stack Overflow</a>  •
  <a href="https://github.com/algolia/algoliasearch-client-csharp/issues" target="_blank">Report a bug</a>  •
  <a href="https://alg.li/support" target="_blank">Support</a>
</p>

## ✨ Features

* Targets .NET Standard: `.NET Standard 2.0` or `.NET Standard 2.1`.
* For more details about supported .NET implementations, please see .NET Standard official [page](https://docs.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-2-1).
* Asynchronous and synchronous methods to interact with Algolia's API.
* Thread-safe clients.
* No external dependencies.
* Typed requests and responses.
* Injectable HTTP client.
* Retry strategy & Helpers.

**Migration note from v5.x to v6.x**

> In January 2019, we released v6 of our .NET client. If you are using version 5.x of the client, read the [migration guide to version 6.x](https://www.algolia.com/doc/api-client/getting-started/upgrade-guides/csharp/).
Version 5.x will **no longer** be under active development.

## 💡 Getting Started

To get started, first install the Algolia.Search client.

You can get the last version of the client from [NuGet](https://www.nuget.org/packages/Algolia.Search/).

If you are using the .NET CLI, you can install the package using the following command:

```bash
dotnet add package Algolia.Search --version <The version you want to install>
```

Or directly in your .csproj file:

```csharp
<PackageReference Include="Algolia.Search" Version=${versions.csharp} />
```

You can now import the Algolia API client in your project and play with it.

```csharp
using Algolia.Search.Clients;
using Algolia.Search.Http;

var client = new SearchClient(new SearchConfig("YOUR_APP_ID", "YOUR_API_KEY"));

// Add a new record to your Algolia index
var response = await client.SaveObjectAsync(
  "<YOUR_INDEX_NAME>",
  new Dictionary<string, string> { { "objectID", "id" }, { "test", "val" } }
);

// Poll the task status to know when it has been indexed
await client.WaitForTaskAsync("<YOUR_INDEX_NAME>", response.TaskID);

// Fetch search results, with typo tolerance
var response = await client.SearchAsync<Object>(
  new SearchMethodParams
  {
    Requests = new List<SearchQuery>
    {
      new SearchQuery(
        new SearchForHits
        {
          IndexName = "<YOUR_INDEX_NAME>",
          Query = "<YOUR_QUERY>",
          HitsPerPage = 50,
        }
      )
    },
  }
);
```

For full documentation, visit the **[Algolia CSharp API Client](https://www.algolia.com/doc/libraries/csharp/)**.

## ❓ Troubleshooting

Encountering an issue? Before reaching out to support, we recommend heading to our [FAQ](https://support.algolia.com/hc/sections/15061037630609-API-Client-FAQs) where you will find answers for the most common issues and gotchas with the client. You can also open [a GitHub issue](https://github.com/algolia/api-clients-automation/issues/new?assignees=&labels=&projects=&template=Bug_report.md)

## Contributing

This repository hosts the code of the generated Algolia API client for CSharp, if you'd like to contribute, head over to the [main repository](https://github.com/algolia/api-clients-automation). You can also find contributing guides on [our documentation website](https://api-clients-automation.netlify.app/docs/introduction).

## 📄 License

The Algolia .NET API Client is an open-sourced software licensed under the [MIT license](LICENSE).
