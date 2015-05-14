using Crowd.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        ParticipantActivity ParticipantActivity { get; set; }
        Dictionary<int, string> ParticipantTaskIdResults { get; set; }
    }
}
