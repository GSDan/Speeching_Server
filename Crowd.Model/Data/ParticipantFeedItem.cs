using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crowd.Model.Data
{
    public class ParticipantFeedItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public bool Dismissable { get; set; }
        public int Importance { get; set; }
        public ParticipantFeedItemInteraction Interaction { get; set; }

        // Shown to all users?
        public bool Global { get; set; }

        // Only used if the feed item is regarding a launchable activity
        public ParticipantActivity PracticeActivity { get; set; }
        public string[] Rationale { get; set; }

        // Only used if the feed item contains a graph
        public TimeGraphPoint[] DataPoints { get; set; }

        // Only used if the feed item contains an image
        public string Image { get; set; }

        // Only used if the feed item is to show a % rating
        public float? Percentage { get; set; }

        // Only used if the feed item is to show a star rating out of 5
        public float? Rating { get; set; }

        // Only used if the feed item should show user profile information
        public User UserAccount { get; set; }

        public ParticipantFeedItem()
        {
            Interaction = new ParticipantFeedItemInteraction
            {
                Type = ParticipantFeedItemInteraction.InteractionType.None
            };
            Percentage = null;
            Rating = null;
        }
    }

    [ComplexType]
    public class ParticipantFeedItemInteraction
    {
        public enum InteractionType { None, Url, Assessment, Activity }

        public InteractionType Type { get; set; }
        public string Value { get; set; }
        public string Label { get; set; }
    }

    [ComplexType]
    public class TimeGraphPoint
    {
        public double YVal;
        public DateTime XVal;
    }
}
