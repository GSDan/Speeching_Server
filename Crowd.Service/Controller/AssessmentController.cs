using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Crowd.Model;
using Crowd.Model.Data;

namespace Crowd.Service.Controller
{
    public class AssessmentController : BaseController
    {
        public async Task<HttpResponseMessage> Get()
        {
            using (CrowdContext db = new CrowdContext())
            {
                User user = await AuthenticateUser(GetAuthentication(), db);
                if (user == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                var ordered = await (from assessment in db.ParticipantActivities
                                     where assessment.AssessmentTasks.Count > 0
                                     orderby assessment.DateSet descending
                                     select assessment).ToArrayAsync();

                return new HttpResponseMessage
                {
                    Content = new JsonContent(ordered)
                };
            }
        }

        public async Task<HttpResponseMessage> Post()
        {
            using (CrowdContext db = new CrowdContext())
            {
                User user = await AuthenticateUser(GetAuthentication(), db);
                return user == null ? new HttpResponseMessage(HttpStatusCode.Unauthorized) 
                                    : new HttpResponseMessage(HttpStatusCode.OK);
            }
        }
    }
}