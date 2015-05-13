using Crowd.Model.Common;
using Crowd.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.Model.Data
{
    public class CrowdTaskRow: ICrowdTaskRowModel
    {
        public int Id{ get; set; }
        public string Name{ get; set; }
        public string Url{ get; set; }
        public string PhysicalPath{ get; set; }
        public string Description{ get; set; }
        public int Stage{ get; set; }
        public TaskType TaskType{ get; set; }
        public String UltimateAnswer { get; set; }
        public int CrowdTaskId { get; set; }
        public virtual ParticipantTask ParticipantTask { get; set; }

        public int CreateCrowdTaskRow()
        {
            throw new NotImplementedException();
        }

        public ParticipantTask GetCrowdTaskRowById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<CrowdTaskRow> GetAllCrowdTaskRows()
        {
            throw new NotImplementedException();
        }


        public ICollection<CrowdTaskRow> GetAllCrowdTaskRowsByTaskId(int taskId)
        {
            if (taskId <= 0)
                return new List<CrowdTaskRow>();
            //using (var db = new SpeechingContext())
            //{
            //    var tr = db.CrowdTaskRows.Where(r => r.CrowdTaskId.Equals(taskId)).OrderBy(r=>r.Stage).ToList();
            //    return tr;
            //} 
            return null;
        }
    }
}
