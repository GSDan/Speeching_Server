using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crowd.Model.Data;

namespace Crowd.Service.Model
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string ExternalId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public int CrowdActivityId { get; set; }
        public TaskResponseModel TaskResponse { get; set; }
        public TaskContentModel TaskContent { get; set; }
        public virtual IEnumerable<CrowdRowResponse> CrowdRowResponses { get; set; }

        public static TaskModel Convert(ParticipantTask crowdTask)
        {
            var retTask = new TaskModel();
            if (crowdTask != null)
            {
                retTask.Description = crowdTask.Description;
                retTask.ExternalId = crowdTask.ExternalId;
                retTask.Id = crowdTask.Id;
                retTask.Title = crowdTask.Title;
                retTask.TaskContent = TaskContentModel.Convert(crowdTask.ParticipantTaskContent);
                retTask.TaskResponse = TaskResponseModel.Convert(crowdTask.ParticipantTaskResponse);
                retTask.CrowdRowResponses = crowdTask.CrowdRowResponses;
            }
            return retTask;
        }

        public static IEnumerable<TaskModel> Convert(IEnumerable<ParticipantTask> crowdTasks)
        {
            var retTaskModels = new List<TaskModel>();
            if (crowdTasks != null && crowdTasks.Any())
            {
                foreach (var crowdTask in crowdTasks)
                {
                    var retTask = Convert(crowdTask);
                    if (retTask != null)
                        retTaskModels.Add(retTask);
                }
            }
            return retTaskModels;
        }
    }
}