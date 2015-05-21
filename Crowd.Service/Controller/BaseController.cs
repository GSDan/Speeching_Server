using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Crowd.Model;
using Crowd.Model.Data;
using Crowd.Service.Model;

namespace Crowd.Service.Controller
{
    public class BaseController : ApiController
    {
        public BaseController()
        {
            DB = new CrowdContext();
        }

        protected CrowdContext DB { get; set; }

        /// <summary>
        /// Checks if the given details validate
        /// </summary>
        /// <param name="auth"></param>
        /// <returns>User obj if allowed, otherwise null</returns>
        protected async Task<User> AuthenticateUser(AuthenticationModel auth)
        {
            if (auth == null) return null;

            User found = await DB.Users.FindAsync(auth.Email);

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

        protected override void Dispose(bool disposing)
        {
            DB.Dispose();
            base.Dispose(disposing);
        }
    }
}