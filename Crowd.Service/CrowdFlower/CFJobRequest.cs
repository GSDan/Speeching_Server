using Crowd.Service.SoundCloud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using Crowd.Model.Data;
using Crowd.Service.Common;
using Crowd.Service.Model;

namespace Crowd.Service.CrowdFlower
{
    public class CFJobRequest
    {
        private const string CrowdflowerBaseUri = "https://api.crowdflower.com/v1/";
        private const string CrowdflowerKey = "yvQuiDN7zkaRQH8uhfnn";
        private const string Mp4TypeCodec = "audio/mp4; codec='mp4a.40.2'";

        public string ResourceUrl { get; set; }
        public List<CrowdRowResponse> Rows;

        public string Cml { get; set; }
        public string Css { get; set; }
        public string Instructions { get; set; }
        public int JudgmentsPerUnit { get; set; }
        public int PaymentCents { get; set; }
        public string SupportEmail { get; set; }
        public string Title { get; set; }
        public int UnitsPerAssignment { get; set; }
        public string WebhookUri { get; set; }

        //Create the HttpContent for the form
        internal FormUrlEncodedContent CreateRequestCUrlData()
        {
            List<KeyValuePair<string, string>> lstKeyValue = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("job[title]", Title),
                new KeyValuePair<string, string>("job[instructions]", Instructions),
                new KeyValuePair<string, string>("job[cml]", CreateAudioCml()),
                new KeyValuePair<string, string>("job[css]", Css),
                new KeyValuePair<string, string>("job[webhook_uri]", WebhookUri),
                new KeyValuePair<string, string>("job[support_email]", SupportEmail),
                new KeyValuePair<string, string>("job[payment_cents]", PaymentCents.ToString()),
                new KeyValuePair<string, string>("job[units_per_assignment]", UnitsPerAssignment.ToString())
            };

            return new FormUrlEncodedContent(lstKeyValue);
        }

        private const string AudioHtmlTag = "<audio src=\"{{AudioUrl}}\" type=\"{{AudioTypeCodec}}\" preload=\"auto\" controls=\"controls\"></audio>";
        private const string TextareaCml = "<cml:textarea name=\"txta\" label=\"{0}\" class=\"\" instructions=\"{1}\" validates=\"{2}\"/>";
        private const string GroupCml = "<cml:group>{0}</cml:group>";
        private const string RadioParentCml = "<cml:radios name=\"rlst{0}\" label=\"{1}\" validates=\"{2}\">{3}</cml:radios>";
        private const string RadioCml = "<cml:radio label=\"{0}\" />";
        private const string RatingCml = "<cml:ratings name=\"rlst{0}\" label=\"{1}\" points=\"{2}\" validates=\"{3}\" />";

        private static string CreateAudioCml()
        {
            var cml = "<div class=\"grp\">";
            cml += AudioHtmlTag;

            cml += "{% if TaskType == \"MP\" %}\r\n";
            cml += CreateMinimalPairsCml("Which of these words was the one spoken?", 12, "required");
            cml += "{% else %}\r\n";
            cml += string.Format(TextareaCml, "Transcribe the above audio", "Please listen to the audio clip and transcribe it as accurately as you can.", "required");
            cml += "{% endif %}";

            string ratingScales = CreateRatingScale("Trans", "How would you rate the ease of listening for this clip?", "required", 5);

            ratingScales += CreateRatingScale("Accent", "How much more difficult did the person's accent make the task?", "required", 5);

            cml += string.Format(GroupCml, ratingScales) + "</div>";

            return cml;
        }

        /// <summary>
        /// Creates the CML for a ratings scale
        /// </summary>
        /// <param name="validator"></param>
        /// <param name="num">Number of radio options</param>
        /// <param name="dataType"></param>
        /// <param name="desc"></param>
        /// <returns>CML string</returns>
        private static string CreateRatingScale(string dataType, string desc, string validator, int num)
        {
            return string.Format(RatingCml, dataType, desc, num, validator);
        }

