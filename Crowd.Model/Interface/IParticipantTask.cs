using System.Collections.Generic;
using Crowd.Model.Data;

namespace Crowd.Model.Interface
{
    public interface IParticipantTask
    {
        int Id { get; set; }
        string ExternalId { get; set; }
        string Description { get; set; }
        string Title { get; set; }
        int? ParticipantActivityId { get; set; }
        ParticipantActivity ParticipantActivity { get; set; }
        ParticipantTaskContent ParticipantTaskContent { get; set; }
        ParticipantTaskResponse ParticipantTaskResponse { get; set; }
        ICollection<CrowdRowResponse> CrowdRowResponses { get; set; }
    }
}