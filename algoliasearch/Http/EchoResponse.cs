using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Algolia.Search.Http
{
  public class EchoResponse
  {

    public String Path;
    public String Host;
    public HttpMethod Method;
    public String Body;
    public Dictionary<string, string> QueryParameters;
    public Dictionary<string, string> Headers;
    public int ConnectTimeout;
    public int ResponseTimeout;
  }
}
