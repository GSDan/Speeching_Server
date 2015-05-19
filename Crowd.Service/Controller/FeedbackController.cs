using System.Data.Entity;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Crowd.Service.Controller
{
    public class FeedbackController : BaseController
    {
        public async Task<HttpResponseMessage> Get()
        {
            var acts = await DB.CrowdJudgements.ToListAsync();
            return new HttpResponseMessage
            {
                Content = new JsonContent(JsonConvert.SerializeObject(acts))
            };
        }
    }
}
