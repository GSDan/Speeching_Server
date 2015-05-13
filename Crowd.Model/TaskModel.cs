using Crowd.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.Model
{
    public class TaskModel : ITaskModel
    {

        public int SaveRecordings()
        {
            throw new NotImplementedException();
        }

        public string GetTasks()
        {
            throw new NotImplementedException();
        }

        public string SetupTask()
        {
            throw new NotImplementedException();
        }

        public string SayHello()
        {
            using (var db = new CrowdContext())
            {
                string ret = "";
                //var speechSample = new SpeechingSample { Filename = "xyz.mp3", Description = "test" };
                //db.SpeechingSamples.Add(speechSample);
                //db.SaveChanges();
                //var query = db.SpeechingSamples;
                //foreach (var q in query)
                //    ret += q.Filename + ", ";

                return ret;
            }
        }
    }
}
