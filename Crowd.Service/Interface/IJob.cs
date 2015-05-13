using Crowd.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.Service.Interface
{
    public interface IJob : IContextBase
    {
        String Answer { get; set; }
        HttpResponseMessage Get();
        HttpResponseMessage Get(int id);
        HttpResponseMessage Create(JobModel Job);
    }
}
