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
        internal FormUrlEncodedContent CreateRequestCUrlData(Crowd.Model.Data.User.AppType app)
        {
            List<KeyValuePair<string, string>> lstKeyValue = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("job[title]", Title),
                new KeyValuePair<string, string>("job[instructions]", Instructions),
                new KeyValuePair<string, string>("job[cml]", (app == User.AppType.Speeching) ? CreateAudioCml() : CreateFluentAudioCml()),
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
        private const string RadioParentCml = "<cml:radios name=\"rlst{0}\" label=\"{1}\" validates=\"required\">\r\n{2}\r\n</cml:radios><br/>\r\n";
        private const string RadioCml = "<cml:radio label=\"{0}\" />\r\n";
        private const string ScaleCml = "<cml:ratings name=\"rlst{0}\" label=\"{1}\" instructions=\"{2}\" points=\"{3}\"  validates=\"{4}\" />\r\n";
        private const string IntRangeCml = "<cml:text name=\"rlst{0}\" label=\"{1}\" instructions=\"{2}\" validates=\"required integerRange:{{min:{3},max:{4}}}\"/>\r\n";

        private static string CreateFluentAudioCml()
        {
            var cml = "<div class=\"grp\">\r\n";
            string audioCml = "{% assign array = AudioUrls | split: \",\" %}\r\n";
            audioCml += "{% for AudioUrl in array %}";
            audioCml += string.Format(AudioHtmlTag, "{{AudioUrl}}") + "<br/>";
            audioCml += "{% endfor %}";
            cml += audioCml;
            cml += "<br/>The user has entered the following: <br/>";
            cml += "<span style='color:blue'>{{FeedbackQuery}}</span><br/><br/>";
            cml += string.Format(
                    TextareaCml,
                    "In the box below please write your feedback.",
                    "",
                    "required");
            cml += "</div>";

            return cml;
        }

        private static string CreateAudioCml()
        {
            var cml = "<div class=\"grp\">\r\n";
            cml += string.Format(AudioHtmlTag, "{{AudioUrl}}");

            cml += "{% if TaskType == \"MP\" %}\r\n";

                cml += CreateMinimalPairsCml("Choose the word that is closest to what you can hear");

            cml += "{% else %}\r\n";

                cml += string.Format(
                    TextareaCml, 
                    "In the box below please write down exactly what you heard the person say, even if the spelling seems strange.", 
                    "If you do not understand a word at all put a question mark (?) in its place.", 
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
                "How much did the person’s accent affect how easy they were to understand?  ",
                "Please give a rating where 1 is 'Not at all' and 5 is 'Their accent was so broad I couldn’t understand a thing'",
                "required",
                5) + "<br/>";

            ratingScales += "{% if TaskType != \"MP\" %}\r\n";

            /* ************************* COMPARING ******************************/
            // If the user has already uploaded a recording of them saying this content, 
            // show the CrowdFlower user the recording and results for comparison
            ratingScales += "{% if Comparison and Comparison != \"\" %}\r\n";
#region comparisons
            ratingScales += "<br/><p>Below you will hear two sentences being spoken. " +
                            "Press the play buttons in each box (1 and then 2) to hear them separately. " +
                            "The second one is the person's previous upload - think about how the person was talking in sentence 1 compared to sentence 2. </p>";
            ratingScales += string.Format(AudioHtmlTag, "{{AudioUrl}}") + string.Format(AudioHtmlTag, "{{Comparison}}");

            ratingScales +=
                "<p>The score representing the average loudness of the person’s voice the last time " +
                "they submitted a recording for analysis was {{PrevLoud}},  where 0 is 'so quiet I could barely hear them' and 100 is 'very loud'. " +
                "We are looking to see if there is a change.</p>";

            ratingScales += CreateIntRatingBox(
                "Volume",
                "Please enter a number from 0-100 indicating how loud you felt the first sentence was, where 0 is 'so quiet I could barely hear them' and 100 is 'very loud'",
                "Please enter a number between 0-100, using the previous score given to the second recording as a comparison",
                0, 100);

            ratingScales += CreateRadios("VolumeChange", "Did the volume change over the course of the first sentence?",
                new[]
                    {
                        new []{"No, it stayed the same", "constant"},
                        new []{"It got louder as the sentence went on", "increase"},
                        new []{"It got quieter as the sentence went on", "decrease"}
                    }) + "<br/>";

            ratingScales +=
                "<p>The score representing the average speed of the person’s voice the last time " +
                "they submitted a recording for analysis was {{PrevPace}},  where 0 is 'very slow' and 100 is 'So fast I could barely understand them'. " +
                "We are looking to see if there is a change.</p>";

            ratingScales += CreateIntRatingBox(
                "Pace",
                "Please enter a number from 0-100 indicating how fast you felt the person in the first sentence was talking, where 0 is 'very slow' and 100 is 'So fast I could barely understand them'.",
                "Please enter a number between 0-100, using the previous score given to the second recording as a comparison",
                0, 100);

            ratingScales += CreateRadios("PaceChange", "Did the speed change over the course of the first sentence?",
                new[]
                    {
                        new []{"No, it stayed the same", "constant"},
                        new []{"It got faster as the sentence went on", "increase"}, 
                        new []{"It got slower as the sentence went on", "decrease"}
                    }) + "<br/>";

            ratingScales += "<p>Think about how much the person’s pitch varied in sentence 1 compared to sentence 2." +
                            "(Pitch refers to the ups and downs in a person’s voice which give it feeling. " +
                            "Someone with a varied pitch might sound excited and interested, someone with little " +
                            "change to their pitch might sound monotonous or bored)</p>";

            ratingScales +=
                "<p>The score representing how much the person's pitch varied in their voice the last time " +
                "they submitted a recording for analysis was {{PrevPace}},  where 0 is 'not at all, they spoke with a" +
                " monotonous voice and sounded bored' and 100 is 'a lot, they sounded excited and interested'. " +
                "We are looking to see if there is a change.</p>";

            ratingScales += CreateIntRatingBox(
                "Pitch",
                "Please enter a number from 0-100 indicating how much you felt the pitch in the first sentence varied, where 0 is 'is not at all, " +
                "they spoke with a monotonous voice and sounded bored' and 100 is 'a lot, they sounded excited and interested'",
                "Please enter a number between 0-100, using the previous score given to the second recording as a comparison",
                0, 100);

            ratingScales += CreateRadios("PitchChange", "Did the pitch change over the course of the sentence?",
                new[]
                    {
                        new []{"No, it stayed the same", "constant"},
                        new []{"It got more excited as the sentence went on", "increase"}, 
                        new []{"It got more bored as the sentence went on", "decrease"}
                    }) + "<br/>";
#endregion
            /* ************************* FIRST RATING ***************************/
            // There are no recordings to use as point of comparison so don't show them.

            ratingScales += "{% else %}\r\n"; // Is first time recording this content
#region firstRating
            ratingScales += CreateIntRatingBox(
                "Volume",
                "Please enter a number from 0-100 indicating how loud you felt the sentence was, where 0 is 'so quiet I could barely hear them' and 100 is 'very loud'",
                "Please enter a number between 0-100",
                0, 100);

            ratingScales += CreateRadios("VolumeChange", "Did the volume change over the course of the sentence?",
                new[]
                    {
                        new []{"No, it stayed the same", "constant"},
                        new []{"It got louder as the sentence went on", "increase"},
                        new []{"It got quieter as the sentence went on", "decrease"}
                    }) + "<br/>";

            ratingScales += CreateIntRatingBox(
                "Pace",
                "Please enter a number from 0-100 indicating how fast you felt the person was talking, where 0 is 'very slow' and 100 is 'So fast I could barely understand them'.",
                "Please enter a number between 0-100",
                0, 100);

            ratingScales += CreateRadios("PaceChange", "Did the speed change over the course of the sentence?",
                new[]
                    {
                        new []{"No, it stayed the same", "constant"},
                        new []{"It got faster as the sentence went on", "increase"}, 
                        new []{"It got slower as the sentence went on", "decrease"}
                    }) + "<br/>";

            ratingScales += "<p>Think about how much the person’s pitch varied in the recording." +
                            "(Pitch refers to the ups and downs in a person’s voice which give it feeling. " +
                            "Someone with a varied pitch might sound excited and interested, someone with little " +
                            "change to their pitch might sound monotonous or bored)</p>";

            ratingScales += CreateIntRatingBox(
                "Pitch",
                "Please enter a number from 0-100 indicating how much you felt the pitch in this sentence varied, where 0 is 'not at all, " +
                "they spoke with a monotonous voice and sounded bored' and 100 is 'a lot, they sounded excited and interested'",
                "Please enter a number between 0-100",
                0, 100);

            ratingScales += CreateRadios("PitchChange", "Did the pitch change over the course of the sentence?",
                new[]
                    {
                        new []{"No, it stayed the same", "constant"},
                        new []{"It got more excited as the sentence went on", "increase"}, 
                        new []{"It got more bored as the sentence went on", "decrease"}
                    }) + "<br/>";
#endregion
            ratingScales += "{% endif %}\r\n"; // End comparison if

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
            return string.Format(ScaleCml, dataType, desc, instructions, num, validator);
        }

        private static string CreateIntRatingBox(string dataType, string desc, string instructions, int min, int max)
        {
            return string.Format(IntRangeCml, dataType, desc, instructions, min, max);
        }

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

        private static string CreateRadios(string dataType, string label, IEnumerable<string[]> args)
        {
            string innerRadios = "";

            foreach (string[] option in args)
            {
                innerRadios += "<cml:radio label=\""+option[0]+"\" value=\""+option[1]+"\"/>";
            }

            return string.Format(RadioParentCml, dataType, label, innerRadios);
        }

        /// <summary>
        /// Create a list of radio buttons
        /// </summary>
        /// <param name="label"></param>
        /// <param name="validator"></param>
        /// <returns></returns>
        private static string CreateMinimalPairsCml(string label)
        {
            string toRet = "{% assign array = Choices | split: \",\" %}\r\n";

            string inner = "{% for choice in array %}";

            inner += string.Format(RadioCml, "{{choice}}");

            inner += "{% endfor %}";

            inner += string.Format(RadioCml, "None of the above");

            toRet += string.Format(RadioParentCml, "MP", label, inner);

            return toRet;
        }

        [Flags]
        internal enum CFWorkForce { OnDemand = 1, Internally = 2 }

        internal FormUrlEncodedContent LaunchRequestCUrlData(CFWorkForce workforce, int unitCount)
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();

            if (workforce == CFWorkForce.OnDemand)
            {
                list.Add(new KeyValuePair<string, string>("channels[0]", "on_demand"));
            }
            else if (workforce == CFWorkForce.Internally)
            {
                list.Add(new KeyValuePair<string, string>("channels[" + list.Count + "]", "cf_internal"));
            }

            list.Add(new KeyValuePair<string, string>("debit[units_count]", unitCount.ToString()));

            return new FormUrlEncodedContent(list);
        }

        internal async Task<SvcStatus> CreateAudioJob(ParticipantResult result, ParticipantActivity activity)
        {
            SvcStatus status = new SvcStatus();
            try
            {
                string userKey = result.User.Email;

                using (HttpClient client = new HttpClient())
                {
                    FormUrlEncodedContent reqContent = CreateRequestCUrlData(result.User.App);
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