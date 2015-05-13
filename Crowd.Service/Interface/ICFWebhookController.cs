using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.Service.Interface
{
    public interface ICFWebhookController
    {
        HttpResponseMessage Get();
        HttpResponseMessage Get(int id);
        HttpResponseMessage Put();
        HttpResponseMessage Post();
        HttpResponseMessage Delete(int id);
    }
}
