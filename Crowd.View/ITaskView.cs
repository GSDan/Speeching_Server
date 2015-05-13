using Crowd.Model.Common;
using Crowd.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.View
{
    public interface ITaskView : IBaseView
    {
        int TaskId { get; }
        String Name { get; set; }
        TaskType TaskType { get; set; }
        String Url { set; }
        //String PhysicalPath { get; set; }
        String Description { get; set; }
        //int Stage { get; set; }
        //int StageCount { get; set; }
        DateTime? CreatedOn { get; set; }
        DateTime? PublishedOn { get; set; }

        ICollection<CrowdTaskRow> TaskRows { set; }
        
        event EventHandler<EventArgs> SaveTask;
        event EventHandler<EventArgs> NewTask;
        event EventHandler<EventArgs> PrevTask;
        event EventHandler<EventArgs> NextTask;
    }
}
