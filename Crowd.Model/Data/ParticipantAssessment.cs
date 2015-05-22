using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.Model.Data
{
    public class ParticipantAssessment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateSet { get; set; }
        public virtual ICollection<ParticipantAssessmentTask> Tasks { get; set; }
    }
}
