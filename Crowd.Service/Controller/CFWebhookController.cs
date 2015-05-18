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

            const string jsonData = "{\r\n    \"signature\": \"645d58c9e38fc4d7b00f215ce4a72b218cc4b52b\",\r\n    \"payload\": \"{\\\"id\\\":698113669,\\\"data\\\":{\\\"AudioUrl\\\":\\\"http://api.opescode.com/Uploads/710069/4.mp4\\\",\\\"AudioTypeCodec\\\":\\\"audio/mp4; codec='mp4a.40.2'\\\"},\\\"difficulty\\\":0,\\\"judgments_count\\\":3,\\\"state\\\":\\\"finalized\\\",\\\"agreement\\\":0.444444444444444,\\\"missed_count\\\":0,\\\"gold_pool\\\":null,\\\"created_at\\\":\\\"2015-04-02T15:13:54+00:00\\\",\\\"updated_at\\\":\\\"2015-04-02T15:54:26+00:00\\\",\\\"job_id\\\":710069,\\\"results\\\":{\\\"judgments\\\":[{\\\"id\\\":1604431321,\\\"created_at\\\":\\\"2015-04-02T15:17:33+00:00\\\",\\\"started_at\\\":\\\"2015-04-02T15:16:14+00:00\\\",\\\"acknowledged_at\\\":null,\\\"external_type\\\":\\\"clixsense\\\",\\\"golden\\\":false,\\\"missed\\\":null,\\\"rejected\\\":null,\\\"tainted\\\":false,\\\"country\\\":\\\"TUR\\\",\\\"region\\\":\\\"34\\\",\\\"city\\\":\\\"Istanbul\\\",\\\"job_id\\\":710069,\\\"unit_id\\\":698113669,\\\"worker_id\\\":29230631,\\\"trust\\\":1.0,\\\"worker_trust\\\":0.915,\\\"unit_state\\\":\\\"finalized\\\",\\\"data\\\":{\\\"txta\\\":\\\"hello, can i order a pizza please?\\\",\\\"rlsttrans\\\":\\\"3\\\",\\\"rlstaccent\\\":\\\"3\\\"},\\\"unit_data\\\":{\\\"AudioUrl\\\":\\\"http://api.opescode.com/Uploads/710069/4.mp4\\\",\\\"AudioTypeCodec\\\":\\\"audio/mp4; codec='mp4a.40.2'\\\"}},{\\\"id\\\":1604431382,\\\"created_at\\\":\\\"2015-04-02T15:18:02+00:00\\\",\\\"started_at\\\":\\\"2015-04-02T15:16:14+00:00\\\",\\\"acknowledged_at\\\":null,\\\"external_type\\\":\\\"clixsense\\\",\\\"golden\\\":false,\\\"missed\\\":null,\\\"rejected\\\":null,\\\"tainted\\\":false,\\\"country\\\":\\\"PRT\\\",\\\"region\\\":\\\"\\\",\\\"city\\\":\\\"\\\",\\\"job_id\\\":710069,\\\"unit_id\\\":698113669,\\\"worker_id\\\":23097572,\\\"trust\\\":0.5,\\\"worker_trust\\\":0.923636363636364,\\\"unit_state\\\":\\\"finalized\\\",\\\"data\\\":{\\\"txta\\\":\\\"Hello, can i order a pizza please?\\\",\\\"rlsttrans\\\":\\\"5 Very easy\\\",\\\"rlstaccent\\\":\\\"1 Not at all\\\"},\\\"unit_data\\\":{\\\"AudioUrl\\\":\\\"http://api.opescode.com/Uploads/710069/4.mp4\\\",\\\"AudioTypeCodec\\\":\\\"audio/mp4; codec='mp4a.40.2'\\\"}},{\\\"id\\\":1604449343,\\\"created_at\\\":\\\"2015-04-02T15:54:15+00:00\\\",\\\"started_at\\\":\\\"2015-04-02T15:46:15+00:00\\\",\\\"acknowledged_at\\\":null,\\\"external_type\\\":\\\"neodev\\\",\\\"golden\\\":false,\\\"missed\\\":null,\\\"rejected\\\":null,\\\"tainted\\\":false,\\\"country\\\":\\\"VEN\\\",\\\"region\\\":\\\"\\\",\\\"city\\\":\\\"\\\",\\\"job_id\\\":710069,\\\"unit_id\\\":698113669,\\\"worker_id\\\":32002644,\\\"trust\\\":0.5,\\\"worker_trust\\\":0.9,\\\"unit_state\\\":\\\"finalized\\\",\\\"data\\\":{\\\"txta\\\":\\\"Hello can i order a pizza please?\\\",\\\"rlsttrans\\\":\\\"5 Very easy\\\",\\\"rlstaccent\\\":\\\"5 Very much\\\"},\\\"unit_data\\\":{\\\"AudioUrl\\\":\\\"http://api.opescode.com/Uploads/710069/4.mp4\\\",\\\"AudioTypeCodec\\\":\\\"audio/mp4; codec='mp4a.40.2'\\\"}}],\\\"rlsttrans\\\":{\\\"agg\\\":null,\\\"confidence\\\":0},\\\"rlstaccent\\\":{\\\"agg\\\":null,\\\"confidence\\\":0}}}\",\r\n    \"signal\": \"unit_complete\"\r\n}";
            
            CFWebhook cfData = JsonConvert.DeserializeObject<CFWebhook>(jsonData);
            ParticipantResult upload = DB.ParticipantResults.FirstOrDefault(sub => sub.CrowdJobId == cfData.PayloadData.job_id);
            //ParticipantTaskResponse task = DB.ParticipantTaskResponses
            List<CrowdRowResponse> responses = null;

            if(cfData != null && cfData.PayloadData.results != null && upload != null)
            {
                ParticipantTask originTask = DB.ParticipantTasks.FirstOrDefault(origTask => upload.ParticipantTaskIdResults.ContainsKey(origTask.Id));
                if(originTask != null)
                {
                    //responses = cfData.GetResponses(originTask);
                }
            }
            
            if(responses != null)
            {
                foreach (CrowdRowResponse newResp in responses)
                    DB.CrowdRowResponses.Add(newResp);

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