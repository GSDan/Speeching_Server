using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Crowd.Model;
using Crowd.Model.Data;
using Crowd.Service.Common;
using Newtonsoft.Json;

namespace Crowd.Service.Controller
{
    public class AccountController : BaseController
    {

        private static string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        private static bool ValidateToken(string encodedToken, string userEmail, User.AppType appType)
        {
            JwtSecurityToken token = new JwtSecurityToken(encodedToken);

            if (token.Claims == null)
            {
                return false;
            }

            Dictionary<string, string> claimVals = token.Claims.ToDictionary(x => x.Type, x => x.Value);

            if (claimVals["iss"] != "accounts.google.com" ||
                claimVals["azp"] != ConfidentialData.GoogleClientIdDictionary[appType] ||
                claimVals["aud"] != ConfidentialData.GoogleWebAppClientId ||
                claimVals["email"] != userEmail)
            {
                return false;
            }

            // Check token hasn't expired
            DateTime expirationDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            expirationDate = expirationDate.AddSeconds(int.Parse(claimVals["exp"]));

            // This is a valid token for this app if it's still in date!
            return expirationDate.ToLocalTime() >= DateTime.Now;
        }

        /// <summary>
        /// Takes a User object containing a Google OAuth token and authenticates it. If the token is valid
        /// the user obj is returned complete with a valid secret key which can be used in other controllers.
        /// If the token is valid and the user does not exist in the database, a new user is created and returned.
        /// </summary>
        /// <returns>User object in JSON</returns>
        [RequireHttps]
        public async Task<HttpResponseMessage> Post()
        {
            using (CrowdContext db = new CrowdContext())
            {
                string jsonData = HttpUtility.UrlDecode(await Request.Content.ReadAsStringAsync());
                bool isDebug;

                try
                {
                    isDebug = Request.Headers.GetValues("DebugSecret").First() == ConfidentialData.DebugSecret;
                }
                catch (Exception)
                {
                    // Debug header not found
                    isDebug = false;
                }

                User thisUser = JsonConvert.DeserializeObject<User>(jsonData);

                string encodedToken = thisUser.IdToken;

                if (string.IsNullOrEmpty(encodedToken) || string.IsNullOrWhiteSpace(thisUser.Email))
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                try
                {
                    // bypass validation if debug
                    bool validated = (isDebug) || ValidateToken(encodedToken, thisUser.Email, thisUser.App);

                    if (!validated) return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
                catch (Exception ex)
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }

                User existingUser = await db.Users.FindAsync(thisUser.Email);

                if (existingUser == null)
                {
                    // User not found - add new
                    thisUser.IsAdmin = false;
                    thisUser.Key = RandomString(32);
                    db.Users.Add(thisUser);
                    existingUser = thisUser;
                }
                else
                {
                    // Update the found user's details if they've been given
                    existingUser.Name = thisUser.Name ?? existingUser.Email;
                    existingUser.Nickname = thisUser.Nickname ?? existingUser.Nickname;
                    existingUser.Avatar = thisUser.Avatar ?? existingUser.Avatar;
                    existingUser.App = thisUser.App;
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