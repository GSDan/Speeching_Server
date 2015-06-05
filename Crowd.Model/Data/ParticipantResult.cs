using System.Collections.Generic;
using Crowd.Model.Interface;

namespace Crowd.Model.Data
{
    public class ParticipantResult : IParticipantResult
    {
        public virtual User User { get; set; }
        public int Id { get; set; }
        public string ResourceUrl { get; set; }
        public string ResourceDirectory { get; set; }
        public string ExternalAccessToken { get; set; }
        public int ParticipantActivityId { get; set; }
        public int CrowdJobId { get; set; }
        public bool IsAssessment { get; set; }
        public ParticipantActivity ParticipantActivity { get; set; }
        public virtual ICollection<ParticipantResultData> Data { get; set; }
    }

    public class ParticipantResultData
    {
        public int Id { get; set; }
        public int? ParticipantTaskId { get; set; }
        public int? ParticipantAssessmentTaskId { get; set; }
        public int? ParticipantAssessmentTaskPromptId { get; set; }

        public virtual ParticipantResult ParentSubmission { get; set; }
        public virtual ParticipantTask ParticipantTask { get; set; }
        public virtual ParticipantAssessmentTask ParticipantAssessmentTask { get; set; }
        public virtual ParticipantAssessmentTaskPrompt ParticipantAssessmentTaskPrompt { get; set; }
        public string FilePath { get; set; }
        public string ExtraData { get; set; }
    }
}