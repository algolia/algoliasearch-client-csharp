using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Algolia.Search.Clients;
using Algolia.Search.Exceptions;
using Algolia.Search.Http;
using Algolia.Search.Transport;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Algolia.Search.Tests;

public class TimeoutIntegrationTests
{
  private static (AlgoliaConfig, StatefulHost) CreateConfigWithHost(string hostUrl)
  {
    var config = new SearchConfig("test-app", "test-key");
    var host = new StatefulHost { Url = hostUrl, Accept = CallType.Read | CallType.Write };
    config.CustomHosts = new List<StatefulHost> { host };
    return (config, host);
  }

  private static StatefulHost CreateServerHost()
  {
    var serverHost =
      Environment.GetEnvironmentVariable("CI") == "true" ? "localhost" : "host.docker.internal";

    return new StatefulHost
    {
      Url = serverHost,
      Port = 6676,
      Scheme = HttpScheme.Http,
      Accept = CallType.Read | CallType.Write,
    };
  }

  [Fact]
  public async Task RetryCountStateful()
  {
    // connect timeout increases across failed requests: 2s -> 4s -> 6s
    var (config, _) = CreateConfigWithHost("10.255.255.1");
    var transport = new HttpTransport(
      config,
      new AlgoliaHttpRequester(NullLoggerFactory.Instance),
      NullLoggerFactory.Instance
    );

    var times = new List<double>();
    for (int i = 0; i < 3; i++)
    {
      var sw = Stopwatch.StartNew();
      try
      {
        await transport.ExecuteRequestAsync(
          HttpMethod.Get,
          "/test",
          new InternalRequestOptions { UseReadTransporter = true }
        );
      }
      catch (Exception)
      {
        sw.Stop();
        times.Add(sw.Elapsed.TotalSeconds);
      }
    }

    // Connect timeout scales: ConnectTimeout (2s default) * (RetryCount+1)
    // Request 1: 2s * 1 = 2s
    // Request 2: 2s * 2 = 4s
    // Request 3: 2s * 3 = 6s
    Assert.True(times[0] > 1.5 && times[0] < 2.5, $"Request 1 should be ~2s, got {times[0]:F2}s");
    Assert.True(times[1] > 3.5 && times[1] < 4.5, $"Request 2 should be ~4s, got {times[1]:F2}s");
    Assert.True(times[2] > 5.5 && times[2] < 7.0, $"Request 3 should be ~6s, got {times[2]:F2}s");
  }

  [Fact]
  public async Task RetryCountResets()
  {
    // retry_count resets to 0 after successful request
    var (config, badHost) = CreateConfigWithHost("10.255.255.1");
    var goodHost = CreateServerHost();
    var transport = new HttpTransport(
      config,
      new AlgoliaHttpRequester(NullLoggerFactory.Instance),
      NullLoggerFactory.Instance
    );

    // fail twice to increment retry_count
    for (int i = 0; i < 2; i++)
    {
      try
      {
        await transport.ExecuteRequestAsync(
          HttpMethod.Get,
          "/test",
          new InternalRequestOptions { UseReadTransporter = true }
        );
      }
      catch (Exception)
      {
        // expected to fail
      }
    }

    // switch to good host and succeed
    config.CustomHosts = new List<StatefulHost> { goodHost };
    goodHost.RetryCount = badHost.RetryCount;
    transport = new HttpTransport(
      config,
      new AlgoliaHttpRequester(NullLoggerFactory.Instance),
      NullLoggerFactory.Instance
    );

    var response = await transport.ExecuteRequestAsync<AlgoliaHttpResponse>(
      HttpMethod.Get,
      "/1/test/instant",
      new InternalRequestOptions { UseReadTransporter = true }
    );

    Assert.Equal(200, response.HttpStatusCode);
    Assert.True(
      goodHost.RetryCount == 0,
      $"retry_count should reset to 0, got {goodHost.RetryCount}"
    );

    // point to bad host again, should timeout at 2s (not 6s)
    goodHost.Url = "10.255.255.1";
    goodHost.Port = null;
    goodHost.Scheme = HttpScheme.Https;

    var sw = Stopwatch.StartNew();
    try
    {
      await transport.ExecuteRequestAsync(
        HttpMethod.Get,
        "/test",
        new InternalRequestOptions { UseReadTransporter = true }
      );
      Assert.Fail("Request should have timed out");
    }
    catch (Exception)
    {
      sw.Stop();
      var elapsed = sw.Elapsed.TotalSeconds;
      Assert.True(elapsed > 1.5 && elapsed < 2.5, $"After reset should be ~2s, got {elapsed:F2}s");
    }
  }
}
