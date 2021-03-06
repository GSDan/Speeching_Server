﻿using System;
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
using Crowd.Model.Interface;
using Crowd.Service.Interface;
using Crowd.Service.Model;
using Newtonsoft.Json;

namespace Crowd.Service.Controller
{
    public class CategoryController : BaseController, ICategoryController
    {
        // GET api/Category
        public async Task<HttpResponseMessage> Get()
        {
            using (CrowdContext db = new CrowdContext())
            {
                User user = await AuthenticateUser(GetAuthentication(), db);
                if (user == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                var defaultCats = await (from category in db.ParticipantActivityCategories
                    where category.DefaultSubscription &&
                    (category.App == Crowd.Model.Data.User.AppType.None || category.App == user.App)
                    select category).ToArrayAsync();

                bool changed = false;
                foreach (ParticipantActivityCategory category in defaultCats)
                {
                    if (user.SubscribedCategories.Contains(category)) continue;
                    user.SubscribedCategories.Add(category);
                    changed = true;
                }

                if (changed)
                {
                    await db.SaveChangesAsync();
                }

                for (int i = 0; i < user.SubscribedCategories.Count; i++)
                {
                    user.SubscribedCategories.ElementAt(i).Activities =
                        (from act in user.SubscribedCategories.ElementAt(i).Activities
                            where act.AppType == user.App || act.AppType == Crowd.Model.Data.User.AppType.None
                            select act).ToList();
                }

                return new HttpResponseMessage()
                {
                    Content = new JsonContent(user.SubscribedCategories)
                };
                
            }
        }

        // GET api/Category/5
        public async Task<HttpResponseMessage> Get(int id)
        {
            using (CrowdContext db = new CrowdContext())
            {
                User user = await AuthenticateUser(GetAuthentication(), db);
                if (user == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                if (id <= 0)
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

                var category = db.ParticipantActivityCategories.Find(id);
                if (category == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                return new HttpResponseMessage()
                {
                    Content = new JsonContent(category)
                };
            }
        }

        // PUT api/Category/5
        public async Task<HttpResponseMessage> Put(ParticipantActivityCategory category)
        {
            using (CrowdContext db = new CrowdContext())
            {
                User user = await AuthenticateUser(GetAuthentication(), db);
                if (user == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                if (category.Id <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                db.ParticipantActivityCategories.Attach(category);
                db.Entry(category).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }

        // POST api/Category
        public async Task<HttpResponseMessage> Post(ParticipantActivityCategory category)
        {
            using (CrowdContext db = new CrowdContext())
            {
                User user = await AuthenticateUser(GetAuthentication(), db);
                if (user == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                if (!ModelState.IsValid) return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

                db.ParticipantActivityCategories.Add(category);
                db.SaveChanges();
                var response = Request.CreateResponse(HttpStatusCode.Created, category);
                response.Headers.Location = new Uri(Request.RequestUri.AbsoluteUri + category.Id);//new Uri(Url.Link("DefaultApi", new { id = category.Key }));
                return response;
            }
        }

        // DELETE api/Category/5
        public async Task<HttpResponseMessage> Delete(int id)
        {
            using (CrowdContext db = new CrowdContext())
            {
                User user = await AuthenticateUser(GetAuthentication(), db);
                if (user == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                if (id <= 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "id must be greater than zero");
                var category = db.ParticipantActivityCategories.Find(id);
                if (category == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                db.ParticipantActivityCategories.Remove(category);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
                return Request.CreateResponse(HttpStatusCode.OK, category);
            }
            
        }
    }
}
