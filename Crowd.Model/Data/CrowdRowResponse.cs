using Crowd.Model.Interface;
using System.Collections.Generic;
using System;

namespace Crowd.Model.Data
{
    public class CrowdRowResponse : ICrowdRowResponse
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public int ParticipantResultId { get; set; }
        public int? ParticipantTaskId { get; set; }
        public int? ParticipantAssessmentTaskId { get; set; }
        public string RecordingUrl { get; set; }

        public virtual ParticipantResult ParticipantResult { get; set; }
        public virtual ParticipantTask ParticipantTask { get; set; }
        public virtual ParticipantAssessmentTask ParticipantAssessmentTask { get; set; }

        public string TaskType { get; set; }
        public string Choices { get; set; }
        public string PrevLoud { get; set; }
        public string PrevPace { get; set; }
        public string PrevPitch { get; set; }
        public string Comparison { get; set; }
        public string ExtraData { get; set; }

        // The judgements given for this row
        public virtual ICollection<CrowdJudgement> TaskJudgements { get; set; }
    }
}