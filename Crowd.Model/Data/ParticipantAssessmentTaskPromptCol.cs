using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.Model.Data
{
    public class ParticipantAssessmentTaskPromptCol
    {
        public enum PromptTaskType
        {
            MinimalPairs, ImageDesc
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public PromptTaskType PromptType { get; set; }
        public virtual ICollection<ParticipantAssessmentTaskPrompt> Prompts { get; set; }
    }
}
