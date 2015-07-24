using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Crowd.Model;
using Crowd.Model.Data;
using Newtonsoft.Json;

namespace Crowd.Service.Controller
{
    public class FeedController : BaseController
    {
        /// <summary>
        /// Checks if the user has completed an assessment today, returning one if not
        /// </summary>
        /// <returns></returns>
        private static async Task<ParticipantActivity> GetAssessmentIfNeeded(CrowdContext db, User user)
        {
            bool existing = false;

            if (user.LastAssessment != null)
            {
                ParticipantActivity act = await db.ParticipantActivities.FindAsync(user.LastAssessment.ParticipantActivityId);
                if (act != null && act.AppType == user.App) existing = true;
            }

            if (existing)
            {
                // We want the user to complete the same assessment each time, at most once a day
                ParticipantResult recentUpload = await (from upload in db.ParticipantResults
                    where upload.User.Email == user.Email &&
                          upload.IsAssessment &&
                          ((int)upload.ParticipantActivity.AppType == (int)user.App ||
                          (int)upload.ParticipantActivity.AppType == (int)Crowd.Model.Data.User.AppType.None)
                    orderby upload.UploadedAt descending
                    select upload).FirstOrDefaultAsync();

                if(recentUpload == null) return null;

                TimeSpan span = DateTime.Now - recentUpload.UploadedAt;

                if (span.Days >= 1)
                {
                    if (recentUpload.ParticipantActivity != null)
                    {
                        return recentUpload.ParticipantActivity;
                    }

                    return await db.ParticipantActivities.FindAsync(recentUpload.ParticipantActivityId);
                }
            }
            else
            {
                // User has yet to complete an assessment - choose a random one
                ParticipantActivity[] assessments = await (
                    from act in db.ParticipantActivities
                    where act.AssessmentTasks.Count >= 1 &&
                          ((int)act.AppType == (int)user.App ||
                          (int)act.AppType == (int)Crowd.Model.Data.User.AppType.None)
                    select act).ToArrayAsync();

                if (assessments.Length >= 1)
                {
                    Random rand = new Random();
                    return assessments[rand.Next(0, assessments.Length - 1)];
                }
            }
            return null;
        }

        private static async Task<ParticipantActivity> GetRandomScenario(CrowdContext db, User user)
        {
            ParticipantActivity[] acts = await (from act in db.ParticipantActivities
                where (act.AppType == Crowd.Model.Data.User.AppType.None || act.AppType == user.App) &&
                      act.ParticipantTasks.Count >= 1
                select act).ToArrayAsync();

            if (acts.Length >= 1)
            {
                Random rand = new Random();
                return acts[rand.Next(0, acts.Length - 1)];
            }
            return null;
        }

