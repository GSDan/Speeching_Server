using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.View
{
    public class TasksView : ITasksView
    {
        public Dictionary<int, string> Status { get; set; }

        public IList<ITaskView> Tasks { get; set; }
    }
}
