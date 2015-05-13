using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Crowd.Service.Common;
using Crowd.Service.Model;

namespace Crowd.Service.CrowdFlower
{
    public class CFUnitRequest
    {
        private const string CROWDFLOWER_BASE_URI = "https://api.crowdflower.com/v1/";
        private const string CROWDFLOWER_KEY = "yvQuiDN7zkaRQH8uhfnn";
        private const string MP4_TYPE_CODEC = "audio/mp4; codec='mp4a.40.2'";

        internal SvcStatus UploadUnits(int jobId, string uploadUnitsJson)
        {
            SvcStatus status = new SvcStatus();
            if (jobId > 0 && !String.IsNullOrWhiteSpace(uploadUnitsJson))
            {
                using (var client = new HttpClient())
                {
                        var baseAddress = new Uri(String.Format("{0}jobs/{1}/upload.json?key={2}&force=true",
                            CROWDFLOWER_BASE_URI, jobId, CROWDFLOWER_KEY));
                        var reqContent = new StringContent(uploadUnitsJson);
                        reqContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        var response = client.PostAsync(baseAddress, reqContent).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            status = new SvcStatus() { Level = 0, Description = "Units uploaded", Response = response };
                        }
                        else
                        {
                            status = new SvcStatus() { Level = 2, Description = "Failed", Response = response };
                        }
                }
            }
            else
                status = new SvcStatus() { Level = 2, Description = "No ParticipantResult found" };
            return status;
        }
    }
}