        /// <summary>
        /// Returns the user's subscription feed
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Get()
        {
            using (CrowdContext db = new CrowdContext())
            {
                User user = await AuthenticateUser(GetAuthentication(), db);
                if (user == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                // Get a list of all feed items which are either
                // 1- Globally visible and have not been dismissed by the user
                // 2- On this user's list of personal feed items
                List<ParticipantFeedItem> items = await (from feedItem in db.ParticipantFeedItems
                                                         where (int)feedItem.App == (int)Crowd.Model.Data.User.AppType.None || (int)feedItem.App == (int)user.App
                                                         from thisUser in db.Users
                                                         where thisUser.Email == user.Email
                                                         where (feedItem.Global && !thisUser.DismissedPublicFeedItems.Contains(feedItem)) || thisUser.FeedItems.Contains(feedItem)
                                                         select feedItem).ToListAsync();

                TimeGraphPoint[] points = await GetGraphPoints("rlsttrans", user, db);
                if (points != null && points.Length >= 2)
                {
                    foreach (TimeGraphPoint point in points)
                    {
                        point.YVal = 5 - point.YVal;
                    }

                    ParticipantFeedItem graphFeedback = new ParticipantFeedItem
                    {
                        Title = "Understanding Progress",
                        Description = "How understandable people have found you over time.",
                        Date = DateTime.Now,
                        Dismissable = false,
                        Importance = 9,
                        DataPoints = points
                    };
                    items.Add(graphFeedback);
                }

                float? transRating = await AverageRating("rlsttrans", user, db);
                if (transRating != null)
                {
                    transRating = 5 - transRating;

                    var transFeedback = new ParticipantFeedItem
                    {
                        Rating = (float)transRating,
                        Title = "Ease of Listening",
                        Description = "This rating shows how easy people found understanding what you said.",
                        Date = DateTime.Now,
                        Dismissable = false,
                        Importance = 9
                    };
                    items.Add(transFeedback);
                }

                float? accentRating = await AverageRating("rlstaccent", user, db);
                if (accentRating != null)
                {
                    accentRating = 5 - accentRating;

                    var accentFeedback = new ParticipantFeedItem
                    {
                        Rating = (float)accentRating,
                        Title = "Accent Clarity",
                        Description =
                            "This total average rating shows how easy to understand people find your accent.",
                        Date = DateTime.Now,
                        Dismissable = false,
                        Importance = 8
                    };
                    items.Add(accentFeedback);
                }

                float? minPairsRating = await MinimalPairsScore(user, db);
                if (minPairsRating != null)
                {
                    var mpFeedback = new ParticipantFeedItem
                    {
                        Percentage = minPairsRating,
                        Title = "QuickFire Success",
                        Description = "This is the success rating of people identifying the words spoken in your QuickFire tests!",
                        Date = DateTime.Now,
                        Dismissable = false,
                        Importance = 8
                    };
                    items.Add(mpFeedback);
                }

                if (items.Count == 0)
                {
                    items.Add(new ParticipantFeedItem
                    {
                        Title = "No stories available",
                        Description = "Your news feed is looking a bit empty! Complete assessments and activities to fill it up with results and feedback. Be sure to check back regularly!",
                        Date = DateTime.Now,
                        Dismissable = false,
                        Importance = 5,
                    });
                }

                if (user.App == Crowd.Model.Data.User.AppType.Speeching)
                {
                    ParticipantActivity assessment = await GetAssessmentIfNeeded(db, user);
                    if (assessment != null)
                    {
                        items.Add(new ParticipantFeedItem
                        {
                            Title = "A new assessment is available!",
                            Description =
                                "There's a new assessment available for you to complete!\n" + assessment.Description,
                            Date = DateTime.Now,
                            Dismissable = false,
                            Importance = 10,
                            Interaction = new ParticipantFeedItemInteraction
                            {
                                Type = ParticipantFeedItemInteraction.InteractionType.Assessment,
                                Value = assessment.Id.ToString(),
                                Label = "Start Assessment!"
                            }
                        });
                    }
                    else
                    {
                        items.Add(new ParticipantFeedItem
                        {
                            Title = "No Assessment Available",
                            Description = "You submitted an assessment recently - please wait at least a day before doing another.",
                            Date = DateTime.Now,
                            Dismissable = false,
                            Importance = 5,
                            App = Crowd.Model.Data.User.AppType.None
                        });
                    }
                }
                else if(user.App == Crowd.Model.Data.User.AppType.Fluent)
                {
                    ParticipantActivity scenario = await GetRandomScenario(db, user);
                    if (scenario != null)
                    {
                        items.Add(new ParticipantFeedItem
                        {
                            Title = "Try this scenario!",
                            Description =
                                "There's a scenario available for you to complete!\n" + scenario.Description,
                            Date = DateTime.Now,
                            Dismissable = false,
                            Importance = 10,
                            Interaction = new ParticipantFeedItemInteraction
                            {
                                Type = ParticipantFeedItemInteraction.InteractionType.Activity,
                                Value = scenario.Id.ToString(),
                                Label = "Start Scenario!"
                            }
                        });
                    }
                }

                return new HttpResponseMessage()
                {
                    Content = new JsonContent(items)
                };
            }
        }


        public async Task<HttpResponseMessage> Dismiss(int id)
        {
            using (CrowdContext db = new CrowdContext())
            {
                User user = await AuthenticateUser(GetAuthentication(), db);
                if (user == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                ParticipantFeedItem feedItem = await db.ParticipantFeedItems.FindAsync(id);

                if (!user.DismissedPublicFeedItems.Contains(feedItem))
                {
                    user.DismissedPublicFeedItems.Add(feedItem);
                }
                if (user.FeedItems.Contains(feedItem))
                {
                    user.FeedItems.Remove(feedItem);
                }

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }

        public async Task<HttpResponseMessage> Post()
        {
            using (CrowdContext db = new CrowdContext())
            {
                string jsonData = HttpUtility.UrlDecode(await Request.Content.ReadAsStringAsync());

                ParticipantFeedItem newItem = JsonConvert.DeserializeObject<ParticipantFeedItem>(jsonData);

                if (string.IsNullOrEmpty(newItem.Title) || string.IsNullOrEmpty(newItem.Description))
                {
                    return new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
                }

                newItem.Date = DateTime.Now;
                newItem.Global = true;
                db.ParticipantFeedItems.Add(newItem);

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }

                return new HttpResponseMessage(HttpStatusCode.Accepted);
            }
        }
    }
}
