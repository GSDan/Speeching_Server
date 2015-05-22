using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Crowd.Model.Data;

namespace Crowd.Service.Controller
{
    public class AssessmentController : BaseController
    {
        public async Task<HttpResponseMessage> Get()
        {
            User user = await AuthenticateUser(GetAuthentication());
            if (user == null)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            var ordered = await (from assessment in DB.ParticipantAssessments
                orderby assessment.DateSet descending
                select assessment).ToArrayAsync();

            return new HttpResponseMessage
            {
                Content = new JsonContent(ordered)
            };
        }

        public async Task<HttpResponseMessage> Post()
        {
            User user = await AuthenticateUser(GetAuthentication());
            if (user == null)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}