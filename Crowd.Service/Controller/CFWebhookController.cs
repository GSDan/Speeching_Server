using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using Crowd.Model.Data;
using Crowd.Service.Interface;
using Crowd.Service.CrowdFlower;
using Newtonsoft.Json;
using System.Web;
using Crowd.Model;

namespace Crowd.Service.Controller
{
    public class CFWebhookController : BaseController, ICFWebhookController
    {
        public async Task<HttpResponseMessage> Get()
        {
            return await Post(); // TODO
        }

        public HttpResponseMessage Get(int id)
        {
            throw new NotImplementedException();
        }

        public HttpResponseMessage Put()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // Should always return OK - crowdflower can't really do anything with errors. Log any errors in the database.
        public async Task<HttpResponseMessage> Post()
        {
            var req = this.Request;
            string content = "Not done yet";
            IEnumerable<string> contentTypes = new[]{""};

            try
            {
                Request.Headers.TryGetValues("Content-Type", out contentTypes);

                content = (await req.Content.ReadAsFormDataAsync())["payload"];

                CFResponseData cfData = JsonConvert.DeserializeObject<CFResponseData>(content);

                if (cfData == null || cfData.results == null || cfData.results.judgments.Count <= 0)
                    return Request.CreateResponse(HttpStatusCode.OK);

                using (CrowdContext db = new CrowdContext())
                {
                    int taskId = cfData.results.judgments[0].unit_id;
                    CrowdRowResponse resp = await db.CrowdRowResponses.FindAsync(taskId.ToString());

                    resp.TaskType = cfData.data.TaskType;
                    resp.Choices = cfData.data.Choices;
                    resp.PrevLoud = cfData.data.PrevLoud;
                    resp.PrevPace = cfData.data.PrevPace;
                    resp.PrevPitch = cfData.data.PrevPitch;
                    resp.Comparison = cfData.data.Comparison;
                    resp.ExtraData = cfData.data.ExtraData;

                    if (resp.TaskJudgements == null) resp.TaskJudgements = new List<CrowdJudgement>();

                    foreach (Judgement judgement in cfData.results.judgments)
                    {
                        if (resp.TaskJudgements.Any(judge =>
                            judge.WorkerId == judgement.worker_id &&
                            judge.CrowdRowResponseId == resp.Id
                            )) continue;

                        CrowdJudgement cJudgement = new CrowdJudgement
                        {
                            Id = judgement.id,
                            City = judgement.city,
                            Country = judgement.country,
                            JobId = judgement.job_id,
                            CrowdRowResponseId = resp.Id,
                            Tainted = judgement.tainted,
                            Trust = judgement.trust,
                            WorkerId = judgement.worker_id,
                            CreatedAt = DateTime.Now
                        };
                        resp.TaskJudgements.Add(cJudgement);
                        db.CrowdJudgements.Add(cJudgement);

                        await cJudgement.AddData(judgement.data, db);
                    }

                    db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                using (CrowdContext db = new CrowdContext())
                {
                    db.DebugMessages.Add(new DebugMessage
                    {
                        Message = ex + "\nSENT CONTENT: " + content +", content-type: " + contentTypes,
                        Filename = "CFWebhookController",
                        FunctionName = "Post"
                    });
                    db.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK); 
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}