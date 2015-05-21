using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Crowd.Model;
using Crowd.Model.Data;
using Crowd.Service.Model;

namespace Crowd.Service.Controller
{
    public class BaseController : ApiController
    {
        protected CrowdContext DB { get; set; }

        /// <summary>
        /// Checks if the given details validate
        /// </summary>
        /// <param name="auth"></param>
        /// <returns>True if correct</returns>
        protected async Task<bool> AuthenticateUser(AuthenticationModel auth)
        {
            if (auth == null) return false;

            User found = await DB.Users.FindAsync(auth.Email);

            return found != null && found.Key == auth.Key;
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

        public BaseController()
        {
            DB = new CrowdContext();
        }

        protected override void Dispose(bool disposing)
        {
            DB.Dispose();
            base.Dispose(disposing);
        }
    }
}
