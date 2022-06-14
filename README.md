
<p align="center">
  <a href="https://www.algolia.com">
    <img alt="Algolia for C#" src="https://user-images.githubusercontent.com/22633119/59595424-10d10880-90f6-11e9-9303-823f70b39d6c.png" >
  </a>

  <h4 align="center">The perfect starting point to integrate <a href="https://algolia.com" target="_blank">Algolia</a> within your .NET project</h4>

  <p align="center">
    <a href="https://www.nuget.org/packages/Algolia.Search/"><img src="https://img.shields.io/nuget/v/Algolia.Search.svg?style=flat-square" alt="Nuget"></img></a>
        <a href="https://circleci.com/gh/algolia/algoliasearch-client-csharp"><img src="https://circleci.com/gh/algolia/algoliasearch-client-csharp.svg?style=shield" alt="CircleCI"></img></a>
    <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/License-MIT-yellow.svg" alt="Licence"></img></a>
  </p>
</p>

<p align="center">
  <a href="https://www.algolia.com/doc/api-client/getting-started/install/csharp/" target="_blank">Documentation</a>  •
  <a href="https://discourse.algolia.com" target="_blank">Community Forum</a>  •
  <a href="http://stackoverflow.com/questions/tagged/algolia" target="_blank">Stack Overflow</a>  •
  <a href="https://github.com/algolia/algoliasearch-client-csharp/issues" target="_blank">Report a bug</a>  •
  <a href="https://www.algolia.com/doc/api-client/troubleshooting/faq/csharp/" target="_blank">FAQ</a>  •
  <a href="https://www.algolia.com/support" target="_blank">Support</a>
</p>

## ✨ Features

* Targets .NET Standard:
  * `.NET Standard 1.3` to `.NET Standard 2.1`. 
  * For more details about supported .NET implementations, please see .NET Standard official [page](https://docs.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-2-1).
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

Without any prior configuration, you can start indexing contacts in the contacts index using the following code:

```csharp
public class Contact
{
  public string ObjectID { get; set; }
  public string Name { get; set; }
  public int Age { get; set; }
}

SearchIndex index = client.InitIndex("contacts");

index.SaveObject(new Contact
{
    ObjectID = "ID1",
    Name = "Jimmie",
    Age = 30
});
```

#### Search

You can now search for contacts by `firstname`, `lastname`, `company`, etc. (even with typos):

```csharp

// Synchronous
index.Search<Contact>(new Query { "jimmie" });

// Asynchronous
await index.SearchAsync<Contact>(new Query { "jimmie" });
```

For full documentation, visit the **[Algolia .NET API Client documentation](https://www.algolia.com/doc/api-client/getting-started/install/csharp/)**.

#### ASP.NET
If you're using ASP.NET, checkout the [following tutorial](https://www.algolia.com/doc/api-client/getting-started/tutorials/asp.net/csharp/).

## ❓ Troubleshooting

Encountering an issue? Before reaching out to support, we recommend heading to our [FAQ](https://www.algolia.com/doc/api-client/troubleshooting/faq/csharp/) where you will find answers for the most common issues and gotchas with the client.


## Use the Dockerfile

If you want to contribute to this project without installing all its dependencies, you can use our Docker image. Please check our [dedicated guide](DOCKER_README.md) to learn more.

## 📄 License
Algolia .NET API Client is an open-sourced software licensed under the [MIT license](LICENSE.md).
