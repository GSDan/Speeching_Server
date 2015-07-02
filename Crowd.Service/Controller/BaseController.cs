using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Crowd.Model.Data;
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
                User.AppType app;
                if (Enum.TryParse(auth.App, true, out app))
                {
                    if (app != found.App)
                    {
                        found.App = app;

                        await db.SaveChangesAsync();
                    }
                }

                return found;
            }
            return null;
        }

        protected async Task<ServiceUser> AuthenticateServiceUser(AuthenticationModel auth, CrowdContext db)
        {
            User rawUser = await AuthenticateUser(auth, db);

            return rawUser == null ? null : new ServiceUser(rawUser);
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
                    Key = Request.Headers.GetValues("Key").First(),
                    App = Request.Headers.GetValues("App").First()
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

                // Get all responses made on these submissions
                IQueryable<CrowdRowResponse> allResponses = from crowdResponse in db.CrowdRowResponses
                            where crowdResponse.ParticipantAssessmentTask != null &&
                                  crowdResponse.ParticipantAssessmentTask.TaskType ==
                                  ParticipantAssessmentTask.AssessmentTaskType.QuickFire &&
                                  crowdResponse.ParticipantResult.User.Email == user.Email &&
                                  ((int)crowdResponse.ParticipantResult.ParticipantActivity.AppType == (int)user.App ||
                                  (int)crowdResponse.ParticipantResult.ParticipantActivity.AppType == (int)Crowd.Model.Data.User.AppType.None )
                            select crowdResponse;

                int numResp = await allResponses.CountAsync();

                if (numResp <= 0) return null;

                int numTotal = await (from crowdResponse in allResponses
                    from judgement in crowdResponse.TaskJudgements
                    from data in judgement.Data
                    where data.DataType == "rlstmp"
                    select data).CountAsync();

                if (numTotal <= 0) return null;

                // Get total num of correct judgements
                int numCorrect = await (from crowdResponse in allResponses
                    from judgement in crowdResponse.TaskJudgements
                    from prompt in db.ParticipantAssessmentTaskPrompts
                    where prompt.Id.ToString() == crowdResponse.ExtraData
                    from data in judgement.Data
                    where data.StringResponse == prompt.Value && data.DataType == "rlstmp"
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

                IQueryable<int> queryRes = from res in db.ParticipantResults
                               where res.User.Email == user.Email &&
                               ((int)res.ParticipantActivity.AppType == (int)user.App ||
                                  (int)res.ParticipantActivity.AppType == (int)Crowd.Model.Data.User.AppType.None )
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
                                                  where res.User.Email == user.Email &&
                                                  ((int)res.ParticipantActivity.AppType == (int)user.App ||
                                                    (int)res.ParticipantActivity.AppType == (int)Crowd.Model.Data.User.AppType.None )
                                                  from judgement in db.CrowdJudgements
                                                  where res.CrowdJobId == judgement.JobId
                                                  orderby judgement.CreatedAt
                                                  select judgement).ToArrayAsync();

                List<TimeGraphPoint> points = new List<TimeGraphPoint>();

                Dictionary<DateTime, List<double>> pointDictionary = new Dictionary<DateTime, List<double>>();

                // Group the results by day, so that each day with results will be a point on the graph

                foreach (CrowdJudgement judgement in ordered)
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

                foreach (KeyValuePair<DateTime, List<double>> pair in pointDictionary)
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