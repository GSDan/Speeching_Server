using Crowd.Model.Common;
using Crowd.Model.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.Model.Interface
{
    public interface ICrowdTaskModel
    {
        int Id { get; set; }
        String Name { get; set; }
        String Url { get; set; }
        [Column(TypeName = "ntext")]
        String Description { get; set; }
        int MinJudgements { get; set; }
        int MaxJudgements { get; set; }
        DateTime? CreatedOn { get; set; }
        DateTime? PublishedOn { get; set; }
        DateTime? UnPublishedOn { get; set; }
        DateTime? CompletedOn { get; set; }
        TaskType TaskType { get; set; }

        int SpeechingUserId { get; set; }
        SpeechingUser SpeechingUser { get; set; }

        int? CrowdWorkerId { get; set; }
        CrowdWorker CrowdWorker { get; set; }
        
        #region Methods
        //int SaveRecordings(/*what is the recordings object*/);
        ParticipantTask GetCrowdTask();
        int CreateCrowdTask();
        //IEnumerable<CrowdTask>
        ParticipantTask GetCrowdTaskById(int id);
        IEnumerable<ParticipantTask> GetAllCrowdTasks();
        #endregion
    }
}
