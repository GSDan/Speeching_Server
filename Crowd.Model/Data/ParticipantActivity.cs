using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crowd.Model.Interface;

namespace Crowd.Model.Data
{
    public class ParticipantActivity : IParticipantActivity
    {
        public ParticipantActivity()
        {
            //CrowdTasks = new Collection<CrowdTask>();
        }
        public int Id { get; set; }
        public string ExternalId { get; set; }
        public string PrincipleInvestigatorId { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Resource { get; set; }
        public int CrowdCategoryId { get; set; }
        public virtual CrowdCategory CrowdCategory { get; set; }
        public virtual ICollection<ParticipantTask> ParticipantTasks { get; set; }
        public virtual ICollection<CrowdPage> CrowdPages { get; set; }
        public virtual ICollection<ParticipantResult> ParticipantResults { get; set; }
    }
}
