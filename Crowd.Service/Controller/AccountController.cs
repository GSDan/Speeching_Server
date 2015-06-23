using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml;
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
                    // User not found - add new
                    thisUser.IsAdmin = false;
                    db.Users.Add(thisUser);
                    existingUser = thisUser;
                }
                else
                {
                    if (thisUser.Key != existingUser.Key)
                    {
                        //if (string.IsNullOrEmpty(thisUser.IdToken))
                        //{
                        return new HttpResponseMessage(HttpStatusCode.Unauthorized); 
                        //}
                        //TODO
                        //JwtSecurityToken token = new JwtSecurityToken(thisUser.IdToken);
                        //JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

                        //var validationParams = new TokenValidationParameters()
                        //{
                        //    ValidAudience = ConfidentialData.GoogleClientId,
                        //    ValidIssuers = new[] {"accounts.google.com", "https://accounts.google.com"},
                        //    RequireExpirationTime = true,
                            
                        //};
                        
                        //validationParams.s

                    }
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
