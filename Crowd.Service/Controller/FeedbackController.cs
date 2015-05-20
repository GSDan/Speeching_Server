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
        private async Task<float?> AverageRating(string resDataType, int jobId = -1)
        {
            if (jobId >= 0)
            {
                return (float?)await (from judgement in DB.CrowdJudgements
                                     where judgement.JobId == jobId
                                     from data in judgement.Data
                                     where data.DataType == resDataType
                                     select data.NumResponse).AverageAsync();
            }

            return (float?)await (from judgement in DB.CrowdJudgements
                from data in judgement.Data
                where data.DataType == resDataType
                select data.NumResponse).AverageAsync();
        }

        private async Task<GraphPoint[]> GetGraphPoints(string resDataType)
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

            return points.ToArray();
        }

        //http://localhost:52215/api/Feedback?id=null
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

                List<IFeedbackItem> feedback = new List<IFeedbackItem>();

                float? accentRating = await AverageRating("rlstaccent", submissionId);
                if (accentRating != null)
                {
                    var accentFeedback = new FeedbackItemRating
                    {
                        Rating = (float)accentRating,
                        Title = "Accent Influence",
                        Description = "This rating shows how much your accent affected listeners' understanding of your speech.",
                        Date = DateTime.Now,
                        Dismissable = false,
                        Importance = 10
                    };
                    feedback.Add(accentFeedback);
                }

                float? transRating = await AverageRating("rlsttrans", submissionId);
                if (transRating != null)
                {
                    var transFeedback = new FeedbackItemRating
                    {
                        Rating = (float)transRating,
                        Title = "Difficulty of Understanding",
                        Description = "This rating shows how difficult listeners find understanding what you say.",
                        Date = DateTime.Now,
                        Dismissable = false,
                        Importance = 10
                    };
                    feedback.Add(transFeedback);
                }


                FeedbackItemGraph graphFeedback = new FeedbackItemGraph
                {
                    Title = "Understanding Progress",
                    Description = "How understandable people have found you over time.",
                    Date = DateTime.Now,
                    Dismissable = false,
                    Importance = 8,
                    DataPoints = await GetGraphPoints("rlsttrans")
                };
                feedback.Add(graphFeedback);

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
