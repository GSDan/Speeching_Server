using Crowd.Model.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.Model.Interface
{
    public interface ICrowdRowResponse
    {
        string Id { get; set; }
        DateTime CreatedAt { get; set; }

        int ParticipantResultId { get; set; }
        int ParticipantTaskId { get; set; }

        [ForeignKey("ParticipantResultId")]
        ParticipantResult ParticipantResult { get; set; }
        [ForeignKey("ParticipantTaskId")]
        ParticipantTask ParticipantTask { get; set; }

        // The judgements given for this row
        ICollection<CrowdJudgement> TaskJudgements { get; set; }
    }
}
