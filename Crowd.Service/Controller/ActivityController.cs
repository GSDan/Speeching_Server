using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Crowd.Model;
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
            using (CrowdContext db = new CrowdContext())
            {
                var acts = db.ParticipantActivities.ToList();
                return new HttpResponseMessage()
                {
                    Content = new JsonContent(acts)
                };
            }
        }

        // GET api/Activity/5
        public HttpResponseMessage Get(int id)
        {
            if (id <= 0)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

            using (CrowdContext db = new CrowdContext())
            {
                var activity = db.ParticipantActivities.Find(id);
                if (activity == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
                }
                return new HttpResponseMessage()
                {
                    Content = new JsonContent(activity)
                };
            }
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

            using (CrowdContext db = new CrowdContext())
            {
                db.ParticipantActivities.Attach(activity);
                db.Entry(activity).State = EntityState.Modified;
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

        // POST api/Activity
        public HttpResponseMessage Post(ParticipantActivity activity)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            using (CrowdContext db = new CrowdContext())
            {
                db.ParticipantActivities.Add(activity);
                db.SaveChanges();
                var response = Request.CreateResponse(HttpStatusCode.Created, activity);
                return response;
            }
        }

        // DELETE api/Activity/5
        public HttpResponseMessage Delete(int id)
        {
            if (id <= 0)
                return Request.CreateResponse(HttpStatusCode.NotFound, "id must be greater than zero");

            using (CrowdContext db = new CrowdContext())
            {
                var activity = db.ParticipantActivities.Find(id);
                if (activity == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                db.ParticipantActivities.Remove(activity);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
                }
                return Request.CreateResponse(HttpStatusCode.OK, activity);
            }
        }
    }
}
