namespace Crowd.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CrowdContext : DbContext
    {
        public CrowdContext()
            : base("name=CrowdContext")
        {
        }

        public virtual DbSet<CrowdTaskRow> CrowdTaskRows { get; set; }
        public virtual DbSet<CrowdTask> CrowdTasks { get; set; }
        public virtual DbSet<CrowdWorker> CrowdWorkers { get; set; }
        public virtual DbSet<SpeechingSample> SpeechingSamples { get; set; }
        public virtual DbSet<SpeechingUser> SpeechingUsers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SpeechingUser>()
                .HasMany(e => e.CrowdTasks)
                .WithRequired(e => e.SpeechingUser)
                .WillCascadeOnDelete(false);
        }
    }
}
