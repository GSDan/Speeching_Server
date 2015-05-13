using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Crowd.Model;

namespace Crowd.Service.Controller
{
    public class BaseController : ApiController
    {
        protected CrowdContext DB { get; set; }

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
