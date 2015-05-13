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
    public interface ICategoryController
    {
        HttpResponseMessage Get();
        HttpResponseMessage Get(int id);
        HttpResponseMessage Put(CrowdCategory category);
        HttpResponseMessage Post(CrowdCategory category);
        HttpResponseMessage Delete(int id);
    }


}
