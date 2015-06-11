using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Crowd.Model.Data;
using Crowd.Service.Common;
using Crowd.Service.Model;

namespace Crowd.Service.CrowdFlower
{
    public class CFJobRequest
    {
        private const string CrowdflowerBaseUri = "https://api.crowdflower.com/v1/";
        private const string CrowdflowerKey = "yvQuiDN7zkaRQH8uhfnn";

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

        private const string AudioHtmlTag = "<audio src=\"{0}\" type=\"audio/mp4; codec='mp4a.40.2'\" preload=\"auto\" controls=\"controls\">\r\n</audio>\r\n";
        private const string TextareaCml = "<cml:textarea name=\"txta\" label=\"{0}\" class=\"\" instructions=\"{1}\" validates=\"{2}\"/><br/>\r\n";
        private const string GroupCml = "<cml:group>\r\n{0}\r\n</cml:group>\r\n";
        private const string RadioParentCml = "<cml:radios name=\"rlst{0}\" label=\"{1}\" validates=\"{2}\">\r\n{3}\r\n</cml:radios><br/>\r\n";
        private const string RadioCml = "<cml:radio label=\"{0}\" />\r\n";
        private const string RatingCml = "<cml:ratings name=\"rlst{0}\" label=\"{1}\" instructions=\"{2}\" points=\"{3}\"  validates=\"{4}\" />\r\n";

        private static string CreateAudioCml()
        {
            var cml = "<div class=\"grp\">\r\n";
            cml += string.Format(AudioHtmlTag, "{{AudioUrl}}");

            cml += "{% if TaskType == \"MP\" %}\r\n";
                cml += CreateMinimalPairsCml("Choose the word which is closest to what you heard:", "required");
            cml += "{% else %}\r\n";
                cml += string.Format(
                    TextareaCml, 
                    "Please write down exactly what you heard the person say.", 
                    "Please write exactly what you heard the person say, even if the spelling seems strange! " +
                        "If you do not understand a word at all, put a question mark (?) in its place.", 
                    "required");
            cml += "{% endif %}\r\n";

            string ratingScales = CreateRatingScale(
                "Trans",
                "How hard was it to understand the person in this clip?",
                "Please give a rating where 1 is 'I understood everything' and 5 is 'I couldn’t understand a thing they said'",
                "required",
                5) + "<br/>";

            ratingScales += CreateRatingScale(
                "Accent", 
                "How much did the person's accent affect how easy they were to understand?",
                "Please give a rating where 1 is 'Their accent wasn't an issue' and 5 is 'Their accent was so broad I couldn't understand a thing'",
                "required",
                5) + "<br/>";

            cml += "{% if TaskType != \"MP\" %}\r\n";

                ratingScales += CreateRatingScale(
                    "Volume",
                    "How loud do you feel the person was speaking?",
                    "Please give a rating where 1 is 'Could barely hear them' and 5 is 'Very loud'",
                    "required",
                    5);

                ratingScales += CreateDropdown("VolumeChange", "Did the volume change over the course of the sentence?",
                    new[]
                    {
                        new []{"The volume stayed constant", "constant"},
                        new []{"The volume got louder as the sentence went on", "increase"},
                        new []{"The volume got quieter as the sentence went on", "decrease"}
                    }) + "<br/>";

                ratingScales += CreateRatingScale(
                    "Pace",
                    "How fast do you feel the person was talking?",
                    "Please give a rating where 1 is 'Very slow' and 5 is 'So fast I could barely understand them'",
                    "required",
                    5);

                ratingScales += CreateDropdown("PaceChange", "Did the speed change over the course of the sentence?",
                    new[]
                    {
                        new []{"The speed stayed constant", "constant"},
                        new []{"The speech got faster", "increase"}, 
                        new []{"The speech got slower", "decrease"}
                    }) + "<br/>";

                ratingScales += CreateRatingScale(
                    "Pitch",
                    "How much do you feel the person's pitch varied?",
                    "Pitch refers to how much the person's voice goes up and down.\n" +
                        "Please give a rating where 1 is 'None, very monotonous and sounded bored' and 5 is 'A lot, sounded very excited'",
                    "required",
                    5);

                ratingScales += CreateDropdown("PitchChange", "Did the pitch change over the course of the sentence?",
                    new[]
                    {
                        new []{"The pitch stayed constant", "constant"},
                        new []{"The pitch got more excited", "increase"}, 
                        new []{"The pitch got more bored sounding", "decrease"}
                    }) + "<br/>";

                ratingScales += "{% if Comparison and Comparison != \"\" %}\r\n";
                    ratingScales += "<br/><p>Please listen to this second recording and compare it to the first one:</p>";
                    ratingScales += string.Format(AudioHtmlTag, "{{Comparison}}");

                    ratingScales += CreateRatingScale(
                    "Volume2",
                    "How loud do you feel the person was speaking in the second recording?",
                    "Please give a rating where 1 is 'Could barely hear them' and 5 is 'Very loud'",
                    "required",
                    5);

                    ratingScales += CreateDropdown("VolumeChange2", "Did the volume change over the course of the sentence?",
                        new[]
                    {
                        new []{"The volume stayed constant", "constant"},
                        new []{"The volume got louder as the sentence went on", "increase"},
                        new []{"The volume got quieter as the sentence went on", "decrease"}
                    }) + "<br/>";

                    ratingScales += CreateRatingScale(
                        "Pace2",
                        "How fast do you feel the person was talking in the second recording?",
                        "Please give a rating where 1 is 'Very slow' and 5 is 'So fast I could barely understand them'",
                        "required",
                        5);

                    ratingScales += CreateDropdown("PaceChange2", "Did the speed change over the course of the sentence?",
                        new[]
                    {
                        new []{"The speed stayed constant", "constant"},
                        new []{"The speech got faster", "increase"}, 
                        new []{"The speech got slower", "decrease"}
                    }) + "<br/>";

                    ratingScales += CreateRatingScale(
                        "Pitch2",
                        "How much do you feel the person's pitch varied in the second recording?",
                        "Pitch refers to how much the person's voice goes up and down.\n" +
                            "Please give a rating where 1 is 'None, very monotonous and sounded bored' and 5 is 'A lot, sounded very excited'",
                        "required",
                        5);

                    ratingScales += CreateDropdown("PitchChange2", "Did the pitch change over the course of the sentence?",
                        new[]
                    {
                        new []{"The pitch stayed constant", "constant"},
                        new []{"The pitch got more excited", "increase"}, 
                        new []{"The pitch got more bored sounding", "decrease"}
                    }) + "<br/>";

              ratingScales += "{% endif %}\r\n"; // End comparison

            ratingScales += "{% endif %}\r\n"; // End != minimal pairs

            cml += string.Format(GroupCml, ratingScales);

            cml+= "</div>";

            return cml;
        }

