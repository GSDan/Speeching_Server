using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Crowd.Model;
using Crowd.Model.Data;

namespace Crowd.Service.Controller
{
    public class BaseController : ApiController
    {
        protected CrowdContext DB { get; set; }

        public async Task<bool> AuthenticateUser(int key, string email)
        {
            User found = await DB.Users.FindAsync(email);

            return found != null && found.Key == key;
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
