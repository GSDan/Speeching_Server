using Crowd.Service.Interface;
using Crowd.Service.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Crowd.Service.Controller
{
    public class JobController : ApiController, IJob
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FileUrl { get; set; }
        public string Key { get; set; }

        public HttpResponseMessage Get()
        {
            throw new NotImplementedException();
        }

        public HttpResponseMessage Get(int id)
        {
            throw new NotImplementedException();
        }

        public HttpResponseMessage Create(JobModel job)
        {
            HttpClient client = new HttpClient();
            Uri baseAddress = new Uri("https://api.crowdflower.com/v1/");
            client.BaseAddress = baseAddress;
            var json = MakeJsonJob("title2", "instructions2");
            //postJson(json);
            HttpResponseMessage response = client.PostAsJsonAsync("jobs.json?key=yvQuiDN7zkaRQH8uhfnn", json).Result;
    
            string s = "safsdf";
            return new HttpResponseMessage() { StatusCode = HttpStatusCode.Accepted };
            //throw new NotImplementedException();
        }

        private string MakeJsonJob(string title, string instructions)
        {
            return String.Format("{0}job[title]={1}&job[instructions]={2}{3}", "{", title, instructions, "}");
        }

        private void postJson(string jsons)
        {
            var baseAddress = "https://api.crowdflower.com/v1/jobs.json?key=yvQuiDN7zkaRQH8uhfnn";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(baseAddress);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"title\":\"test\"," +
                              "\"password\":\"bla\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }

        public string Answer { get; set; }


        public string UserId { get; set; }
    }
}
