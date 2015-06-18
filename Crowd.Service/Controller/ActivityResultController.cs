using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Crowd.Model;
using Crowd.Model.Data;
using Crowd.Service.Common;
using Crowd.Service.CrowdFlower;
using Crowd.Service.Interface;
using Newtonsoft.Json;

namespace Crowd.Service.Controller
{
    public class ActivityResultController : BaseController, IActivityResultController
    {
        // GET api/ActivityResult
        public async Task<HttpResponseMessage> Get()
        {
            using (CrowdContext db = new CrowdContext())
            {
                User user = await AuthenticateUser(GetAuthentication(), db);
                if (user == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                List<ParticipantResult> lst = await (
                    from result in db.ParticipantResults
                    where result.User == user
                    orderby result.Id
                    select result).ToListAsync();

                return new HttpResponseMessage
                {
                    Content = new JsonContent(lst)
                };
            }
        }

        // GET api/ActivityResult/5
        public async Task<HttpResponseMessage> Get(int id)
        {
            if (id <= 0)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

            using (CrowdContext db = new CrowdContext())
            {
                User user = await AuthenticateUser(GetAuthentication(), db);
                if (user == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                var result = db.ParticipantResults.Find(id);
                if (result == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
                }

                if (user != result.User)
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                return new HttpResponseMessage
                {
                    Content = new JsonContent(result)
                };
            }
        }

        // GET api/task/5
        public async Task<HttpResponseMessage> GetByActivityId(int id)
        {
            if (id <= 0)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

            using (CrowdContext db = new CrowdContext())
            {
                User user = await AuthenticateUser(GetAuthentication(), db);
                if (user == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                var linqResults = from result in db.ParticipantResults
                    where result.ParticipantActivityId == id && result.User == user
                    orderby result.Id
                    select result;

                if (linqResults.Any())
                {
                    return new HttpResponseMessage
                    {
                        Content = new JsonContent(linqResults)
                    };
                }

                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
        }

        // PUT api/ActivityResult/5
        public async Task<HttpResponseMessage> Put(int id, ParticipantResult result)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            if (id != result.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            using (CrowdContext db = new CrowdContext())
            {
                User user = await AuthenticateUser(GetAuthentication(), db);
                if (user == null || !user.IsAdmin)
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                db.ParticipantResults.Attach(result);
                db.Entry(result).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }

        // POST api/ActivityResult
        [RequireHttps]
        public async Task<HttpResponseMessage> Post(ParticipantResult result)
        {
            using (CrowdContext db = new CrowdContext())
            {
                User user = await AuthenticateUser(GetAuthentication(), db);
                if (user == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                if (!ModelState.IsValid) return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                if (result == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Activity result cannot be null");

                result.User = user;
                result = db.ParticipantResults.Add(result);

                user.Submissions.Add(result);

                if (result.IsAssessment)
                {
                    user.LastAssessment = result;
                }

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
                }

                SvcStatus status = await CrowdFlowerApi.CreateJob(result,
                    await db.ParticipantActivities.FindAsync(result.ParticipantActivityId));

                if (status.Level == 0)
                {
                    string json = await status.Response.Content.ReadAsStringAsync();
                    CFJobResponse jobRes = JsonConvert.DeserializeObject<CFJobResponse>(json);

                    result.CrowdJobId = jobRes.id;

                    if (status.CreatedRows != null)
                    {
                        foreach (CrowdRowResponse row in status.CreatedRows)
                        {
                            db.CrowdRowResponses.Add(row);
                        }
                    }

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        throw new HttpResponseException(Request.CreateResponse(
                            HttpStatusCode.ExpectationFailed, e.Message));
                    }

                    CrowdFlowerApi.LaunchJob(jobRes.id);
                    return status.Response;
                }
                db.DebugMessages.Add(new DebugMessage
                {
                    Message = status.Description,
                    Filename = "ActivityResultController",
                    FunctionName = "Post"
                });
                await db.SaveChangesAsync();
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, status.Description);
            }
        }

        // DELETE api/ActivityResult/5
        public async Task<HttpResponseMessage> Delete(int id)
        {
            if (id <= 0)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "id must be greater than zero");

            using (CrowdContext db = new CrowdContext())
            {
                User user = await AuthenticateUser(GetAuthentication(), db);
                if (user == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                var result = db.ParticipantResults.Find(id);
                if (result == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                if (result.User != user && !user.IsAdmin)
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                db.ParticipantResults.Remove(result);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
                }
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
        }
    }
}