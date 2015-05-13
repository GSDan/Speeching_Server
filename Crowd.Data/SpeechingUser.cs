namespace Crowd.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SpeechingUser
    {
        public SpeechingUser()
        {
            CrowdTasks = new HashSet<CrowdTask>();
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string DisplayName { get; set; }

        public virtual ICollection<CrowdTask> CrowdTasks { get; set; }
    }
}
