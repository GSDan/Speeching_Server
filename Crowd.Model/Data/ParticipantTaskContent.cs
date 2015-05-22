using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Crowd.Model.Interface;

namespace Crowd.Model.Data
{
    public class ParticipantTaskContent : IParticipantTaskContent
    {
        //public int Key { get; set; }
        [Key, ForeignKey("ParticipantTask")]
        public int ParticipantTaskId { get; set; }
        public string ExternalId { get; set; }
        public string Type { get; set; }
        public string Visual { get; set; }
        public string Audio { get; set; }
        public string Text { get; set; }

        public virtual ParticipantTask ParticipantTask { get; set; }
    }
}