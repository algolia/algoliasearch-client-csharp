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
    public EchoResponse LastResponse;

    private static Dictionary<string, string> SplitQuery(string query)
    {
      var collection = HttpUtility.ParseQueryString(query);
      return collection.AllKeys.ToDictionary(key => key, key => collection[key]);
    }

    public async Task<AlgoliaHttpResponse> SendRequestAsync(Request request, TimeSpan requestTimeout,
      TimeSpan connectTimeout,
      CancellationToken ct = default)
    {
      EchoResponse echo = new EchoResponse
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

      return new AlgoliaHttpResponse
      {
        Body = new MemoryStream(),
        HttpStatusCode = 200
      };
    }
  }
}
