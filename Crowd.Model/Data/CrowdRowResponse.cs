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
        public int ParticipantTaskId { get; set; }

        [ForeignKey("ParticipantResultId")]
        public virtual ParticipantResult ParticipantResult { get; set; }
        [ForeignKey("ParticipantTaskId")]
        public virtual ParticipantTask ParticipantTask { get; set; }

        // The judgements given for this row
        public virtual ICollection<CrowdJudgement> TaskJudgements { get; set; }
    }
}