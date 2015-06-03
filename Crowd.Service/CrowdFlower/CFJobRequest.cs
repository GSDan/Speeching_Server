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
        private const string CROWDFLOWER_BASE_URI = "https://api.crowdflower.com/v1/";
        private const string CROWDFLOWER_KEY = "yvQuiDN7zkaRQH8uhfnn";
        private const string SERVICE_UPLOAD_URL = "http://api.opescode.com/uploads/";
        private const string MP4_TYPE_CODEC = "audio/mp4; codec='mp4a.40.2'";
        private const string AUDIO_HTML_TAG = "<audio src=\"{{AudioUrl}}\" type=\"{{AudioTypeCodec}}\" preload=\"auto\" controls=\"controls\"></audio>";
        private const string TEXTAREA_CML =
            "<cml:textarea name=\"txta\" label=\"{0}\" class=\"\" instructions=\"{1}\" validates=\"{2}\"/>";

        private const string GROUP_CML = "<cml:group only-if=\"txta\">{0}</cml:group>";
        private const string RADIOS_CML = "<cml:radios name=\"rlst{0}\" label=\"{1}\" validates=\"{2}\">{3}</cml:radios>";
        private const string RADIO_CML = "<cml:radio label=\"{0}\" />";

        public string resourceUrl { get; set; }
        public List<CrowdRowResponse> rows;

        public int after_gold { get; set; }
        public string alias { get; set; }
        public int auto_order_threshold { get; set; }
        public int auto_order_timeout { get; set; }
        public bool auto_order { get; set; }
        public string cml { get; set; }
        public string css { get; set; }
        public int expected_judgments_per_unit { get; set; }
        public int gold_per_assignment { get; set; }
        public bool include_unfinished { get; set; }
        public string instructions { get; set; }
        public string js { get; set; }
        public int judgments_per_unit { get; set; }
        public int max_judgments_per_unit { get; set; }
        public int min_unit_confidence { get; set; }
        public int payment_cents { get; set; }
        public string problem { get; set; }
        public string support_email { get; set; }
        public string title { get; set; }
        public int units_per_assignment { get; set; }
        public bool units_remain_finalized { get; set; }
        public string uri { get; set; }
        public string variable_judgments_mode { get; set; }
        public string webhook_uri { get; set; }
        
        //Create the HttpContent for the form
        internal FormUrlEncodedContent CreateRequestCUrlData()
        {
            List<KeyValuePair<string, string>> lstKeyValue = new List<KeyValuePair<string, string>>();

            foreach (PropertyInfo property in typeof(CFJobRequest).GetProperties())
            {
                if (property.CanRead && " title instructions cml css webhook_uri support_email payment_cents units_per_assignment ".IndexOf(" " + property.Name + " ") >= 0)
                {
                    lstKeyValue.Add(new KeyValuePair<string, string>("job[" + property.Name + "]", property.GetValue(this) + ""));
                }
            }

            return new FormUrlEncodedContent(lstKeyValue);
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

        internal async Task<SvcStatus> CreateAudioJob(ParticipantResult result)
        {
            SvcStatus status = new SvcStatus();
            try
            {
                string userKey = result.User.Key.ToString();
                ParticipantActivity activity = result.ParticipantActivity;

                using (HttpClient client = new HttpClient())
                {
                    var reqContent = CreateRequestCUrlData();
                    Uri baseAddress = new Uri(CROWDFLOWER_BASE_URI + "jobs.json?key=" + CROWDFLOWER_KEY);
                    var response = client.PostAsync(baseAddress, reqContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        CFJobResponse jobRes = await response.Content.ReadAsAsync<CFJobResponse>();
                        var lstAudioFiles = await SvcUtil.DownloadAndExtractZip(resourceUrl, jobRes.id.ToString(), userKey, activity.Id.ToString());

                        if (!lstAudioFiles.Any()) return status;

                        CFUnitRequest unitsReq = new CFUnitRequest();
                        string uploadUnitsJson = AudioUnit.CreateCFUploadUnits(lstAudioFiles, MP4_TYPE_CODEC);

                        rows = await unitsReq.UploadUnits(jobRes.id, uploadUnitsJson);

                        foreach (CrowdRowResponse unit in rows)
                        {
                            unit.ParticipantResultId = result.Id;
                        }

                        status = new SvcStatus()
                        {
                            Level = 0,
                            Description = "Job created and launched",
                            Response = response,
                            CreatedRows = rows
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
            var status = new SvcStatus();
            if (jobId > 0)
            {
                var baseAddress = new Uri(String.Format("{0}jobs/{1}/orders.json?key={2}",
                    CROWDFLOWER_BASE_URI, jobId, CROWDFLOWER_KEY));
                using (HttpClient client = new HttpClient())
                {
                    CFJobRequest job = new CFJobRequest();
                    //TODO: what is unitCount? I use 20 for now
                    var response =
                        client.PostAsync(baseAddress, job.LaunchRequestCUrlData(CFJobRequest.CFWorkForce.Internally, 20))
                            .Result;
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