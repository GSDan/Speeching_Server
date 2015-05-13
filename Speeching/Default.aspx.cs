using Crowd.Presenter;
using Crowd.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Http;
using Crowd.Service;
using Crowd.Service.Interface;
using Crowd.Service.Model;

namespace Speeching
{
    public partial class Default : System.Web.UI.Page, ISpeechingTaskView
    {
        SpeechingTaskPresenter _presenter;
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            _presenter = new SpeechingTaskPresenter(this);
            btnHello.Click += delegate { _presenter.GetHello(); };
            if (!IsPostBack)
            {
                HttpClient client = new HttpClient();
                Uri baseAddress = new Uri("http://localhost:52215/");
                client.BaseAddress = baseAddress;

                System.Collections.ArrayList paramList = new System.Collections.ArrayList();
                Product product = new Product { Id = 1, Name = "Book", Price = 500, Category = "Soap" };
                ////Supplier supplier = new Supplier { SupplierId = 1, Name = "AK Singh", Address = "Delhi" };
                //paramList.Add(product);
                //paramList.Add(supplier);

                //HttpResponseMessage response = client.PostAsJsonAsync("api/Products/AddProduct", product).Result;

                //JobModel job = new JobModel() { Description = "wdwdw", Id = 20, Title = "dfds" };
                //response = client.PostAsJsonAsync("api/Job/Post", job).Result; 
                ////response = client.PostAsJsonAsync("api/Job/Get", 2).Result;
                //if (response.IsSuccessStatusCode)
                //{
                //    //return View();
                //}
                //else
                //{
                //    //return RedirectToAction("About");
                //}
            }
            //}
        }

        //protected void btnHello_Click(object sender, EventArgs e)
        //{

        //}

        public string Message
        {
            set { lblHelloWorld.Text = value; }
        }
    }
}