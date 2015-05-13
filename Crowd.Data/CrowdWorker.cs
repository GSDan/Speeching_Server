namespace Crowd.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CrowdWorker
    {
        public CrowdWorker()
        {
            CrowdTasks = new HashSet<CrowdTask>();
        }

        public int Id { get; set; }

        [StringLength(255)]
        public string SourceId { get; set; }

        [StringLength(255)]
        public string Source { get; set; }

        public string Email { get; set; }

        public string DisplayName { get; set; }

        public virtual ICollection<CrowdTask> CrowdTasks { get; set; }
    }
}
