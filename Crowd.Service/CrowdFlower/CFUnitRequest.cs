using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Crowd.Service.Common;
using Crowd.Service.Model;
using System.Threading.Tasks;
using Crowd.Model.Data;
using Newtonsoft.Json;

namespace Crowd.Service.CrowdFlower
{
    public class CFUnitRequest
    {
        private const string CROWDFLOWER_BASE_URI = "https://api.crowdflower.com/v1/";
        private const string CROWDFLOWER_KEY = "yvQuiDN7zkaRQH8uhfnn";
        private const string MP4_TYPE_CODEC = "audio/mp4; codec='mp4a.40.2'";

        internal async Task<List<CrowdRowResponse>> UploadUnits(int jobId, string uploadUnitsJson)
        {
            if (jobId <= 0 || string.IsNullOrWhiteSpace(uploadUnitsJson)) return null;

            // Send the units (rows) to crowdflower to get added to the job
            using (var client = new HttpClient())
            {
                var baseAddress = new Uri(string.Format("{0}jobs/{1}/upload.json?key={2}&force=true",
                    CROWDFLOWER_BASE_URI, jobId, CROWDFLOWER_KEY));

                var reqContent = new StringContent(uploadUnitsJson);
                reqContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(baseAddress, reqContent);

                if (!response.IsSuccessStatusCode) return null;
            }

            // Get the created rows back so we can keep the IDs (not included in the response to the above for some reason)
            using (HttpClient client = new HttpClient())
            {
                Uri baseUri = new Uri(CROWDFLOWER_BASE_URI + "jobs/" + jobId + "/units.json?key=" + CROWDFLOWER_KEY + "&page=1");
                HttpResponseMessage response = await client.GetAsync(baseUri);

                CFUnitResponse unitData = new CFUnitResponse
                {
                    ReturnedUnits = JsonConvert.DeserializeObject<Dictionary<int, CFUnitResponse.CFUnitItem>>(
                        await response.Content.ReadAsStringAsync())
                };

                return unitData.ProcessedUnits;
            }
        }
    }
}