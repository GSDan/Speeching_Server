using Crowd.Model.Interface;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Crowd.Model.Data
{
    public class ParticipantTask : IParticipantTask
    {
        public int Id { get; set; }
        public string ExternalId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public int? ParticipantActivityId { get; set; }
        public virtual ParticipantActivity ParticipantActivity { get; set; }
        public virtual ParticipantTaskContent ParticipantTaskContent { get; set; }
        public virtual ParticipantTaskResponse ParticipantTaskResponse { get; set; }

        [JsonIgnore]
        public virtual ICollection<CrowdRowResponse> CrowdRowResponses { get; set; }
    }
}
