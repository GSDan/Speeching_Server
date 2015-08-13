using System;
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
        public static string CreateCFData(IEnumerable<string> audioPaths, ParticipantActivity activity, ParticipantResult result, out int count)
        {
            count = 0;
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

                        json += createUnit(path, taskType, assTaskId, normTaskId, choices, prevLoud, prevPace, prevPitch, comparisonPath, extraData);

                        count += 1;
                    }
                }
                else //Fluent
                {
                    json += string.Format("{{\"AudioUrls\":\"{0}\", " +
                                              "\"FeedbackQuery\":\"{1}\"}}\r\n"
                            , String.Join(",", audioPaths), result.FeedbackQuery);
                    count += 1;
                }

            }
            json += TestQuestions();
            json = json.TrimEnd();

            return json;
        }

        private static string createUnit(string path, string taskType, int assTaskId, int normTaskId, string choices, int prevLoud, int prevPace, int prevPitch, 
            string comparisonPath, string extraData, bool isTest = false, string testVolAnswer1 = "", string testVolAnsweReason1 = "", string testPaceAnswer2 = "", 
            string testPaceAnsweReason2 = "", string testPitchAnswer3 = "", string testPitchAnsweReason3 = "")
        {
            return string.Format("{{\"AudioUrl\":\"{0}\", " +
                                "\"TaskType\":\"{1}\"," +
                                "\"ParticipantAssessmentTaskId\":\"{2}\"," +
                                "\"ParticipantTaskId\":\"{3}\"," +
                                "\"Choices\":\"{4}\"," +
                                "\"PrevLoud\":\"{5}\"," +
                                "\"PrevPace\":\"{6}\"," +
                                "\"PrevPitch\":\"{7}\"," +
                                "\"Comparison\":\"{8}\"," +
                                "\"ExtraData\":\"{9}\","+
                                "\"_golden\":\"{10}\"," +
                                "\"rlstVolumeChange_gold\":\"{11}\"," +
                                "\"rlstVolumeChange_gold_reason\":\"{12}\"," +
                                "\"rlstPaceChange_gold\":\"{13}\"," +
                                "\"rlstPaceChange_gold_reason\":\"{14}\"," +
                                "\"rlstPitchChange_gold\":\"{15}\"," +
                                "\"rlstPitchChange_gold_reason\":\"{16}\"}}\r\n"
                                , path, taskType, assTaskId, normTaskId, choices, prevLoud, prevPace, prevPitch, comparisonPath, extraData, (isTest ? "TRUE" : ""), testVolAnswer1, testVolAnsweReason1, testPaceAnswer2, testPaceAnsweReason2, testPitchAnswer3, testPitchAnsweReason3);
        }

        private static string TestQuestions()
        {
            string retJson = string.Empty;

            //retJson += createUnit("https://openlabdata.blob.core.windows.net/speechinguploads/speeching2@gmail.com/6/748059/imgDesc_4-24.mp4", "Other", -1, 8, "", 0, 0, 0, "https://openlabdata.blob.core.windows.net/speechinguploads/speeching2@gmail.com/6/748059/imgDesc_4-24.mp4", "", true, "constant", "becasue the volume stayed the same",  "constant", "becasue the volume stayed the same",  "constant", "becasue the volume stayed the same");
            //retJson += createUnit("https://xyz2.com", "test2", -1, 8, "", 0, 0, 0, "", "", true, "increase", "becasue its is increase");
            //retJson += createUnit("https://xyz3.com", "test3", -1, 8, "", 0, 0, 0, "", "", true, "increase", "becasue its is increase");
            //retJson += createUnit("https://xyz4.com", "test4", -1, 8, "", 0, 0, 0, "", "", true, "increase", "becasue its is increase");
            //retJson += createUnit("https://xyz5.com", "test5", -1, 8, "", 0, 0, 0, "", "", true, "increase", "becasue its is increase");
            //retJson += createUnit("https://xyz6.com", "test6", -1, 8, "", 0, 0, 0, "", "", true, "increase", "becasue its is increase");
            //retJson += createUnit("https://xyz7.com", "test7", -1, 8, "", 0, 0, 0, "", "", true, "increase", "becasue its is increase");
            //retJson += createUnit("https://xyz8.com", "test8", -1, 8, "", 0, 0, 0, "", "", true, "increase", "becasue its is increase");

            return retJson;
        }

        private static int GetAverageRes(CrowdRowResponse resp, string dataType)
        {
            List<int> results = new List<int>();

            foreach (CrowdJudgement judgement in resp.TaskJudgements)
            {
                foreach (CrowdJudgementData data in judgement.Data)
                {
                    if (data.DataType == dataType)
                    {
                        results.Add(data.NumResponse);
                    }
                }
            }

            return SvcUtil.GetMedian(results) ?? default(int);
        }
    }
}