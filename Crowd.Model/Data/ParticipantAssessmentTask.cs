using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.Model.Data
{
    public class ParticipantAssessmentTask
    {
        public enum AssessmentTaskType { ImageDescription, QuickFire }

        public AssessmentTaskType TaskType { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Instructions { get; set; }
        public virtual ParticipantAssessmentTaskPromptCol PromptCol { get; set; }

        //Only used in image description
        public string Image { get; set; }
    }
}
