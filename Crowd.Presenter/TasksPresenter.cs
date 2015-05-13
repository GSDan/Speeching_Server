using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Crowd.View;

namespace Crowd.Presenter
{
    public class TasksPresenter : BasePresenter
    {
        private ITasksView _tasksView;
        public TasksPresenter(ITasksView tasksView)
        {
            _tasksView = tasksView;
        }
        public void GetAllTasks()
        {
            using (HttpClient client = new HttpClient())
            {
                Uri baseAddress = new Uri("http://api.opescode.com/");
                client.BaseAddress = baseAddress;
                HttpResponseMessage response = client.GetAsync("api/Task/").Result;
                var obj = response.Content.ReadAsAsync<object>();
                var tasks = ((Newtonsoft.Json.Linq.JContainer) obj.Result);
                //return new HttpResponseMessage() { StatusCode = HttpStatusCode.Accepted };

                _tasksView.Status = new Dictionary<int, string>();
                if (tasks.Count > 0 && _tasksView != null)
                {
                    _tasksView.Tasks = new List<ITaskView>();
                    //ITaskView taskView;
                    foreach (var task in tasks)
                    {
                        _tasksView.Tasks.Add(new TaskView()
                        {
                            Description = (string) task["Description"],
                            //TaskType = task.TaskType,
                            //PublishedOn = task.PublishedOn,
                            //CreatedOn = task.CreatedOn,
                            Name = (string) task["Title"],
                            //Url = task.Url,
                            //CrowdTaskRow ctr = new CrowdTaskRow();
                            //TaskRows = ctr.GetAllCrowdTaskRowsByTaskId(task.Id),
                        });
                    }

                    _tasksView.Status.Add(0, "Success");
                }
                else
                    _tasksView.Status.Add(100, "Error occurs");
            }
        }
    }
}
