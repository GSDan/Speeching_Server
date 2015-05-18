using Crowd.Model.Common;
using Crowd.Model.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.Model.Data
{
    public class ParticipantTask : IParticipantTask
    {
        public int Id { get; set; }
        public string ExternalId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public int ParticipantActivityId { get; set; }
        public virtual ParticipantActivity ParticipantActivity { get; set; }
        public virtual ParticipantTaskContent ParticipantTaskContent { get; set; }
        public virtual ParticipantTaskResponse ParticipantTaskResponse { get; set; }
        public virtual ICollection<CrowdRowResponse> CrowdRowResponses { get; set; }

        //public CrowdTask()
        //{
        //    //this.CrowdTaskRows = new HashSet<CrowdTaskRow>();
        //}
        //public CrowdTask GetCrowdTask()
        //{
        //    throw new NotImplementedException();
        //}

        //public int CreateCrowdTask()
        //{
        //    throw new NotImplementedException();
        //}
        //public IEnumerable<CrowdTask> GetAllCrowdTasks()
        //{
        //    using (var db = new SpeechingContext())
        //    {
        //        return db.CrowdTasks;
        //    }
        //}

        //public CrowdTask GetCrowdTaskById(int id)
        //{
        //    using (var db = new SpeechingContext())
        //    {
        //        var ct = db.CrowdTasks.SingleOrDefault(t => t.Id.Equals(id));
        //        return ct;
        //    }
        //}

        //public int Id { get; set; }
        //public string Name { get; set; }
        //public string Url { get; set; }
        //public string Description { get; set; }
        //public int MinJudgements { get; set; }
        //public int MaxJudgements { get; set; }
        //public DateTime? CreatedOn { get; set; }
        //public DateTime? PublishedOn { get; set; }
        //public DateTime? UnPublishedOn { get; set; }
        //public DateTime? CompletedOn { get; set; }
        //public TaskType TaskType { get; set; }
        //public int SpeechingUserId { get; set; }
        //public SpeechingUser SpeechingUser { get; set; }
        //public int? CrowdWorkerId { get; set; }
        //public CrowdWorker CrowdWorker { get; set; }
        ////public virtual ICollection<CrowdTaskRow> CrowdTaskRows { get; set; }
    }
}
