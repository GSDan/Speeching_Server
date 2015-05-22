using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Crowd.Service.Controller
{
    public class AssessmentController : BaseController
    {
        public async Task<HttpResponseMessage> Get()
        {
            var ordered = await (from assessment in DB.ParticipantAssessments
                orderby assessment.DateSet descending
                select assessment).ToArrayAsync();

            return new HttpResponseMessage()
            {
                Content = new JsonContent(ordered)
            };
        }
    }
}
