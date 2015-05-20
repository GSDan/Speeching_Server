using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crowd.Model.Data;

namespace Crowd.Service.Model
{
    public class TaskResponseModel
    {
        //public int Key { get; set; }
        public string ExternalId { get; set; }
        public string Type { get; set; }
        public string Prompt { get; set; }
        public int ParticipantTaskId { get; set; }
        public List<string> Related { get; set; }
        
        public static TaskResponseModel Convert(ParticipantTaskResponse crowdTaskResponse)
        {
            var retTask = new TaskResponseModel();
            if (crowdTaskResponse != null)
            {
                retTask.Type = crowdTaskResponse.Type;
                retTask.ExternalId = crowdTaskResponse.ExternalId;
                //retTask.Key = crowdTaskResponse.Key;
                retTask.Prompt = crowdTaskResponse.Prompt;
                retTask.ParticipantTaskId = crowdTaskResponse.ParticipantTaskId;
                retTask.Related = new List<string>();
                if (!String.IsNullOrWhiteSpace(crowdTaskResponse.Related))
                {
                    retTask.Related.AddRange(crowdTaskResponse.Related.Split(new char[] {'|'}));
                }
            }
            return retTask;
        }
    }
}