using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Crowd.Model.Data;

namespace Crowd.Model.Interface
{
    public interface IParticipantTaskResponse
    {
        //int Key { get; set; }
        int ParticipantTaskId { get; set; }
        string ExternalId { get; set; }
        string Type { get; set; }
        string Prompt { get; set; }
        string Related { get; set; }
        ParticipantTask ParticipantTask { get; set; }
    }
}