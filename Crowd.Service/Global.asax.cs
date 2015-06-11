using System;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;

namespace Crowd.Service
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configure(config =>
            {
                config.MapHttpAttributeRoutes();

                config.Routes.MapHttpRoute("Help", "api/Help/{activityType}",
                    new { controller = "Help", activityType = "", action = "Get" });

                config.Routes.MapHttpRoute("DefaultApiAction", "api/{controller}/{action}/{id}",
                    new {id = RouteParameter.Optional}
                    );

                config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new {id = RouteParameter.Optional}
                    );
            });

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };

            ///* Initialise the database (Crowd.Model) */
            //CrowdContext db = new CrowdContext();
            //db.Database.Initialize(true);

            //RouteTable.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = System.Web.Http.RouteParameter.Optional }
            //    );
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }
    }
}