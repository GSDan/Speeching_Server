using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Crowd.Model.Data;
using Crowd.Model.Interface;
using Crowd.Service.Model;

namespace Crowd.Service
{
    public partial class Categories : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            string id = Request.QueryString["id"];
            try
            {
                var crowdCategories = GetService<IEnumerable<CategoryModel>>("http://api.opescode.com/",
                    "api/Category/" + id);

                gvCategories.DataSource = crowdCategories.Any() ? crowdCategories : null;

                gvCategories.DataBind();
            }
            catch (Exception ex)
            {
                string s = ex.Message;
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

        public T PutService<T>(string baseUri, string requestUri, string serializedCrowdCategory)
        {
            using (HttpClient client = new HttpClient())
            {
                Uri baseAddress = new Uri(baseUri + requestUri);
                var httpContent = new StringContent(serializedCrowdCategory, Encoding.UTF8, "application/json");
                var response = client.PutAsync(baseAddress, httpContent).Result;
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

        public T PostService<T>(string baseUri, string requestUri, string serializedCrowdCategory)
        {
            using (HttpClient client = new HttpClient())
            {
                Uri baseAddress = new Uri(baseUri + requestUri);
                var httpContent = new StringContent(serializedCrowdCategory, Encoding.UTF8, "application/json");
                var response = client.PostAsync(baseAddress, httpContent).Result;
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

        protected void gvCategories_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvCategories.EditIndex = e.NewEditIndex;
            BindData();
        }

        protected void gvCategoreis_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //Reset the edit index.
            gvCategories.EditIndex = -1;
            //Bind data to the GridView control.
            BindData();
        }


        protected void gvCategories_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvCategories.Rows[e.RowIndex];
            ParticipantActivityCategory crowdCat = new ParticipantActivityCategory();
            crowdCat.Id = Convert.ToInt32(gvCategories.DataKeys[e.RowIndex].Values[0]);
            crowdCat.ExternalId = ((TextBox)(row.Cells[1].Controls[0])).Text;
            crowdCat.Title = ((TextBox)(row.Cells[2].Controls[0])).Text;
            crowdCat.Icon = ((TextBox)(row.Cells[3].Controls[0])).Text;
            crowdCat.Recommended = ((CheckBox)(row.Cells[4].Controls[1])).Checked;
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            var jsonSerialized = jsonSerializer.Serialize(crowdCat);
            var response = PutService<ParticipantActivityCategory>("http://api.opescode.com/", "api/Category/", jsonSerialized);

            gvCategories.EditIndex = -1; 
            BindData();
        }

        protected void lbtnCreate_OnClick(object sender, EventArgs e)
        {
            ParticipantActivityCategory crowdCat = new ParticipantActivityCategory();
            crowdCat.ExternalId = "xyz";
            crowdCat.Title = "test title";
            crowdCat.Icon = "https://testaddress";
            crowdCat.Recommended = true;
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            var jsonSerialized = jsonSerializer.Serialize(crowdCat);
            var response = PostService<ParticipantActivityCategory>("http://api.opescode.com/", "api/Category/", jsonSerialized);
            BindData();
        }
    }
}