using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Crowd.Model;
using Crowd.Model.Data;
using Newtonsoft.Json;

namespace Crowd.Service.Controller
{
    public class FeedbackController : BaseController
    {
        //http://localhost:52215/api/Feedback?id=null
        public async Task<HttpResponseMessage> Get(int? id)
        {
            using (CrowdContext db = new CrowdContext())
            {
                User user = await AuthenticateUser(GetAuthentication(), db);
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
                        ParticipantResult res = await db.ParticipantResults.FindAsync(id);
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

                    float? accentRating = await AverageRating("rlstaccent", user, db, jobId);
                    if (accentRating != null)
                    {
                        var accentFeedback = new ParticipantFeedItem
                        {
                            Rating = (float)accentRating,
                            Title = "Accent Influence",
                            Description =
                                "This rating shows how much your accent affected listeners' understanding of your speech.",
                            Date = DateTime.Now,
                            Dismissable = false,
                            Importance = 10
                        };
                        feedback.Add(accentFeedback);
                    }

                    float? transRating = await AverageRating("rlsttrans", user, db, submissionId);
                    if (transRating != null)
                    {
                        var transFeedback = new ParticipantFeedItem
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


                    ParticipantFeedItem graphFeedback = new ParticipantFeedItem
                    {
                        Title = "Understanding Progress",
                        Description = "How understandable people have found you over time.",
                        Date = DateTime.Now,
                        Dismissable = false,
                        Importance = 8,
                        DataPoints = await GetGraphPoints("rlsttrans", user, db)
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
}