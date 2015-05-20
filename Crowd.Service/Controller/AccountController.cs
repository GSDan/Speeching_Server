using System;
using System.Collections.Generic;
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
    public class AccountController : BaseController
    {
        public async Task<HttpResponseMessage> Post()
        {
            var req = this.Request;
            string jsonData = HttpUtility.UrlDecode(await req.Content.ReadAsStringAsync());

            User thisUser = JsonConvert.DeserializeObject<User>(jsonData);
            User existingUser = await DB.Users.FindAsync(thisUser.Email);

            if (existingUser == null)
            {
                // User not found - add new
                DB.Users.Add(thisUser);
                existingUser = thisUser;
            }
            else
            {
                // Update the found user's details if they've been given
                existingUser.Email = thisUser.Email ?? existingUser.Email;
                existingUser.Name = thisUser.Name ?? existingUser.Email;
                existingUser.Nickname = thisUser.Nickname ?? existingUser.Nickname;
                existingUser.Avatar = thisUser.Avatar ?? existingUser.Avatar;
            }

            try
            {
                await DB.SaveChangesAsync();
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            return new HttpResponseMessage
            {
                Content = new JsonContent(JsonConvert.SerializeObject(existingUser)),
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
