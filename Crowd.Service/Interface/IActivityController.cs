using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Crowd.Model.Data;
using Crowd.Model.Interface;
using Crowd.Service.Model;

namespace Crowd.Service.Interface
{
    public interface IActivityController
    {
        HttpResponseMessage Get();
        HttpResponseMessage Get(int id);
        HttpResponseMessage Put(int id, ParticipantActivity activity);
        HttpResponseMessage Post(ParticipantActivity activity);
        HttpResponseMessage Delete(int id);
    }
}
