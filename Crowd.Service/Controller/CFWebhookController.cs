using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using Crowd.Model.Data;
using Crowd.Service.Interface;
using Crowd.Service.CrowdFlower;
using Newtonsoft.Json;
using System.Web;

namespace Crowd.Service.Controller
{
    public class CFWebhookController : BaseController, ICFWebhookController
    {
        public HttpResponseMessage Get()
        {
            return this.Post(); // TODO
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
            // TEMP for local testing TODO
            //var req = this.Request;
            //var content = req.Content;
            //string realData = HttpUtility.UrlDecode(req.Content.ReadAsStringAsync().Result);

            string jsonData = "{\"id\":721239936,\"data\":{\"AudioUrl\":\"http://api.opescode.com/Uploads/726084/4.mp4\",\"AudioTypeCodec\":\"audio/mp4; codec='mp4a.40.2'\"},\"difficulty\":0,\"judgments_count\":3,\"state\":\"finalized\",\"agreement\":0.444444444444444,\"missed_count\":0,\"gold_pool\":null,\"created_at\":\"2015-05-13T15:35:35+00:00\",\"updated_at\":\"2015-05-13T17:40:37+00:00\",\"job_id\":726084,\"results\":{\"judgments\":[{\"id\":1635812670,\"created_at\":\"2015-05-13T15:39:14+00:00\",\"started_at\":\"2015-05-13T15:35:52+00:00\",\"acknowledged_at\":null,\"external_type\":\"neodev\",\"golden\":false,\"missed\":null,\"rejected\":null,\"tainted\":false,\"country\":\"BRA\",\"region\":\"27\",\"city\":\"Ita\u00ED\",\"job_id\":726084,\"unit_id\":721239936,\"worker_id\":32598013,\"trust\":1.0,\"worker_trust\":0.754761904761905,\"unit_state\":\"finalized\",\"data\":{\"txta\":\"hello can't your pizza place\",\"rlsttrans\":\"2\",\"rlstaccent\":\"4\"},\"unit_data\":{\"AudioUrl\":\"http://api.opescode.com/Uploads/726084/4.mp4\",\"AudioTypeCodec\":\"audio/mp4; codec='mp4a.40.2'\"}},{\"id\":1635834460,\"created_at\":\"2015-05-13T16:07:00+00:00\",\"started_at\":\"2015-05-13T16:06:00+00:00\",\"acknowledged_at\":null,\"external_type\":\"neodev\",\"golden\":false,\"missed\":null,\"rejected\":null,\"tainted\":false,\"country\":\"DEU\",\"region\":\"\",\"city\":\"\",\"job_id\":726084,\"unit_id\":721239936,\"worker_id\":32296205,\"trust\":0.5,\"worker_trust\":0.875337765957447,\"unit_state\":\"finalized\",\"data\":{\"txta\":\"hello can i\",\"rlsttrans\":\"1 Very difficult\",\"rlstaccent\":\"1 Not at all\"},\"unit_data\":{\"AudioUrl\":\"http://api.opescode.com/Uploads/726084/4.mp4\",\"AudioTypeCodec\":\"audio/mp4; codec='mp4a.40.2'\"}},{\"id\":1635888729,\"created_at\":\"2015-05-13T17:40:20+00:00\",\"started_at\":\"2015-05-13T17:38:53+00:00\",\"acknowledged_at\":null,\"external_type\":\"tremorgames\",\"golden\":false,\"missed\":null,\"rejected\":null,\"tainted\":false,\"country\":\"ARG\",\"region\":\"\",\"city\":\"\",\"job_id\":726084,\"unit_id\":721239936,\"worker_id\":28539034,\"trust\":0.5,\"worker_trust\":0.977777777777778,\"unit_state\":\"finalized\",\"data\":{\"txta\":\"hello i can order pizza please.\",\"rlsttrans\":\"2\",\"rlstaccent\":\"3\"},\"unit_data\":{\"AudioUrl\":\"http://api.opescode.com/Uploads/726084/4.mp4\",\"AudioTypeCodec\":\"audio/mp4; codec='mp4a.40.2'\"}}],\"rlsttrans\":{\"agg\":null,\"confidence\":0},\"rlstaccent\":{\"agg\":null,\"confidence\":0}}}";

            CFWebhook cfData = JsonConvert.DeserializeObject<CFWebhook>(jsonData);
            ParticipantResult upload = DB.ParticipantResults.Where(sub => sub.CrowdJobId == cfData.Payload.job_id).FirstOrDefault();
            
            List<CrowdTaskResponse> responses = null;

            if(cfData != null && cfData.Payload.results != null && upload != null)
            {
                ParticipantTask originTask = DB.ParticipantTasks.Where(origTask => upload.ParticipantTaskIdResults.ContainsKey(origTask.Id)).FirstOrDefault();
                if(originTask != null)
                {
                    responses = cfData.GetResponses(originTask);
                }
            }
            
            if(responses != null)
            {
                foreach (CrowdTaskResponse newResp in responses)
                    DB.CrowdTaskResponses.Add(newResp);

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

            return this.Request.CreateResponse(HttpStatusCode.InternalServerError);            
        }

        public HttpResponseMessage Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}