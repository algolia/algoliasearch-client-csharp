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

    public async Task<AlgoliaHttpResponse> SendRequestAsync(Request request, TimeSpan totalTimeout,
      CancellationToken ct = default)
    {
      EchoResponse echo = new EchoResponse();
      echo.Path = request.Uri.AbsolutePath;
      echo.Host = request.Uri.Host;
      echo.Method = request.Method;
      echo.Body = request.Body;
      echo.QueryParameters = SplitQuery(request.Uri.Query);
      echo.Headers = new Dictionary<string, string>(request.Headers);

      LastResponse = echo;

      return new AlgoliaHttpResponse
      {
        Body = new MemoryStream(),
        HttpStatusCode = 200
      };
    }
  }
}
