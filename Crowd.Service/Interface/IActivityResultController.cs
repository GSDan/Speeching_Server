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
    public interface IActivityResultController
    {
        HttpResponseMessage Get();
        HttpResponseMessage Get(int id);
        HttpResponseMessage GetByActivityId(int id);
        HttpResponseMessage Put(int id, ActivityResultModel crowdResult);
        HttpResponseMessage Post(ActivityResultModel crowdResult);
        HttpResponseMessage Delete(int id);
    }
}
