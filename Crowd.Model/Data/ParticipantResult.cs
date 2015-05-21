using Crowd.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.Model.Data
{
    public class ParticipantResult : IParticipantResult
    {
        public int Id { get; set; }
        public string ResourceUrl { get; set; }
        public string ResourceDirectory { get; set; }
        public string ExternalAccessToken { get; set;  }
        public int ParticipantActivityId { get; set; }
        public int CrowdJobId { get; set; }
        public ParticipantActivity ParticipantActivity { get; set; }
        public Dictionary<int, string> ParticipantTaskIdResults { get; set; }

        public virtual User User { get; set; }
    }
}
