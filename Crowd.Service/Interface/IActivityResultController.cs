using System.Net.Http;
using System.Threading.Tasks;
using Crowd.Model.Data;

namespace Crowd.Service.Interface
{
    public interface IActivityResultController
    {
        Task<HttpResponseMessage> Get();
        Task<HttpResponseMessage> Get(int id);
        Task<HttpResponseMessage> GetByActivityId(int id);
        Task<HttpResponseMessage> Put(int id, ParticipantResult crowdResult);
        Task<HttpResponseMessage> Post(ParticipantResult crowdResult);
        Task<HttpResponseMessage> Delete(int id);
    }
}
