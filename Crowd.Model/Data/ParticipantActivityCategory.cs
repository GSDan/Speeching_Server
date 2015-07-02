using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Crowd.Model.Interface;

namespace Crowd.Model.Data
{
    public class ParticipantActivityCategory : IParticipantActivityCategory
    {
        public int Id { get; set; }
        public string ExternalId { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public bool Recommended { get; set; }
        public bool DefaultSubscription { get; set; }
        public virtual ICollection<ParticipantActivity> Activities { get; set; }

        public User.AppType App { get; set; }
    }
}
