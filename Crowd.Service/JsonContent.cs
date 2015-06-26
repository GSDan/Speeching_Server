using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Crowd.Service.Common;
using Newtonsoft.Json;

namespace Crowd.Service
{
    public class JsonContent : HttpContent
    {
        private readonly Stream jsonStream;

        public JsonContent(object value)
        {
            var json = JsonConvert.SerializeObject(
                value,
                Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            jsonStream = SvcUtil.GenerateStreamFromString(json);
            Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return jsonStream.CopyToAsync(stream);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = jsonStream.Length;
            return true;
        }
    }
}