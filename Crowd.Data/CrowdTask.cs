namespace Crowd.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CrowdTask
    {
        public CrowdTask()
        {
            CrowdTaskRows = new HashSet<CrowdTaskRow>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public int SpeechingUserId { get; set; }

        public int? CrowdWorkerId { get; set; }

        public int MinJudgements { get; set; }

        public int MaxJudgements { get; set; }

        public DateTime? PublishedOn { get; set; }

        public DateTime? UnPublishedOn { get; set; }

        public DateTime? CompletedOn { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string Description { get; set; }

        public int TaskType { get; set; }

        public virtual ICollection<CrowdTaskRow> CrowdTaskRows { get; set; }

        public virtual CrowdWorker CrowdWorker { get; set; }

        public virtual SpeechingUser SpeechingUser { get; set; }
    }
}
