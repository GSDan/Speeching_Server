using Crowd.Model.Data;
using System.Collections.Generic;

namespace Crowd.Model.Interface
{
    public interface IParticipantResult
    {
        int Id { get; set; }
        string ResourceUrl { get; set; }
        string ExternalAccessToken { get; set;  }
        string ResourceDirectory { get; set; }
        int ParticipantActivityId { get; set; }
        int CrowdJobId { get; set; }
        bool IsAssessment { get; set; }
        ParticipantActivity ParticipantActivity { get; set; }
        ICollection<ParticipantResultData> Data { get; set; }
    }
}
