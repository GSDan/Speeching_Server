using System.Collections.Generic;
using Crowd.Model.Interface;

namespace Crowd.Model.Data
{
    public class ParticipantResult : IParticipantResult
    {
        public virtual User User { get; set; }
        public int Id { get; set; }
        public string ResourceUrl { get; set; }
        public string ResourceDirectory { get; set; }
        public string ExternalAccessToken { get; set; }
        public int ParticipantActivityId { get; set; }
        public int CrowdJobId { get; set; }
        public bool IsAssessment { get; set; }
        public ParticipantActivity ParticipantActivity { get; set; }
        public Dictionary<int, string> ParticipantTaskIdResults { get; set; }
    }
}