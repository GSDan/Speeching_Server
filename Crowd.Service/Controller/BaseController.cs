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
                    Key = int.Parse(Request.Headers.GetValues("Key").First())
                };
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

                return (float?)await queryRes.AverageAsync();
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
            catch(Exception)
            {
                return null;
            }
        }
    }
}