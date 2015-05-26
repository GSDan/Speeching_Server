using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Crowd.Model.Data;
using Crowd.Service.Common;
using Crowd.Service.CrowdFlower;
using Crowd.Service.Interface;
using Crowd.Service.Model;
using Newtonsoft.Json;

namespace Crowd.Service.Controller
{
    public class ActivityResultController : BaseController, IActivityResultController
    {
        // GET api/ActivityResult
        public HttpResponseMessage Get()
        {
            var lst = DB.ParticipantResults.ToList();
            return new HttpResponseMessage()
            {
                Content = new JsonContent(lst)
            };
        }

        // GET api/ActivityResult/5
        public HttpResponseMessage Get(int id)
        {
            if (id <= 0)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

            var result = DB.ParticipantResults.Find(id);
            if (result == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return new HttpResponseMessage()
            {
                Content = new JsonContent(result)
            };
        }

        // GET api/task/5
        public HttpResponseMessage GetByActivityId(int id)
        {
            if (id <= 0)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

            var lst = DB.ParticipantResults.Where(c => c.ParticipantActivityId.Equals(id));
            if (lst.Any())
            {
                return new HttpResponseMessage()
                {
                    Content = new JsonContent(lst)
                };
            }
            else
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
        }

        // PUT api/ActivityResult/5
        public HttpResponseMessage Put(int id, ParticipantResult result)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            if (id != result.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            DB.ParticipantResults.Attach(result);
            DB.Entry(result).State = EntityState.Modified;
            try
            {
                DB.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        
        // POST api/ActivityResult
        [RequireHttps]
        public async Task<HttpResponseMessage> Post(ParticipantResult result)
        {
            User user = await AuthenticateUser(GetAuthentication());
            if (user == null)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            if (ModelState.IsValid)
            {
                if (result != null)
                {
                    result.User = user;
                    user.Submissions.Add(result);
                    DB.ParticipantResults.Add(result);
                    try
                    {
                        DB.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed,
                            ex.Message));
                    }
                    SvcStatus status = await CrowdFlowerApi.CreateJob(result);
                    if (status.Level == 0)
                    {
                        string json = await status.Response.Content.ReadAsStringAsync();
                        CFJobResponse jobRes = JsonConvert.DeserializeObject<CFJobResponse>(json);
                        CrowdFlowerApi.UploadUnits(jobRes.id, result.ResourceUrl);
                        CrowdFlowerApi.JobQualitySettings(jobRes.id);

                        result.CrowdJobId = jobRes.id;

                        if (status.CreatedRows != null)
                        {
                            foreach (CrowdRowResponse row in status.CreatedRows)
                            {
                                DB.CrowdRowResponses.Add(row);
                            }
                        }

                        try
                        {
                            DB.SaveChanges();
                        }
                        catch(Exception e)
                        {
                            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message));
                        }

                        ////TODO: Save CF job response to database
                        ////TODO: Launch the job
                        //CrowdFlowerApi.LaunchJob(jobRes.id);
                        return status.Response;
                    }
                    else
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, status.Description);
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Activity result cannot be null");
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/ActivityResult/5
        public HttpResponseMessage Delete(int id)
        {
            if (id <= 0)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "id must be greater than zero");
            var result = DB.ParticipantResults.Find(id);
            if (result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            DB.ParticipantResults.Remove(result);
            try
            {
                DB.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}