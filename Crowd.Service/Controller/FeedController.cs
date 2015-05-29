using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Crowd.Model.Data;
using Newtonsoft.Json;

namespace Crowd.Service.Controller
{
    public class FeedController : BaseController
    {
        public async Task<HttpResponseMessage> Get()
        {
            User user = await AuthenticateUser(GetAuthentication());
            if (user == null)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            ICollection<ParticipantFeedItem> dismissed = user.DismissedPublicFeedItems;
            ICollection<ParticipantFeedItem> userItems = user.FeedItems;

            ParticipantFeedItem[] items = await (from feedItem in DB.ParticipantFeedItems
                where (feedItem.Global && !dismissed.Contains(feedItem)) || userItems.Contains(feedItem)
                select feedItem).ToArrayAsync();

            return new HttpResponseMessage()
            {
                Content = new JsonContent(items)
            };
        }

        public async Task<HttpResponseMessage> Post()
        {
            string jsonData = HttpUtility.UrlDecode(await Request.Content.ReadAsStringAsync());

            ParticipantFeedItem newItem = JsonConvert.DeserializeObject<ParticipantFeedItem>(jsonData);

            if (string.IsNullOrEmpty(newItem.Title) || string.IsNullOrEmpty(newItem.Description))
            {
                return new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
            }

            newItem.Global = true;
            DB.ParticipantFeedItems.Add(newItem);
            
            try
            {
                await DB.SaveChangesAsync();
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }
    }
}
