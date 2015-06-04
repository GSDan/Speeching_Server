using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        // The judgements given for this row
        public virtual ICollection<CrowdJudgement> TaskJudgements { get; set; }
    }
}