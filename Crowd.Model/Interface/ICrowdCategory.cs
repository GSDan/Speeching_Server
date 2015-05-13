using System.Collections.Generic;
using Crowd.Model.Data;
using Crowd.Model.Interface;

namespace Crowd.Model.Interface
{
    public interface ICrowdCategory
    {
        int Id { get; set; }
        string ExternalId { get; set; }
        string Title { get; set; }
        string Icon { get; set; }
        bool Recommended { get; set; }
        ICollection<ParticipantActivity> CrowdActivities { get; set; }
    }
}