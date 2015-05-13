using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Crowd.Model.Data;

namespace Crowd.Service
{
    public partial class Tasks : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            gvTasks.DataSource = null;
            gvTasks.DataBind();
            string id = Request.QueryString["id"];
            var tasks = GetService<IEnumerable<ParticipantTask>>("http://api.opescode.com/", "api/Task/GetByActivityId/" + id);

            if (tasks.Any())
            {
                gvTasks.DataSource = tasks;
                gvTasks.DataBind();
            }
        }

        public T GetService<T>(string baseUri, string requestUri)
        {
            using (HttpClient client = new HttpClient())
            {
                Uri baseAddress = new Uri(baseUri);
                client.BaseAddress = baseAddress;

                HttpResponseMessage response = client.GetAsync(requestUri).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<T>().Result;
                }
                else
                {
                    string msg = response.Content.ReadAsStringAsync().Result;
                    throw new Exception(msg);
                }
            }
        }
    }
}