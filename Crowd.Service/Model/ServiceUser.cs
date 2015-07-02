using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crowd.Model.Data;

namespace Crowd.Service.Model
{
    /// <summary>
    /// Used to hide unneeded database info from the service's output
    /// </summary>
    public class ServiceUser
    {
        public string Email { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string Avatar { get; set; }
        public bool IsAdmin { get; set; }
        public List<ParticipantActivityCategory> SubscribedCategories { get; set; }
        public List<ParticipantResult> Submissions { get; set; }

        public ParticipantResult LastAssessment { get; set; }

        public List<ParticipantFeedItem> FeedItems { get; set; }
        public List<ParticipantFeedItem> DismissedPublicFeedItems { get; set; }
        public User.AppType App;


        public ServiceUser(User baseUser)
        {
            Email = baseUser.Email;
            Key = baseUser.Key;
            Name = baseUser.Name;
            Nickname = baseUser.Nickname;
            Avatar = baseUser.Avatar;
            IsAdmin = baseUser.IsAdmin;
            SubscribedCategories = baseUser.SubscribedCategories.ToList();
            Submissions = baseUser.Submissions.ToList();
            LastAssessment = baseUser.LastAssessment;
            FeedItems = baseUser.FeedItems.ToList();
            DismissedPublicFeedItems = baseUser.DismissedPublicFeedItems.ToList();
            App = baseUser.App;
        }
    }
}