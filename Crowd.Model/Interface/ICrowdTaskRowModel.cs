using Crowd.Model.Common;
using Crowd.Model.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.Model.Interface
{
    public interface ICrowdTaskRowModel
    {
        int Id { get; set; }
        String Name { get; set; }
        String Url { get; set; }
        String PhysicalPath { get; set; }
        [Column(TypeName = "ntext")]
        String Description { get; set; }
        int Stage { get; set; }
        //bool Complete { get; set; }
        TaskType TaskType { get; set; }
        String UltimateAnswer { get; set; }
        #region Methods
        int CreateCrowdTaskRow();
        ParticipantTask GetCrowdTaskRowById(int id);
        ICollection<CrowdTaskRow> GetAllCrowdTaskRows();
        ICollection<CrowdTaskRow> GetAllCrowdTaskRowsByTaskId(int taskId);
        #endregion
    }
}
