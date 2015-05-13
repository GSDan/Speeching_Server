using Crowd.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.View
{
    public interface ITaskRowView : IBaseView
    {
        int RowId { get; }
        String Name { get; set; }
        TaskType TaskType { get; set; }
        String Url { set; }
        String PhysicalPath { get; set; }
        String Description { get; set; }
        int Stage { get; set; }
        
        event EventHandler<EventArgs> Save;
        event EventHandler<EventArgs> New;
        event EventHandler<EventArgs> Prev;
        event EventHandler<EventArgs> Next;
    }
}
