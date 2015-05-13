using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crowd.Model.Interface;

namespace Crowd.Model.Data
{
    public class CrowdPage : IParticipantPage
    {
        public int Id { get; set; }
        public string MediaLocation { get; set; }
        public string Text { get; set; }
        public int CrowdActivityId { get; set; }
        public virtual ParticipantActivity CrowdActivity { get; set; }
    }
}
