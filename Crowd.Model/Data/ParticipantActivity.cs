using System;
using System.Collections.Generic;
using Crowd.Model.Interface;
using Newtonsoft.Json;

namespace Crowd.Model.Data
{
    public class ParticipantActivity : IParticipantActivity
    {
        public int Id { get; set; }
        public string ExternalId { get; set; }
        public string PrincipleInvestigatorId { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Resource { get; set; }
        public int CrowdCategoryId { get; set; }
        public string Description { get; set; }
        public DateTime DateSet { get; set; }
        [JsonIgnore]
        public virtual ParticipantActivityCategory CrowdCategory { get; set; }

        // For scenarios
        public virtual ICollection<ParticipantTask> ParticipantTasks { get; set; }

        // For guides
        public virtual ICollection<ParticipantPage> CrowdPages { get; set; }

        // For assessments
        public virtual ICollection<ParticipantAssessmentTask> AssessmentTasks { get; set; }

        [JsonIgnore]
        public virtual ICollection<ParticipantResult> ParticipantResults { get; set; }
    }
}
