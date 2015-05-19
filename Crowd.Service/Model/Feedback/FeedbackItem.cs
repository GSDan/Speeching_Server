using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crowd.Service.Interface;

namespace Crowd.Service.Model.Feedback
{
    public class FeedbackItem : IFeedbackItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public bool Dismissable { get; set; }
        public int Importance { get; set; }
    }
}