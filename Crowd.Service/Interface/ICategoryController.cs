using System.Net.Http;
using System.Threading.Tasks;
using Crowd.Model.Data;

namespace Crowd.Service.Interface
{
    public interface ICategoryController
    {
        Task<HttpResponseMessage> Get();
        Task<HttpResponseMessage> Get(int id);
        Task<HttpResponseMessage> Put(ParticipantActivityCategory category);
        Task<HttpResponseMessage> Post(ParticipantActivityCategory category);
        Task<HttpResponseMessage> Delete(int id);
    }
}
