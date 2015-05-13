namespace Crowd.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CrowdTaskRow
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string PhysicalPath { get; set; }

        public string Description { get; set; }

        public int Stage { get; set; }

        public int TaskType { get; set; }

        public int CrowdTaskId { get; set; }

        public string UltimateAnswer { get; set; }

        public virtual CrowdTask CrowdTask { get; set; }
    }
}
