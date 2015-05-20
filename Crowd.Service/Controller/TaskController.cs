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
using Crowd.Service.SoundCloud;

namespace Crowd.Service.Controller
{
    public class TaskController : BaseController, ITaskController
    {
        // GET api/task
        public IEnumerable<IParticipantTask> Get()
        {
            return DB.ParticipantTasks.AsEnumerable();
        }

        // GET api/task/5
        public IParticipantTask Get(int id)
        {
            if (id <= 0)
                return new ParticipantTask();

            IParticipantTask task = DB.ParticipantTasks.Single(c => c.Id.Equals(id));
            if (task == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return task;
        }

        // GET api/task/5
        public IEnumerable<IParticipantTask> GetByActivityId(int id)
        {
            if (id <= 0)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));

            var tasks = DB.ParticipantTasks.Where(c => c.ParticipantActivityId.Equals(id));
            if (tasks.Any())
                return tasks;
            else
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
        }

        // PUT api/task/5
        public HttpResponseMessage Put(int id, ParticipantTask task)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            if (id != task.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            DB.ParticipantTasks.Attach(task);
            DB.Entry(task).State = EntityState.Modified;
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

        // POST api/task
        public HttpResponseMessage Post(ParticipantTask task)
        {
            if (ModelState.IsValid)
            {
                DB.ParticipantTasks.Add(task);
                DB.SaveChanges();
                var response = Request.CreateResponse(HttpStatusCode.Created, task);
                //response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = task.Key }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/task/5
        public HttpResponseMessage Delete(int id)
        {
            if (id <= 0)
                return Request.CreateResponse(HttpStatusCode.NotFound, "id must be greater than zero");
            var task = DB.ParticipantTasks.Single(c => c.Id.Equals(id));
            if (task == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            DB.ParticipantTasks.Remove(task);
            try
            {
                DB.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK, task);
        }
    }
}
