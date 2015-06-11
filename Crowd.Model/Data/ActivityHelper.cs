using System.ComponentModel.DataAnnotations;

namespace Crowd.Model.Data
{
    public class ActivityHelper
    {
        [Key]
        public ParticipantAssessmentTask.AssessmentTaskType ActivityType { get; set; }
        public string ActivityName { get; set; }
        public string ActivityDescription { get; set; }
        public string HelpVideo { get; set; }
    }
}
