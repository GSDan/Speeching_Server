//using Crowd.Model.Data;
//using Crowd.Model.Interface;
using Crowd.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Crowd.Presenter
{
    public class TaskPresenter : BasePresenter/*, ITaskModel*/
    {
        ITaskView _taskView;
        public TaskPresenter(ITaskView taskView)
        {
            _taskView = taskView;
        }

        public void CreateTask()
        {
            throw new NotImplementedException();
        }

        public HttpResponseMessage NextTask()
        {//TODO: This method will look for the next task to follow the current TaskId
            using (HttpClient client = new HttpClient())
            {
                Uri baseAddress = new Uri("https://api.crowdflower.com/v1/");
                client.BaseAddress = baseAddress;
                var json = MakeJsonJob("title2", "instructions2");
                //postJson(json);
                HttpResponseMessage response = client.PostAsJsonAsync("jobs.json?key=yvQuiDN7zkaRQH8uhfnn", json).Result;

                string s = "safsdf";
                return new HttpResponseMessage() {StatusCode = HttpStatusCode.Accepted};
            }
            //_taskView.Status = new Dictionary<int, string>();
            //if (_taskView != null && _taskView.TaskId > 0)
            //{
            //    CrowdTask tmodel = new CrowdTask();
            //    var task = tmodel.Get(_taskView.TaskId);
            //    _taskView.Description = task.Description;
            //    _taskView.TaskType = task.TaskType;
            //    _taskView.PublishedOn = task.PublishedOn;
            //    _taskView.CreatedOn = task.CreatedOn;
            //    _taskView.Name = task.Name;
            //    _taskView.Url = task.Url;
            //    _taskView.Status.Add(0, "Success");
            //}
            //else
            //    _taskView.Status.Add(100, "Task Id is required");
        }

        private string MakeJsonJob(string title, string instructions)
        {
            return String.Format("{0}job[title]={1}&job[instructions]={2}{3}", "{", title, instructions, "}");
        }

        public void GetTask()//TODO: May be I change the name to LoadTask!
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    Uri baseAddress = new Uri("http://localhost:52215/api/");
                    client.BaseAddress = baseAddress;

                    var jsonData =
                        "{\"Id\": 61240,\"ResourceUrl\": \"https://di.ncl.ac.uk/owncloud/remote.php/webdav/uploads/7041992/1427112068907.7_3.zip\",\"ParticipantActivityId\": 3,\"UploadState\": 3,\"UserId\": 0,\"CompletionDate\": \"0001-01-01T00: 00: 00\"}";
                    HttpContent content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                    //var res = client.GetAsync("");
                    var response = client.PostAsync("ActivityResult", content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var toReturn = response.Content.ReadAsStringAsync();
                        //return JsonConvert.DeserializeObject<T>(toReturn);
                    }
                    else
                    {
                        var msg = response.Content.ReadAsStringAsync();
                        throw new Exception(msg.ToString());
                    }
                }
            }
            catch (Exception except)
            {
                Console.WriteLine(except);
                throw except;
            }
        }

        //public void GetTasks()
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        Uri baseAddress = new Uri("http://api.opescode.com/");
        //        client.BaseAddress = baseAddress;
        //        HttpResponseMessage response = client.GetAsync("api/Category/").Result;
        //        var obj = response.Content.ReadAsAsync<object>();]
        //        var categories = ((Newtonsoft.Json.Linq.JContainer) obj.Result);
        //        //return new HttpResponseMessage() { StatusCode = HttpStatusCode.Accepted };
        //        _taskView.Status = new Dictionary<int, string>();
        //        if (categories.Count > 0 && _taskView != null && _taskView.TaskId > 0)
        //        {
        //            foreach (var cat in categories)
        //            {

        //            }
        //            //CrowdTask tmodel = new CrowdTask();
        //            //var task = tmodel.GetCrowdTaskById(_taskView.TaskId);
        //            _taskView.Description = task.Description;
        //            _taskView.TaskType = task.TaskType;
        //            _taskView.PublishedOn = task.PublishedOn;
        //            _taskView.CreatedOn = task.CreatedOn;
        //            _taskView.Name = task.Name;
        //            _taskView.Url = task.Url;
        //            CrowdTaskRow ctr = new CrowdTaskRow();
        //            _taskView.TaskRows = ctr.GetAllCrowdTaskRowsByTaskId(task.Id); // task.CrowdTaskRows;

        //            _taskView.Status.Add(0, "Success");
        //        }
        //        else
        //            _taskView.Status.Add(100, "Task Id is required");
        //    }
        //}

        public void ValidateFeedback()
        {
        }
    }
}
