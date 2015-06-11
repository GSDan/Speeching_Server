using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Crowd.Model;
using Crowd.Model.Data;

namespace Crowd.Service.Controller
{
    public class HelpController : BaseController
    {

        public async Task<HttpResponseMessage> Get(string activityType)
        {
            using (CrowdContext db = new CrowdContext())
            {
                User user = await AuthenticateUser(GetAuthentication(), db);
                if (user == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                ParticipantAssessmentTask.AssessmentTaskType type;

                Enum.TryParse(activityType, true, out type);

                ActivityHelper helper = await db.ActivityHelpers.FindAsync(type);

                return new HttpResponseMessage()
                {
                    Content = new JsonContent(helper)
                };
            }
        }
    }
}
