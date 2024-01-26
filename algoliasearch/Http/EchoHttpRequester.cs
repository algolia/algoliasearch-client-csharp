using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Algolia.Search.Http
{
  /// <summary>
  /// Http Custom requester, used for testing
  /// </summary>
  public class EchoHttpRequester : IHttpRequester
  {
    /// <summary>
    /// Last response returned by the echo API
    /// </summary>
    public EchoResponse LastResponse;

    private static Dictionary<string, string> SplitQuery(string query)
    {
      var collection = HttpUtility.ParseQueryString(query);
      return collection.AllKeys.ToDictionary(key => key, key => collection[key]);
    }

    /// <summary>
    /// Send a fake request
    /// </summary>
    /// <param name="request"></param>
    /// <param name="requestTimeout"></param>
    /// <param name="connectTimeout"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public Task<AlgoliaHttpResponse> SendRequestAsync(Request request, TimeSpan requestTimeout,
      TimeSpan connectTimeout,
      CancellationToken ct = default)
    {
      var echo = new EchoResponse
      {
        Path = request.Uri.AbsolutePath,
        Host = request.Uri.Host,
        Method = request.Method,
        Body = request.Body,
        QueryParameters = SplitQuery(request.Uri.Query),
        Headers = new Dictionary<string, string>(request.Headers),
        ConnectTimeout = connectTimeout,
        ResponseTimeout = requestTimeout
      };

      LastResponse = echo;

      return Task.FromResult(new AlgoliaHttpResponse
      {
        Body = new MemoryStream(),
        HttpStatusCode = 200
      });
    }
  }
}
