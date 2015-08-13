using System;
using System.Net.Http;
using System.Threading.Tasks;
using Crowd.Model.Data;
using Crowd.Service.Common;
using System.Collections.Generic;

namespace Crowd.Service.CrowdFlower
{
    class CrowdFlowerApi
    {
        private const string CrowdflowerBaseUri = "https://api.crowdflower.com/v1/";

        internal static async Task<SvcStatus> CreateJob(ParticipantResult participantResult, ParticipantActivity activity)
        {
            CFJobRequest job = new CFJobRequest()
                {
                    Title = (participantResult.User.App == Crowd.Model.Data.User.AppType.Speeching) ? "Speeching " : "Fluent" + participantResult.Id,
                    Instructions = "Please listen to the audio clip(s) and complete the following tasks, making sure that your computer's volume is loud enough to hear the voice samples clearly",
                    PaymentCents = 5,
                    UnitsPerAssignment = 2,
                    Css = "div.grp { border: solid thin gray; margin: 10px 0; }",
                    WebhookUri = ConfidentialData.ApiUrl + "/api/CFWebhook/",
                    SupportEmail = ConfidentialData.SupportEmail,
                    ResourceUrl = participantResult.ResourceUrl
                };
            return await job.CreateAudioJob(participantResult, activity);
        }

        internal static SvcStatus LaunchJob(int jobId, int unitCount)
        {
            SvcStatus status;

            if (jobId > 0)
            {
                using (HttpClient client = new HttpClient())
                {
                    CFJobRequest job = new CFJobRequest();
                    var incExcCountStatus = job.ControlQuality(jobId);
                    if (incExcCountStatus.Result.Response.IsSuccessStatusCode)
                    {
                        var baseAddress = new Uri(string.Format("{0}jobs/{1}/orders.json?key={2}",
                             CrowdflowerBaseUri, jobId, ConfidentialData.CrowdFlowerKey));
                        var response = client.PostAsync(baseAddress, job.LaunchRequestCUrlData(CFJobRequest.CFWorkForce.OnDemand, unitCount)).Result;

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
                    else
                    {
                        return incExcCountStatus.Result;
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