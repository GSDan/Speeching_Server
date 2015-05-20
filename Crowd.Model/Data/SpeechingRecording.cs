using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.Model.Data
{
    public class SpeechingRecording
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Url { get; set; }
        public String PhysicalPath { get; set; }
        public int MinJudgements { get; set; }
        public int MaxJudgements { get; set; }
        public bool Complete { get; set; }

        public int SpeechingUserId { get; set; }
        public virtual User User { get; set; }

        public int? CrowdWorkerId { get; set; }
        public virtual CrowdWorker CrowdWorker { get; set; }
    }
}
