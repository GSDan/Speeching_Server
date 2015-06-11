using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Threading.Tasks;
using Crowd.Model.Data;
using Newtonsoft.Json;

namespace Crowd.Service.CrowdFlower
{
    public class CFUnitRequest
    {
        private const string CrowdflowerBaseUri = "https://api.crowdflower.com/v1/";
        private const string CrowdflowerKey = "yvQuiDN7zkaRQH8uhfnn";

        internal async Task<List<CrowdRowResponse>> SendCfUnits(int jobId, string uploadUnitsJson)
        {
            if (jobId <= 0 || string.IsNullOrWhiteSpace(uploadUnitsJson)) return null;

            // Send the units (rows) to crowdflower to get added to the job
            using (var client = new HttpClient())
            {
                var baseAddress = new Uri(string.Format("{0}jobs/{1}/upload.json?key={2}&force=true",
                    CrowdflowerBaseUri, jobId, CrowdflowerKey));

                var reqContent = new StringContent(uploadUnitsJson);
                reqContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(baseAddress, reqContent);

                if (!response.IsSuccessStatusCode) return null;
            }

            // Get the created rows back so we can keep the IDs (not included in the response to the above for some reason)
            using (HttpClient client = new HttpClient())
            {
                Uri baseUri = new Uri(CrowdflowerBaseUri + "jobs/" + jobId + "/units.json?key=" + CrowdflowerKey + "&page=1");
                HttpResponseMessage response = await client.GetAsync(baseUri);

                string content = await response.Content.ReadAsStringAsync();

                CFUnitResponse unitData = new CFUnitResponse
                {
                    ReturnedUnits = JsonConvert.DeserializeObject<Dictionary<int, CFUnitResponse.CFUnitItem>>(content)
                };

                return unitData.ProcessedUnits;
            }
        }
    }
}