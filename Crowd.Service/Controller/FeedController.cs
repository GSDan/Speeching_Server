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
        private async Task<ParticipantActivity> GetAssessmentIfNeeded(CrowdContext db, User user)
        {
            var blob = await (from upload in db.ParticipantResults
                where upload.User.Email == user.Email &&
                      upload.IsAssessment
                orderby upload.UploadedAt
                select upload).ToArrayAsync();

            Console.WriteLine(blob.Length);

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
                        Description = "This rating shows on average how easy listeners have found understanding what you say.",
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
                            "This total average rating shows easy to understand people find your accent.",
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

                await GetAssessmentIfNeeded(db, user);
                items.Add( new ParticipantFeedItem
                {
                    Title = "A new assessment is available!",
                    Description = "There's a new assessment available for you to complete! The feedback from completing this short activity will help you to keep track of your progress.",
                    Date = DateTime.Now,
                    Dismissable = false,
                    Importance = 10,
                    Interaction = new ParticipantFeedItemInteraction
                    {
                        Type = ParticipantFeedItemInteraction.InteractionType.Assessment,
                        Value = "5",
                        Label = "Start Assessment!"
                    }
                });

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
