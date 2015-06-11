using System.Collections.Generic;
using Crowd.Model.Data;

namespace Crowd.Model.Interface
{
    public interface IParticipantActivity
    {
        int Id { get; set; }
        string ExternalId { get; set; }
        string PrincipleInvestigatorId { get; set; }
        string Title { get; set; }
        string Icon { get; set; }
        string Resource { get; set; }
        int CrowdCategoryId { get; set; }

        ParticipantActivityCategory CrowdCategory { get; set; }
        ICollection<ParticipantTask> ParticipantTasks { get; set; }
        ICollection<ParticipantPage> CrowdPages { get; set; }
        ICollection<ParticipantResult> ParticipantResults { get; set; }
    }
}