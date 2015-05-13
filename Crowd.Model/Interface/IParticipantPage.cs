using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crowd.Model.Data;

namespace Crowd.Model.Interface
{
    public interface IParticipantPage
    {
        int Id { get; set; }
        string MediaLocation { get; set; }
        string Text { get; set; }
        int CrowdActivityId { get; set; }
        ParticipantActivity CrowdActivity { get; set; }
    }
}
