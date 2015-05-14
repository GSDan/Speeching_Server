using System.Data.Entity;
using Crowd.Model.Data;
using Crowd.Model.Interface;

namespace Crowd.Model
{
    public class CrowdContext : DbContext
    {
        public CrowdContext()
            : base("name=CrowdScience")
        {
            System.Console.WriteLine("*****start******");
            Database.SetInitializer<CrowdContext>(new CustomDBInitializer());
            this.Database.Initialize(true);
        }
        public DbSet<CrowdCategory> CrowdCategories { get; set; }
        public DbSet<ParticipantTask> ParticipantTasks { get; set; }
        public DbSet<CrowdPage> CrowdPages { get; set; }
        public DbSet<ParticipantTaskContent> ParticipantTaskContents { get; set; }
        public DbSet<ParticipantTaskResponse> ParticipantTaskResponses { get; set; }
        public DbSet<CrowdTaskResponse> CrowdTaskResponses { get; set; }
        public DbSet<ParticipantActivity> ParticipantActivities { get; set; }
        public DbSet<ParticipantResult> ParticipantResults { get; set; }
        //public DbSet<SpeechingSample> SpeechingSamples { get; set; }
        //public DbSet<CrowdWorker> CrowdWorkers { get; set; }
        //public DbSet<SpeechingUser> SpeechingUsers { get; set; }
        //public DbSet<CrowdTaskRow> CrowdTaskRows { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParticipantTask>()
            .HasOptional(l => l.ParticipantTaskContent)
            .WithRequired(r => r.ParticipantTask);

            modelBuilder.Entity<ParticipantTask>()
            .HasOptional(l => l.ParticipantTaskResponse)
            .WithRequired(r => r.ParticipantTask);

            //modelBuilder.Entity<ParticipantTask>()
            //.HasOptional(l => l.ScientistTaskResponse)
            //.WithRequired(r => r.ParticipantTask);

            //modelBuilder.Entity<CrowdTask>()
            //    .HasOptional(w => w.CrowdWorker)
            //    .WithMany(c=> c.SpeechingRecordings)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<CrowdTask>()
            //    .HasRequired(w => w.SpeechingUser)
            //    .WithMany(c => c.SpeechingRecordings)
            //    .WillCascadeOnDelete(false);

            //To rename a column using Fluent API
            //modelBuilder.Entity<SpeechingSample>()
            //    .Property(s => s.Filename)
            //    .HasColumnName("File_Name");
        }
    }
}