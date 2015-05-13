using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.Service.Interface
{
    public interface IContextBase
    {
        String Key { get; set; }
        int Id { get; set; }
        String UserId { get; set; }
        String Title { get; set; }
        String Description { get; set; }
    }
}
