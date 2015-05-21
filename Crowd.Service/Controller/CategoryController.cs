using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Crowd.Model.Data;
using Crowd.Model.Interface;
using Crowd.Service.Interface;
using Crowd.Service.Model;

namespace Crowd.Service.Controller
{
    public class CategoryController : BaseController, ICategoryController
    {
        // GET api/Category
        public async Task<HttpResponseMessage> Get()
        {
            AuthenticationModel auth = GetAuthentication();
            if (!await AuthenticateUser(auth))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            var cats = DB.ParticipantActivityCategories.ToList();

            return new HttpResponseMessage()
            {
                Content = new JsonContent(CategoryModel.Convert(cats))
            };

        }

        // GET api/Category/5
        public async Task<HttpResponseMessage> Get(int id)
        {
            AuthenticationModel auth = GetAuthentication();
            if (!await AuthenticateUser(auth))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            if (id <= 0)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

            var category = DB.ParticipantActivityCategories.Find(id);
            if (category == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
            }
            return new HttpResponseMessage()
            {
                Content = new JsonContent(CategoryModel.Convert(category))
            };
        }

        // PUT api/Category/5
        public async Task<HttpResponseMessage> Put(ParticipantActivityCategory category)
        {
            AuthenticationModel auth = GetAuthentication();
            if (!await AuthenticateUser(auth))
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
            DB.ParticipantActivityCategories.Attach(category);
            DB.Entry(category).State = EntityState.Modified;
            try
            {
                DB.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Category
        public async Task<HttpResponseMessage> Post(ParticipantActivityCategory category)
        {
            AuthenticationModel auth = GetAuthentication();
            if (!await AuthenticateUser(auth))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            if (ModelState.IsValid)
            {
                DB.ParticipantActivityCategories.Add(category);
                DB.SaveChanges();
                var response = Request.CreateResponse(HttpStatusCode.Created, category);
                response.Headers.Location = new Uri(this.Request.RequestUri.AbsoluteUri + category.Id);//new Uri(Url.Link("DefaultApi", new { id = category.Key }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Category/5
        public async Task<HttpResponseMessage> Delete(int id)
        {
            AuthenticationModel auth = GetAuthentication();
            if (!await AuthenticateUser(auth))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            if (id <= 0)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "id must be greater than zero");
            var category = DB.ParticipantActivityCategories.Find(id);
            if (category == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
            }
            DB.ParticipantActivityCategories.Remove(category);
            try
            {
                DB.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK, category);
        }
    }
}
