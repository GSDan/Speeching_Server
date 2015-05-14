using Crowd.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.Model.Interface
{
    public interface ICrowdTaskJudgement
    {
        string Id { get; set; }
        DateTime Created_at { get; set; }
        bool Tainted { get; set; }
        string Country { get; set; }
        string City { get; set; }
        int JobId { get; set; }
        int WorkerId { get; set; }
        double Trust { get; set; }

        int ParticipantTaskId { get; set; }
        ParticipantTask ParticipantTask { get; set; }

        // eg "txta":"hello can I order a pizza please"
        Dictionary<string, string> Data { get; set; }
    }
}
