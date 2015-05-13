using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.Model.Interface
{
    public interface IUser
    {
        Guid Id { get; set; }
        String FirstName { get; set; }
        String LastName { get; set; }
    }
}
