using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Crowd.Model.Data;
using Newtonsoft.Json;

namespace Crowd.Service.Controller
{
    public class FeedbackController : BaseController
    {
        private async Task<float?> AverageRating(string resDataType, User user, int jobId = -1)
        {
            if (jobId >= 0)
            {
                return (float?) await (from judgement in DB.CrowdJudgements
                    where judgement.JobId == jobId
                    from data in judgement.Data
                    where data.DataType == resDataType
                    select data.NumResponse).AverageAsync();
            }

            var queryRes = from res in DB.ParticipantResults
                where res.User.Email == user.Email
                from judgement in DB.CrowdJudgements
                where res.CrowdJobId == judgement.JobId
                from data in judgement.Data
                where data.DataType == resDataType
                select data.NumResponse;

            return (float?) await queryRes.AverageAsync();
        }

        private async Task<TimeGraphPoint[]> GetGraphPoints(string resDataType, User user)
        {
            CrowdJudgement[] ordered = await (from res in DB.ParticipantResults
                where res.User.Email == user.Email
                from judgement in DB.CrowdJudgements
                where res.CrowdJobId == judgement.JobId
                orderby judgement.CreatedAt
                select judgement).ToArrayAsync();

            List<TimeGraphPoint> points = new List<TimeGraphPoint>();

            foreach (var judgement in ordered)
            {
                double yVal = (from data in judgement.Data
                    where data.DataType == resDataType
                    select data.NumResponse).Average();

                points.Add(new TimeGraphPoint
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
            User user = await AuthenticateUser(GetAuthentication());
            if (user == null)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            try
            {
                int submissionId = id ?? -1;
                int jobId = -1;
                if (submissionId >= 0)
                {
                    ParticipantResult res = await DB.ParticipantResults.FindAsync(id);
                    if (res != null)
                    {
                        if (res.User != user)
                        {
                            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden));
                        }
                        jobId = res.CrowdJobId;
                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
                    }
                }

                List<ParticipantFeedItem> feedback = new List<ParticipantFeedItem>();

                float? accentRating = await AverageRating("rlstaccent", user, jobId);
                if (accentRating != null)
                {
                    var accentFeedback = new ParticipantFeedItem
                    {
                        Rating = (float) accentRating,
                        Title = "Accent Influence",
                        Description =
                            "This rating shows how much your accent affected listeners' understanding of your speech.",
                        Date = DateTime.Now,
                        Dismissable = false,
                        Importance = 10
                    };
                    feedback.Add(accentFeedback);
                }

                float? transRating = await AverageRating("rlsttrans", user, submissionId);
                if (transRating != null)
                {
                    var transFeedback = new ParticipantFeedItem
                    {
                        Rating = (float) transRating,
                        Title = "Difficulty of Understanding",
                        Description = "This rating shows how difficult listeners find understanding what you say.",
                        Date = DateTime.Now,
                        Dismissable = false,
                        Importance = 10
                    };
                    feedback.Add(transFeedback);
                }


                ParticipantFeedItem graphFeedback = new ParticipantFeedItem
                {
                    Title = "Understanding Progress",
                    Description = "How understandable people have found you over time.",
                    Date = DateTime.Now,
                    Dismissable = false,
                    Importance = 8,
                    DataPoints = await GetGraphPoints("rlsttrans", user)
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