        /// <summary>
        /// Creates the CML for a ratings scale
        /// </summary>
        /// <param name="instructions"></param>
        /// <param name="validator"></param>
        /// <param name="num">Number of radio options</param>
        /// <param name="dataType"></param>
        /// <param name="desc"></param>
        /// <returns>CML string</returns>
        private static string CreateRatingScale(string dataType, string desc, string instructions, string validator, int num)
        {
            return string.Format(RatingCml, dataType, desc, instructions, num, validator);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="label"></param>
        /// <param name="args">[dropdown option: [label, value]]</param>
        /// <returns></returns>
        private static string CreateDropdown(string dataType, string label, IEnumerable<string[]> args)
        {
            string toRet = "<cml:select label=\"" + label + " \" name=\""+ dataType +"\"> ";

            foreach (string[] option in args)
            {
                toRet += "<cml:option label=\""+ option[0] +"\" value=\""+ option[1] +"\" />\n";
            }

            toRet += "</cml:select>";
            return toRet;
        }

        /// <summary>
        /// Create a list of radio buttons
        /// </summary>
        /// <param name="label"></param>
        /// <param name="validator"></param>
        /// <returns></returns>
        private static string CreateMinimalPairsCml(string label, string validator)
        {
            string toRet = "{% assign array = Choices | split: \",\" %}\r\n";

            string inner = "{% for choice in array %}";

            inner += string.Format(RadioCml, "{{choice}}");

            inner += "{% endfor %}";

            inner += string.Format(RadioCml, "None of the above");

            toRet += string.Format(RadioParentCml, "MP", label, validator, inner);

            return toRet;
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
                        string uploadUnitsJson = AudioUnit.CreateCFData(audioFiles, activity, result);

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