        /// <summary>
        /// Create a list of radio buttons
        /// </summary>
        /// <param name="label"></param>
        /// <param name="numChoices"></param>
        /// <param name="validator"></param>
        /// <returns></returns>
        private static string CreateMinimalPairsCml(string label, int numChoices, string validator)
        {
            string toRet = "";

            for(int i = 0; i < numChoices; i++)
            {
                toRet += string.Format(RadioCml, string.Format("{{{{choice{0}}}}}", i + 1));
            }
            toRet += string.Format(RadioCml, "None of the above");

            return string.Format(RadioParentCml, "MP", label, validator, toRet);
        }

        [Flags]
        internal enum CFWorkForce { OnDemand = 1, Internally = 2 }

        internal FormUrlEncodedContent LaunchRequestCUrlData(CFWorkForce workforce, int unitCount)
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();

            if ((workforce & CFWorkForce.OnDemand) != 0)
            {
                list.Add(new KeyValuePair<string, string>("channels[0]", "on_demand"));
            }

            if ((workforce & CFWorkForce.Internally) != 0)
            {
                list.Add(new KeyValuePair<string, string>("channels[" + list.Count + "]", "on_demand"));
            }

            list.Add(new KeyValuePair<string, string>("debit[units_count]", unitCount.ToString()));

            return new FormUrlEncodedContent(list);
        }

        internal async Task<SvcStatus> CreateAudioJob(ParticipantResult result, ParticipantActivity activity)
        {
            SvcStatus status = new SvcStatus();
            try
            {
                string userKey = result.User.Key.ToString();

                using (HttpClient client = new HttpClient())
                {
                    FormUrlEncodedContent reqContent = CreateRequestCUrlData();
                    Uri baseAddress = new Uri(CrowdflowerBaseUri + "jobs.json?key=" + CrowdflowerKey);
                    HttpResponseMessage response = client.PostAsync(baseAddress, reqContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        CFJobResponse jobRes = await response.Content.ReadAsAsync<CFJobResponse>();
                        List<string> audioFiles = await SvcUtil.DownloadAndExtractZip(ResourceUrl, jobRes.id.ToString(), userKey, activity.Id.ToString());

                        if (!audioFiles.Any()) return status;

                        CFUnitRequest unitsReq = new CFUnitRequest();
                        string uploadUnitsJson = AudioUnit.CreateCFData(audioFiles, Mp4TypeCodec, activity, result);

                        Rows = await unitsReq.SendCfUnits(jobRes.id, uploadUnitsJson);

                        foreach (CrowdRowResponse unit in Rows)
                        {
                            unit.ParticipantResultId = result.Id;
                        }

                        status = new SvcStatus()
                        {
                            Level = 0,
                            Description = "Job created",
                            Response = response,
                            CreatedRows = Rows
                        };
                    }
                    else
                    {
                        status = new SvcStatus() { Level = 2, Description = "Failed", Response = response };
                    }
                }
            }
            catch (Exception e)
            {
                status = new SvcStatus() { Level = 2, Description = e.Message, Response = new HttpResponseMessage(HttpStatusCode.InternalServerError) };
            }
            
            return status;
        }

        internal static SvcStatus LaunchJob(int jobId)
        {
            SvcStatus status;

            if (jobId > 0)
            {
                Uri baseAddress = new Uri(string.Format("{0}jobs/{1}/orders.json?key={2}", CrowdflowerBaseUri, jobId, CrowdflowerKey));

                using (HttpClient client = new HttpClient())
                {
                    CFJobRequest job = new CFJobRequest();

                    //TODO: what is unitCount? I use 20 for now
                    var response = client.PostAsync(baseAddress, job.LaunchRequestCUrlData(CFWorkForce.Internally, 20)).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        status = new SvcStatus() { Level = 0, Description = "Job launched", Response = response };
                    }
                    else
                    {
                        status = new SvcStatus()
                        {
                            Level = 2,
                            Description = "Failed to launch",
                            Response = response
                        };
                    }
                }
            }
            else
            {
                status = new SvcStatus()
                {
                    Level = 2,
                    Description = "Job Key must be greater than 0"
                };
            }
            return status;
        }
    }
}