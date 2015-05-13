using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using Crowd.Model.Data;
using Crowd.Service.Interface;

namespace Crowd.Service.Controller
{
    public class CFWebhookController : BaseController, ICFWebhookController
    {
        public HttpResponseMessage Get()
        {
            throw new NotImplementedException();
        }

        public HttpResponseMessage Get(int id)
        {
            throw new NotImplementedException();
        }

        public HttpResponseMessage Put()
        {
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage Post()
        {
            var req = this.Request;
            //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            //var jsonSerialized = jsonSerializer.Serialize(req);
            var content = req.Content;
            var contentRs = content.ReadAsStringAsync().Result;
            DB.ScientistTaskResponses.Add(new ScientistTaskResponse() { ExternalId = "x2", ParticipantTaskId = 4, Prompt = contentRs, Type = req.ToString() });
            try
            {
                DB.SaveChanges();
            }
            catch (Exception ex)
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}