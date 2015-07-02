using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crowd.Model.Data
{
    public class User
    {
        public enum AppType { None = 0, Speeching = 1, Fluent = 2 };

        [Key]
        public string Email { get; set; }

        [StringLength(450)]
        [Index(IsUnique = true)]
        public string Key { get; set; }

        public string IdToken { get; set; }
        public string RefToken { get; set; }

        public string Name { get; set; }
        public string Nickname { get; set; }
        public string Avatar { get; set; }
        public bool IsAdmin { get; set; }
        public virtual ICollection<ParticipantActivityCategory> SubscribedCategories { get; set; }
        public virtual ICollection<ParticipantResult> Submissions { get; set; }

        public virtual ParticipantResult LastAssessment { get; set; }

        public virtual ICollection<ParticipantFeedItem> FeedItems { get; set; }
        public virtual ICollection<ParticipantFeedItem> DismissedPublicFeedItems { get; set; }

        public AppType App { get; set; }
    }
}