using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Crowd.Model.Data;
using Newtonsoft.Json;

namespace Crowd.Service
{
    public partial class CreateFeedItem : Page
    {
        private const string BaseUri = "https://localhost:44300/api/";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["newStoryForm"] == null)
            {
                Session["newStoryForm"] = "waiting";
            }
        }

        private void ShowAlert(string message)
        {
            Response.Write("<script>alert('"+ message +"');</script>");
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (Page.IsPostBack && (string)Session["newStoryForm"] == "submitted")
            {
                Session["newStoryForm"] = "waiting";
                newTitle.Text = "";
                newDesc.Text = "";
                newLink.Text = "";
                newImage.Text = "";
                newType.SelectedIndex = 0;
                UpdateDropdown();
                ShowAlert("Post submitted");
            }
        }

        private static async void Post(ParticipantFeedItem item)
        {
            using (HttpClient client = new HttpClient())
            { 
                Uri baseAddress = new Uri(BaseUri);
                client.BaseAddress = baseAddress;

                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, BaseUri + "Feed");

                //TODO login headers

                HttpContent content = new StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8,
                    "application/json");
                requestMessage.Content = content;

                HttpResponseMessage response = await client.SendAsync(requestMessage);
                string toReturn = await response.Content.ReadAsStringAsync();
            }

        }

        protected void submitBtn_Click(object sender, EventArgs e)
        {
            if (!IsValid)
            {
                return;
            }

            Session["newStoryForm"] = "submitted";

            ParticipantFeedItem item = new ParticipantFeedItem
            {
                Title = newTitle.Text,
                Description = newDesc.Text,
                Dismissable = true,
                Importance = 1,
                Global = true
            };

            int typeVal = int.Parse(newType.SelectedValue);

            if (typeVal >= 1)
            {

                item.Image = newImage.Text;
            }

            if (typeVal >= 2)
            {
                item.Interaction = new ParticipantFeedItemInteraction
                {
                    Label = "Open story",
                    Type = ParticipantFeedItemInteraction.InteractionType.Url,
                    Value = newLink.Text
                };
            }

            Post(item);
        }

        private void UpdateDropdown()
        {
            switch (newType.SelectedValue)
            {
                case "0":
                    imageDiv.Visible = false;
                    linkDiv.Visible = false;
                    break;
                case "1":
                    imageDiv.Visible = true;
                    linkDiv.Visible = false;
                    break;
                case "2":
                    imageDiv.Visible = true;
                    linkDiv.Visible = true;
                    break;
            }
        }

        protected void newType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDropdown();
        }

    }
}