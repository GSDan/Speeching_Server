using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Crowd.Model.Interface;
namespace Crowd.Model.Data
{
    public class ParticipantTaskResponse : IParticipantTaskResponse
    {
        //public int Id { get; set; }
        [Key, ForeignKey("ParticipantTask")]
        public int ParticipantTaskId { get; set; }
        public string ExternalId { get; set; }
        public string Type { get; set; }
        public string Prompt { get; set; }
        public string Related { get; set; }
        public virtual ParticipantTask ParticipantTask { get; set; }
    }
}