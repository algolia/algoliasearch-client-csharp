using System.IO;
using System.IO.Compression;
using System.Text;
using Algolia.Search.Models.Common;

namespace Algolia.Search.Transport;

internal static class Compression
{
  private static readonly UTF8Encoding DefaultEncoding = new(false);

  private const int DefaultBufferSize = 1024;

  // Buffer sized as recommended by Bradley Grainger, http://faithlife.codes/blog/2012/06/always-wrap-gzipstream-with-bufferedstream/
  private const int GZipBufferSize = 8192;

  public static MemoryStream CreateStream(string data, bool compress)
  {
    var stream = new MemoryStream();

    var compressionType = compress ? CompressionType.Gzip : CompressionType.None;
    if (compressionType == CompressionType.Gzip)
    {
      using var gzipStream = new GZipStream(stream, CompressionMode.Compress, true);
      using var sw = new StreamWriter(gzipStream, DefaultEncoding, GZipBufferSize);
      sw.Write(data);
      sw.Flush();
    }
    else
    {
      using var sw = new StreamWriter(stream, DefaultEncoding, DefaultBufferSize, true);
      sw.Write(data);
      sw.Flush();
    }

    stream.Seek(0, SeekOrigin.Begin);
    return stream;
  }
}
