using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using Crowd.Model;
using Crowd.Model.Data;
using Crowd.Service.Common;
using Newtonsoft.Json;

namespace Crowd.Service.Model
{
    public class AudioUnit
    {
        public string AudioUrl { get; set; }
        public string AudioTypeCodec { get; set; }
        public int Id { get; set; }

        public AudioUnit()
        {
        }

        public AudioUnit(int id, string audioUrl, string audioTypeCodec)
        {
            this.AudioUrl = audioUrl;
            this.AudioTypeCodec = audioTypeCodec;
            this.Id = id;
        }

        public static string CreateCFData(IEnumerable<string> audioPaths, string audioTypeCodec, ParticipantActivity activity, ParticipantResult result)
        {
            string json = "";

            Dictionary<string, ICollection<string>> quickFireDictionary = new Dictionary<string, ICollection<string>>();
            Dictionary<string, int> taskIdDictionary = new Dictionary<string, int>();

            foreach (ParticipantAssessmentTask task in activity.AssessmentTasks)
            {
                if (task.TaskType == ParticipantAssessmentTask.AssessmentTaskType.QuickFire && !quickFireDictionary.ContainsKey("quickfire_" + task.Id))
                {
                    List<string> quickFireOptions = new List<string>();
                    foreach (var prompt in task.PromptCol.Prompts)
                    {
                        quickFireOptions.Add(prompt.Value);
                    }
                    quickFireDictionary.Add("quickfire_" + task.Id +"-", quickFireOptions);
                    taskIdDictionary.Add("quickfire_" + task.Id + "-", task.Id);
                }
                else if (task.TaskType == ParticipantAssessmentTask.AssessmentTaskType.ImageDescription && !taskIdDictionary.ContainsKey("imgDesc_" + task.Id))
                {
                    taskIdDictionary.Add("imgDesc_" + task.Id + "-", task.Id);
                }
                // TODO comparisons
            }

            foreach (var path in audioPaths)
            {
                string[] options = {"", "", "", "", "", "", "", "", "", "", "", ""};
                string taskType = "Other";

                string filename = Path.GetFileNameWithoutExtension(path);
                int assTaskId = -1;
                int normTaskId = -1;
                if(filename == null) continue;

                // Remove numbers from end of string
                filename = filename.TrimEnd(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });

                if (!string.IsNullOrEmpty(filename) && quickFireDictionary.ContainsKey(filename))
                {
                    options = quickFireDictionary[filename].ToArray();
                    assTaskId = taskIdDictionary[filename];
                    taskType = "MP";
                }
                else if (taskIdDictionary.ContainsKey(filename))
                {
                    assTaskId = taskIdDictionary[filename];
                    taskType = "Image"; //TODO
                }
                else
                {
                    normTaskId = int.Parse(Path.GetFileNameWithoutExtension(path));
                }

                var audioUrl = SvcUtil.GetAudioUrlFromPath(path);
                
                json += string.Format("{{\"AudioUrl\":\"{0}\", " +
                                      "\"AudioTypeCodec\":\"{1}\", " +
                                      "\"TaskType\":\"{2}\"," +
                                      "\"ParticipantAssessmentTaskId\":\"{3}\"," +
                                      "\"ParticipantTaskId\":\"{4}\""
                                      , audioUrl, audioTypeCodec, taskType, assTaskId, normTaskId);

                for (int i = 0; i < options.Length; i++)
                {
                    json += string.Format(",\"choice{0}\":\"{1}\"", i + 1, options[i]);
                }

                json += "}\r\n";
            }
            

            json = json.TrimEnd();

            return json;
        }
    }
}