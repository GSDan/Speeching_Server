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
using Crowd.Model;
using Crowd.Service.Model;

namespace Crowd.Service.Controller
{
    public class BaseController : ApiController
    {
        /// <summary>
        /// Checks if the given details validate
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="db"></param>
        /// <returns>User obj if allowed, otherwise null</returns>
        protected async Task<User> AuthenticateUser(AuthenticationModel auth, CrowdContext db)
        {
            if (auth == null) return null;

            User found = await db.Users.FindAsync(auth.Email);

            if (found != null && found.Key == auth.Key)
            {
                return found;
            }
            return null;
        }

        /// <summary>
        /// Get the user details from the current Header
        /// </summary>
        /// <returns></returns>
        protected AuthenticationModel GetAuthentication()
        {
            try
            {
                return new AuthenticationModel
                {
                    Email = Request.Headers.GetValues("Email").First(),
                    Key = Request.Headers.GetValues("Key").First()
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected async Task<float?> MinimalPairsScore(User user, CrowdContext db, int jobId = -1)
        {
            try
            {
                // Get all Quickfire submissions
                var allSubmissions = from res in db.ParticipantResults
                                    where res.User.Email == user.Email
                                    from resData in res.Data
                                    where resData.ParticipantAssessmentTask != null
                                          && resData.ParticipantAssessmentTask.TaskType ==
                                          ParticipantAssessmentTask.AssessmentTaskType.QuickFire
                                    select resData;

                if ((await allSubmissions.ToArrayAsync()).Length == 0) return null;

                // Get total num of judgements 
                int numTotal = await (from resData in allSubmissions
                                    from judgement in db.CrowdJudgements
                                    where judgement.JobId == resData.ParentSubmission.CrowdJobId
                                    from data in judgement.Data
                                    where data.DataType == "rlstmp"
                                    select data).CountAsync();

                if (numTotal <= 0) return null;

                // Get total num of correct judgements
                int numCorrect = await (from resData in allSubmissions
                                        from judgement in db.CrowdJudgements
                                        where judgement.JobId == resData.ParentSubmission.CrowdJobId
                                        from data in judgement.Data
                                        where
                                            data.DataType == "rlstmp" &&
                                            data.StringResponse == resData.ParticipantAssessmentTaskPrompt.Value
                                        select data).CountAsync();

                float onePercent = numTotal/100f;

                return numCorrect/onePercent;
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected async Task<float?> AverageRating(string resDataType, User user, CrowdContext db, int jobId = -1)
        {
            try
            {
                if (jobId >= 0)
                {
                    return (float?)await (from judgement in db.CrowdJudgements
                                          where judgement.JobId == jobId
                                          from data in judgement.Data
                                          where data.DataType == resDataType
                                          select data.NumResponse).AverageAsync();
                }

                var queryRes = from res in db.ParticipantResults
                               where res.User.Email == user.Email
                               from judgement in db.CrowdJudgements
                               where res.CrowdJobId == judgement.JobId
                               from data in judgement.Data
                               where data.DataType == resDataType
                               select data.NumResponse;

                if (await queryRes.CountAsync() >= 1)
                {
                    return (float?)await queryRes.AverageAsync();
                }

                return null;
            }
            catch(Exception)
            {
                return null;
            }
        }

        protected async Task<TimeGraphPoint[]> GetGraphPoints(string resDataType, User user, CrowdContext db)
        {
            try
            {
                CrowdJudgement[] ordered = await (from res in db.ParticipantResults
                                                  where res.User.Email == user.Email
                                                  from judgement in db.CrowdJudgements
                                                  where res.CrowdJobId == judgement.JobId
                                                  orderby judgement.CreatedAt
                                                  select judgement).ToArrayAsync();

                List<TimeGraphPoint> points = new List<TimeGraphPoint>();

                Dictionary<DateTime, List<double>> pointDictionary = new Dictionary<DateTime, List<double>>();

                // Group the results by day, so that each day with results will be a point on the graph

                foreach (var judgement in ordered)
                {
                    double yVal = (from data in judgement.Data
                                   where data.DataType == resDataType
                                   select data.NumResponse).Average();

                    if (pointDictionary.ContainsKey(judgement.CreatedAt.Date))
                    {
                        pointDictionary[judgement.CreatedAt.Date].Add(yVal);
                    }
                    else
                    {
                        pointDictionary.Add(judgement.CreatedAt.Date, new List<double>() { yVal });
                    }
                }

                foreach (var pair in pointDictionary)
                {
                    points.Add(new TimeGraphPoint
                    {
                        XVal = pair.Key,
                        YVal = pair.Value.Average()
                    }); 
                }

                return points.ToArray();
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}