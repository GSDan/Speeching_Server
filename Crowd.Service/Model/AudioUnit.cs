using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Crowd.Model;
using Crowd.Model.Data;

namespace Crowd.Service.Model
{
    public class AudioUnit
    {
        public static string CreateCFData(IEnumerable<string> audioPaths, ParticipantActivity activity, ParticipantResult result)
        {
            string json = "";
            if (!audioPaths.Any())
                return json;

            using (CrowdContext db = new CrowdContext())
            {
                result = db.ParticipantResults.Find(result.Id);
                CrowdRowResponse[] lastAssessResponses = null;
                CrowdRowResponse[] lastNormResponses = null;
                int lastAssessTaskId = -1;
                int lastNormTaskId = -1;
                string extraData = "";
                if (result.User.App == User.AppType.Speeching)
                {
                    foreach (var path in audioPaths)
                    {
                        string[] options = null;
                        string taskType = "Other";
                        CrowdRowResponse comparionResponse = null;

                        ParticipantResultData thisData = (from data in result.Data
                                                          where data.FilePath == Path.GetFileName(path)
                                                          select data).FirstOrDefault();

                        if (thisData == null)
                        {
                            continue;
                        }

                        int assTaskId = -1;
                        int normTaskId = -1;

                        if (thisData.ParticipantAssessmentTask != null) // Is assessment
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

                            string filename = Path.GetFileNameWithoutExtension(path);
                            string promptId = "";
                            bool started = false;

                            // Get the prompt index from the filename
                            foreach (char c in filename)
                            {
                                if (c == '-')
                                {
                                    started = true;
                                    continue;
                                }
                                if (started && char.IsDigit(c))
                                {
                                    promptId += c;
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(promptId))
                            {
                                extraData = promptId;
                            }

                            if (lastAssessTaskId != task.Id)
                            {
                                lastAssessResponses = (from rowResp in db.CrowdRowResponses
                                                       where rowResp.ParticipantAssessmentTaskId == task.Id &&
                                                             rowResp.ParticipantResult.User.Key == result.User.Key
                                                       orderby rowResp.CreatedAt
                                                       select rowResp).ToArray();
                                lastAssessTaskId = assTaskId;
                            }

                            if (lastAssessResponses != null)
                            {
                                foreach (CrowdRowResponse resp in lastAssessResponses)
                                {
                                    // Can't use GetFileName in LINQ expressions so have to do in memory
                                    bool matches = Path.GetFileName(resp.RecordingUrl) == Path.GetFileName(path);
                                    if (!matches) continue;

                                    comparionResponse = resp;
                                    break;
                                }
                            }

                        }
                        else if (thisData.ParticipantTask != null) // Not assessment
                        {
                            normTaskId = thisData.ParticipantTask.Id;
                            if (lastNormTaskId != normTaskId)
                            {
                                lastNormResponses = (from rowResp in db.CrowdRowResponses
                                                     where rowResp.ParticipantTaskId == normTaskId &&
                                                           rowResp.ParticipantResult.User.Key == result.User.Key
                                                     orderby rowResp.CreatedAt
                                                     select rowResp).ToArray();
                                lastNormTaskId = normTaskId;
                            }
                            if (lastNormResponses != null)
                            {
                                foreach (CrowdRowResponse resp in lastNormResponses)
                                {
                                    // Can't use GetFileName in LINQ expressions so have to do in memory
                                    bool matches = Path.GetFileName(resp.RecordingUrl) == Path.GetFileName(path);
                                    if (!matches) continue;

                                    comparionResponse = resp;
                                    break;
                                }
                            }
                        }

                        string choices = "";
                        string comparisonPath = "";
                        int prevLoud = -1;
                        int prevPace = -1;
                        int prevPitch = -1;

                        if (options != null)
                        {
                            // Shuffle the words
                            Random rnd = new Random();
                            options = options.OrderBy(x => rnd.Next()).ToArray();

                            for (int i = 0; i < options.Length; i++)
                            {
                                choices += options[i];
                                if (i < options.Length - 1) choices += ", ";
                            }
                        }

                        if (comparionResponse != null)
                        {
                            comparisonPath = comparionResponse.RecordingUrl;
                            prevLoud = GetAverageRes(comparionResponse, "rlstvolume");
                            prevPace = GetAverageRes(comparionResponse, "rlstpace");
                            prevPitch = GetAverageRes(comparionResponse, "rlstpitch");
                        }

                        json += string.Format("{{\"AudioUrl\":\"{0}\", " +
                                              "\"TaskType\":\"{1}\"," +
                                              "\"ParticipantAssessmentTaskId\":\"{2}\"," +
                                              "\"ParticipantTaskId\":\"{3}\"," +
                                              "\"Choices\":\"{4}\"," +
                                              "\"PrevLoud\":\"{5}\"," +
                                              "\"PrevPace\":\"{6}\"," +
                                              "\"PrevPitch\":\"{7}\"," +
                                              "\"Comparison\":\"{8}\"," +
                                              "\"ExtraData\":\"{9}\"}}\r\n"
                            , path, taskType, assTaskId, normTaskId, choices, prevLoud, prevPace, prevPitch, comparisonPath, extraData);
                    }
                }
                else //Fluent
                {
                    json += string.Format("{{\"AudioUrls\":\"{0}\", " +
                                              "\"FeedbackQuery\":\"{1}\"}}\r\n"
                            , String.Join(",", audioPaths), result.FeedbackQuery);
                }

            }
            json = json.TrimEnd();

            return json;
        }

        private static int GetAverageRes(CrowdRowResponse resp, string dataType)
        {
            double totalScore = 0;
            int count = 0;

            foreach (CrowdJudgement judgement in resp.TaskJudgements)
            {
                foreach (CrowdJudgementData data in judgement.Data)
                {
                    if (data.DataType == dataType)
                    {
                        totalScore += data.NumResponse;
                        count++;
                    }
                }
            }

            return (int)(totalScore/count);
        }
    }
}