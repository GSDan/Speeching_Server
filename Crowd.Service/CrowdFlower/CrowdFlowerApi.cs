using System;
using System.Net.Http;
using System.Threading.Tasks;
using Crowd.Model.Data;
using Crowd.Service.Common;

namespace Crowd.Service.CrowdFlower
{
    class CrowdFlowerApi
    {
        private const string CrowdflowerBaseUri = "https://api.crowdflower.com/v1/";

        internal static async Task<SvcStatus> CreateJob(ParticipantResult participantResult, ParticipantActivity activity)
        {
            CFJobRequest job = new CFJobRequest()
                {
                    Title = "Speeching " + participantResult.Id,
                    Instructions = "Please listen to the audio clip and complete the following tasks, making sure that your computer's volume is loud enough to hear the voice samples clearly",
                    PaymentCents = 5,
                    UnitsPerAssignment = 2,
                    Css = "div.grp { border: solid thin gray; margin: 10px 0; }",
                    WebhookUri = ConfidentialData.ApiUrl + "/api/CFWebhook/",
                    SupportEmail = "dan.richardson@newcastle.ac.uk",
                    ResourceUrl = participantResult.ResourceUrl
                };

            return await job.CreateAudioJob(participantResult, activity);;
        }

        internal static SvcStatus LaunchJob(int jobId)
        {
            SvcStatus status;

            if (jobId > 0)
            {
                var baseAddress = new Uri(string.Format("{0}jobs/{1}/orders.json?key={2}",
                    CrowdflowerBaseUri, jobId, ConfidentialData.CrowdFlowerKey));
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

        internal static SvcStatus JobQualitySettings(int jobId)
        {
            SvcStatus status = new SvcStatus();



            return status;
        }
    }
}