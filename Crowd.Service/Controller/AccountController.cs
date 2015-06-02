using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Crowd.Model;
using Crowd.Model.Data;
using Crowd.Service.Common;
using Newtonsoft.Json;

namespace Crowd.Service.Controller
{
    public class AccountController : BaseController
    {
        [RequireHttps]
        public async Task<HttpResponseMessage> Post()
        {
            using (CrowdContext db = new CrowdContext())
            {
                string jsonData = HttpUtility.UrlDecode(await Request.Content.ReadAsStringAsync());

                User thisUser = JsonConvert.DeserializeObject<User>(jsonData);
                User existingUser = await db.Users.FindAsync(thisUser.Email);

                if (existingUser == null)
                {
                    // User not found - add new, with default subscriptions
                    thisUser.SubscribedCategories = db.ParticipantActivityCategories.Where(
                        cat => cat.DefaultSubscription).ToList();

                    db.Users.Add(thisUser);
                    existingUser = thisUser;
                }
                else
                {
                    // Update the found user's details if they've been given
                    existingUser.Name = thisUser.Name ?? existingUser.Email;
                    existingUser.Nickname = thisUser.Nickname ?? existingUser.Nickname;
                    existingUser.Avatar = thisUser.Avatar ?? existingUser.Avatar;
                }

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }

                return new HttpResponseMessage
                {
                    Content = new JsonContent(existingUser),
                    StatusCode = HttpStatusCode.OK
                };
            }
        }
    }
}
