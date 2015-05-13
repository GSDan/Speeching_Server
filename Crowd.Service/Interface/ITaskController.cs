using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Crowd.Model.Data;
using Crowd.Model.Interface;

namespace Crowd.Service.Interface
{
    public interface ITaskController
    {
        IEnumerable<IParticipantTask> Get();
        IParticipantTask Get(int id);
        HttpResponseMessage Put(int id, ParticipantTask task);
        HttpResponseMessage Post(ParticipantTask task);
        HttpResponseMessage Delete(int id);
    }
}
