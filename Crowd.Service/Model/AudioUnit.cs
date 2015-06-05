using System.Collections.Generic;
using System.IO;
using System.Linq;
using Crowd.Model;
using Crowd.Model.Data;
using Crowd.Service.Common;

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
            AudioUrl = audioUrl;
            AudioTypeCodec = audioTypeCodec;
            Id = id;
        }

        public static string CreateCFData(IEnumerable<string> audioPaths, string audioTypeCodec,
            ParticipantActivity activity, ParticipantResult result)
        {
            string json = "";

            using (CrowdContext db = new CrowdContext())
            {
                result = db.ParticipantResults.Find(result.Id);

                foreach (var path in audioPaths)
                {
                    string[] options = {"", "", "", "", "", "", "", "", "", "", "", ""};
                    string taskType = "Other";

                    ParticipantResultData thisData = (from data in result.Data
                        where data.FilePath == Path.GetFileName(path)
                        select data).FirstOrDefault();

                    if (thisData == null)
                    {
                        continue;
                    }

                    int assTaskId = -1;
                    int normTaskId = -1;

                    if (thisData.ParticipantAssessmentTask != null)
                    {
                        ParticipantAssessmentTask task = thisData.ParticipantAssessmentTask;
                        assTaskId = task.Id;

                        List<string> quickFireOptions = new List<string>();
                        foreach (var prompt in task.PromptCol.Prompts)
                        {
                            quickFireOptions.Add(prompt.Value);
                        }
                        options = quickFireOptions.ToArray();

                        if (task.TaskType == ParticipantAssessmentTask.AssessmentTaskType.QuickFire)
                        {
                            taskType = "MP";
                        }
                        else if (task.TaskType == ParticipantAssessmentTask.AssessmentTaskType.ImageDescription)
                        {
                            taskType = "Image";
                        }
                    }
                    else if (thisData.ParticipantTask != null)
                    {
                        normTaskId = thisData.ParticipantTask.Id;
                    }

                    json += string.Format("{{\"AudioUrl\":\"{0}\", " +
                                          "\"AudioTypeCodec\":\"{1}\", " +
                                          "\"TaskType\":\"{2}\"," +
                                          "\"ParticipantAssessmentTaskId\":\"{3}\"," +
                                          "\"ParticipantTaskId\":\"{4}\""
                        , path, audioTypeCodec, taskType, assTaskId, normTaskId);

                    for (int i = 0; i < options.Length; i++)
                    {
                        json += string.Format(",\"choice{0}\":\"{1}\"", i + 1, options[i]);
                    }

                    json += "}\r\n";
                }

            }
            json = json.TrimEnd();

            return json;
        }
    }
}