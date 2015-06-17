﻿using System;
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
                    ParticipantFeedItem graphFeedback = new ParticipantFeedItem
                    {
                        Title = "Understanding Progress",
                        Description = "How understandable people have found you over time.",
                        Date = DateTime.Now,
                        Dismissable = false,
                        Importance = 8,
                        DataPoints = points
                    };
                    items.Add(graphFeedback);
                }

                float? transRating = await AverageRating("rlsttrans", user, db);
                if (transRating != null)
                {
                    var transFeedback = new ParticipantFeedItem
                    {
                        Rating = (float)transRating,
                        Title = "Ease of Listening",
                        Description = "This rating shows on average how difficult listeners have found understanding what you say - the lower the better!",
                        Date = DateTime.Now,
                        Dismissable = false,
                        Importance = 10
                    };
                    items.Add(transFeedback);
                }

                float? accentRating = await AverageRating("rlstaccent", user, db);
                if (accentRating != null)
                {
                    var accentFeedback = new ParticipantFeedItem
                    {
                        Rating = (float)accentRating,
                        Title = "Accent Influence",
                        Description =
                            "This total average rating shows how much your accent affected listeners' understanding of your speech since you started.",
                        Date = DateTime.Now,
                        Dismissable = false,
                        Importance = 10
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
                        Importance = 10
                    };
                    items.Add(mpFeedback);
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
