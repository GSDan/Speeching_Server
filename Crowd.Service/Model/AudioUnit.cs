using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Crowd.Model;
using Crowd.Model.Data;

namespace Crowd.Service.Model
{
    public class AudioUnit
    {
        public static string CreateCFData(IEnumerable<string> audioPaths, ParticipantActivity activity, ParticipantResult result)
        {
            string json = "";

            using (CrowdContext db = new CrowdContext())
            {
                result = db.ParticipantResults.Find(result.Id);
                CrowdRowResponse[] lastResponses = null;
                int lastAssessTaskId = -1;

                foreach (var path in audioPaths)
                {
                    string[] options = {"", "", "", "", "", "", "", "", "", "", "", ""};
                    string taskType = "Other";
                    string comparisonPath = "";

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

                        switch (task.TaskType)
                        {
                            case ParticipantAssessmentTask.AssessmentTaskType.QuickFire:
                                taskType = "MP";
                                break;
                            case ParticipantAssessmentTask.AssessmentTaskType.ImageDesc:
                                taskType = "Image";
                                break;
                        }

                        if (lastAssessTaskId != task.Id)
                        {
                            lastResponses = (from rowResp in db.CrowdRowResponses
                                            where rowResp.ParticipantAssessmentTaskId == task.Id &&
                                                  rowResp.ParticipantResult.User.Key == result.User.Key
                                            orderby rowResp.CreatedAt
                                            select rowResp).ToArray();
                            lastAssessTaskId = assTaskId;
                        }

                        if (lastResponses != null)
                        {
                            foreach (CrowdRowResponse resp in lastResponses)
                            {
                                // Can't use GetFileName in LINQ expressions so have to do in memory
                                bool matches = Path.GetFileName(resp.RecordingUrl) == Path.GetFileName(path);
                                if (!matches) continue;

                                comparisonPath = resp.RecordingUrl;
                                break;
                            }
                        }
                        
                    }
                    else if (thisData.ParticipantTask != null)
                    {
                        normTaskId = thisData.ParticipantTask.Id;
                    }

                    string choices = "";

                    for (int i = 0; i < options.Length; i++)
                    {
                        choices += options[i];
                        if (i < options.Length - 1) choices += ", ";
                    }

                    json += string.Format("{{\"AudioUrl\":\"{0}\", " +
                                          "\"TaskType\":\"{1}\"," +
                                          "\"ParticipantAssessmentTaskId\":\"{2}\"," +
                                          "\"ParticipantTaskId\":\"{3}\"," +
                                          "\"Choices\":\"{4}\"," +
                                          "\"Comparison\":\"{5}\"}}\r\n"
                        , path, taskType, assTaskId, normTaskId, choices, comparisonPath);
                }

            }
            json = json.TrimEnd();

            return json;
        }
    }
}