using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crowd.Model.Common;
using Crowd.Model.Data;

namespace Crowd.View
{
    public class TaskView : ITaskView
    {
        public Dictionary<int, string> Status { get; set; }
        public int TaskId { get; private set; }
        public string Name { get; set; }
        public TaskType TaskType { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? PublishedOn { get; set; }
        public ICollection<CrowdTaskRow> TaskRows { get; set; }
        public event EventHandler<EventArgs> SaveTask;
        public event EventHandler<EventArgs> NewTask;
        public event EventHandler<EventArgs> PrevTask;
        public event EventHandler<EventArgs> NextTask;
    }
}
