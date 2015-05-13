using Crowd.Model.Data;

namespace Crowd.Model.Interface
{
    public interface IScientistTaskResponse
    {
        //int Id { get; set; }
        string ExternalId { get; set; }
        string Type { get; set; }
        string Prompt { get; set; }
        int ParticipantTaskId { get; set; }
        ParticipantTask ParticipantTask { get; set; }
    }
}