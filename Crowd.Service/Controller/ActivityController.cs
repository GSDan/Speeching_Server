using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Crowd.Model.Data;
using Crowd.Model.Interface;
using Crowd.Service.Interface;
using Crowd.Service.Model;

namespace Crowd.Service.Controller
{
    public class ActivityController : BaseController, IActivityController
    {
        // GET api/Activity
        public HttpResponseMessage Get()
        {
            var acts = DB.ParticipantActivities.ToList();
            return new HttpResponseMessage()
            {
                Content = new JsonContent(ActivityModel.Convert(acts))
            };
        }
        //public IEnumerable<ActivityModel> Get()
        //{
        //    var acts = DB.ParticipantActivities.ToList();
        //    ActivityModel am = new ActivityModel();
        //    //return am.Convert(acts);
        //    return new List<ActivityModel>() { new ActivityModel() { Id = 3}, new ActivityModel() { Id = 4 } };
        //    //System.Web.Script.Serialization.JavaScriptSerializer sr = new System.Web.Script.Serialization.JavaScriptSerializer();
        //    //var x = sr.Serialize(acts);
        //}

        // GET api/Activity/5
        public HttpResponseMessage Get(int id)
        {
            if (id <= 0)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

            var activity = DB.ParticipantActivities.Find(id);
            if (activity == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return new HttpResponseMessage()
            {
                Content = new JsonContent(ActivityModel.Convert(activity))
            };
        }

        // GET api/task/5
        public IEnumerable<IParticipantActivity> GetByCategoryId(int id)
        {
            if (id <= 0)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));

            var acts = DB.ParticipantActivities.Where(c => c.CrowdCategoryId.Equals(id));
            if (acts.Any())
                return acts;
            else
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
        }

        // PUT api/Activity/5
        public HttpResponseMessage Put(int id, ParticipantActivity activity)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            if (id != activity.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            DB.ParticipantActivities.Attach(activity);
            DB.Entry(activity).State = EntityState.Modified;
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

        // POST api/Activity
        public HttpResponseMessage Post(ParticipantActivity activity)
        {
            if (ModelState.IsValid)
            {
                DB.ParticipantActivities.Add(activity);
                DB.SaveChanges();
                var response = Request.CreateResponse(HttpStatusCode.Created, activity);
                //response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = activity.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Activity/5
        public HttpResponseMessage Delete(int id)
        {
            if (id <= 0)
                return Request.CreateResponse(HttpStatusCode.NotFound, "id must be greater than zero");
            var activity = DB.ParticipantActivities.Find(id);
            if (activity == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            DB.ParticipantActivities.Remove(activity);
            try
            {
                DB.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK, activity);
        }
    }
}
