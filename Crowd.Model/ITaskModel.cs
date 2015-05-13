using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.Model
{
    public interface ITaskModel
    {
        int SaveRecordings(/*what is the recordings object*/);
        string GetTasks();
        string SetupTask();

        string SayHello();
    }
}
