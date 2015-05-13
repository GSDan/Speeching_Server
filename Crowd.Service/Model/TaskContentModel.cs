using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crowd.Model.Data;

namespace Crowd.Service.Model
{
    public class TaskContentModel
    {
        //public int Id { get; set; }
        public string ExternalId { get; set; }
        public string Type { get; set; }
        public string Visual { get; set; }
        public string Audio { get; set; }
        public string Text { get; set; }
        public int ParticipantTaskId { get; set; }

        public static TaskContentModel Convert(ParticipantTaskContent crowdTaskContent)
        {
            var retTask = new TaskContentModel();
            if (crowdTaskContent != null)
            {
                retTask.Type = crowdTaskContent.Type;
                retTask.ExternalId = crowdTaskContent.ExternalId;
                //retTask.Id = crowdTaskContent.Id;
                retTask.Visual = crowdTaskContent.Visual;
                retTask.Audio = crowdTaskContent.Audio;
                retTask.Text = crowdTaskContent.Text;
                retTask.ParticipantTaskId = crowdTaskContent.ParticipantTaskId;
            }
            return retTask;
        }
    }
}