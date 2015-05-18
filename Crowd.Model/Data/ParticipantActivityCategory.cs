using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crowd.Model.Interface;

namespace Crowd.Model.Data
{
    public class ParticipantActivityCategory : IParticipantActivityCategory
    {
        public ParticipantActivityCategory()
        {
            //CrowdActivities = new Collection<CrowdActivity>();
        }
        public int Id { get; set; }
        public string ExternalId { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public bool Recommended { get; set; }
        public virtual ICollection<ParticipantActivity> Activities { get; set; }
    }
}
