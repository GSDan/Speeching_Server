using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Crowd.Model.Data;

namespace Crowd.Model.Interface
{
    public interface IParticipantTaskContent
    {
        //int Key { get; set; }
        int ParticipantTaskId { get; set; }
        string ExternalId { get; set; }
        string Type { get; set; }
        string Visual { get; set; }
        string Audio { get; set; }
        string Text { get; set; }
        ParticipantTask ParticipantTask { get; set; }
    }
}