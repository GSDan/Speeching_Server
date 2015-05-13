using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Crowd.Service.Common;
using Crowd.Service.CrowdFlower;
using Crowd.Service.Interface;
using Crowd.Service.Model;

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
                Content = new JsonContent(ActivityResultModel.Convert(lst))
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
                Content = new JsonContent(ActivityResultModel.Convert(result))
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
                    Content = new JsonContent(ActivityResultModel.Convert(lst))
                };
            }
            else
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
        }

        // PUT api/ActivityResult/5
        public HttpResponseMessage Put(int id, ActivityResultModel result)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            if (id != result.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            DB.ParticipantResults.Attach(ActivityResultModel.ConvertToEntity(result));
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
        public HttpResponseMessage Post(ActivityResultModel result)
        {
            if (ModelState.IsValid)
            {
                if (result != null)
                {
                    var parRes = ActivityResultModel.ConvertToEntity(result);
                    DB.ParticipantResults.Add(parRes);
                    try
                    {
                        DB.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.ExpectationFailed,
                            ex.Message));
                    }
                    SvcStatus status = CrowdFlowerApi.CreateJob(parRes);
                    if (status.Level == 0)
                    {
                        //var jobRes = status.Response.Content.ReadAsAsync<CFJobResponse>().Result;
                        //CrowdFlowerApi.UploadUnits(jobRes.id, parRes.ResourceUrl);
                        //CrowdFlowerApi.JobQualitySettings(jobRes.id);
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

        public HttpResponseMessage PutExternalAccessKey(string key)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            ActivityResultModel arm = new ActivityResultModel()
            {
                Id = 4,
                ParticipantActivityId = 1,
                ResourceUrl = "https://di.ncl.ac.uk/owncloud/remote.php/webdav/uploads/7041992/1426180356968.87_1.zip",
                ExternalAccessKey = Request.RequestUri.AbsoluteUri
            };
            DB.ParticipantResults.Attach(ActivityResultModel.ConvertToEntity(arm));
            DB.Entry(arm).State = EntityState.Modified;
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
    }
}