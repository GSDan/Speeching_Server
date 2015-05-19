using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Crowd.Model.Data;
using Crowd.Service.Interface;
using Crowd.Service.Model.Feedback;
using Newtonsoft.Json;

namespace Crowd.Service.Controller
{
    public class FeedbackController : BaseController
    {
        private async Task<float> AverageRating(string resDataType, int jobId = -1)
        {
            if (jobId >= 0)
            {
                return (float)await (from judgement in DB.CrowdJudgements
                                     where judgement.JobId == jobId
                                     from data in judgement.Data
                                     where data.DataType == resDataType
                                     select data.NumResponse).AverageAsync();
            }

            return (float)await (from judgement in DB.CrowdJudgements
                from data in judgement.Data
                where data.DataType == resDataType
                select data.NumResponse).AverageAsync();
        }

        private async Task<List<GraphPoint>> GetGraphPoints(string resDataType)
        {
            CrowdJudgement[] ordered = await (from judgement in DB.CrowdJudgements
                orderby judgement.CreatedAt
                select judgement).ToArrayAsync();

            List<GraphPoint> points = new List<GraphPoint>();

            foreach (var judgement in ordered)
            {
                double yVal = (from data in judgement.Data
                    where data.DataType == resDataType
                    select data.NumResponse).Average();

                points.Add(new GraphPoint
                {
                    XVal = judgement.CreatedAt,
                    YVal = yVal
                });
            }

            return points;
        }

        public async Task<HttpResponseMessage> Get(int? id)
        {
            try
            {
                int submissionId = id ?? -1;
                if (submissionId >= 0)
                {
                    ParticipantResult res = await DB.ParticipantResults.FindAsync(id);
                    if (res != null)
                    {
                        submissionId = res.CrowdJobId;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
                    }
                }

                FeedbackItemRating accentFeedback = new FeedbackItemRating
                {
                    Rating = await AverageRating("rlstaccent", submissionId),
                    Title = "Accent Influence",
                    Description = "This rating shows how much your accent affected listeners' understanding of your speech.",
                    Date = DateTime.Now,
                    Dismissable = false,
                    Importance = 10
                };

                FeedbackItemRating transFeedback = new FeedbackItemRating
                {
                    Rating = await AverageRating("rlsttrans", submissionId),
                    Title = "Difficulty of Understanding",
                    Description = "This rating shows how difficult listeners find understanding what you say.",
                    Date = DateTime.Now,
                    Dismissable = false,
                    Importance = 10
                };

                IFeedbackItem[] feedback = { transFeedback, accentFeedback };

                return new HttpResponseMessage
                {
                    Content = new JsonContent(JsonConvert.SerializeObject(feedback))
                };
            }
            catch (Exception)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
        }
    }
}
