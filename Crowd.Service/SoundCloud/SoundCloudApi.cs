using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ewk.SoundCloud;
using  Ewk.SoundCloud.ApiLibrary;
using Ewk.SoundCloud.ApiLibrary.Entities;

namespace Crowd.Service.SoundCloud
{
    public class SoundCloudApi
    {
        private const string SOUNDCLOUD_CLIENT_ID = "dea4a819be164a34d73abab26cdac23c";
        private const string SOUNDCLOUD_CLIENT_SECRET = "13529e6a32ed0e437a8ac66a1e227e20";
        private const string SOUNDCLOUD_BASE_URI = "http://api.soundcloud.com/";

        public int UploadAudioFiles(string path)
        {
            using (HttpClient client = new HttpClient())
            {
                SCTrackRequest track = new SCTrackRequest()
                {
                    title = "test xyz 3",
                    asset_data = @"Z:\Developments\NCLUni\Speeching\Crowd.Service\Uploads\sc2ev1.mp3"
                };
                Ewk.SoundCloud.ApiLibrary.SoundCloud d = new Ewk.SoundCloud.ApiLibrary.SoundCloud(SOUNDCLOUD_CLIENT_ID);
                var e = d.RequestOAuthToken(new Uri("http://api.opescode.com/api/PutExternalAccessKey"), SOUNDCLOUD_CLIENT_SECRET);
                //Uri baseAddress = new Uri(SOUNDCLOUD_BASE_URI + "tracks.json?client_id=" + SOUNDCLOUD_CLIENT_ID);
                //var response = client.GetAsync(baseAddress).Result;
                //var x = response.Content.ReadAsAsync<IEnumerable<SCTrackScheme>>().Result;
                //var content = track.CreateRequestCUrlData();
                //var response2 = client.PostAsync(baseAddress, content);
                //var tracks = d.Tracks;


                //var tracks2 = d.Tracks.Get();
                ////var res = d.RequestOAuthToken(new Uri("http://www.speeching.co.uk"), SOUNDCLOUD_CLIENT_SECRET);
                //Track track = new Track();
                //track.title = "new test track 1";
                //track.asset_data = File.ReadAllBytes(@"Z:\Developments\NCLUni\Speeching\Crowd.Service\Uploads\sc2ev1.mp3");
                //var response = d.Tracks.Post(track);
                //string x = "";
                //var reqContent = track.CreateRequestCUrlData();
                //Uri baseAddress = new Uri(CROWDFLOWER_BASE_URI + "jobs.json?key=" + CROWDFLOWER_KEY);
                //var response = client.PostAsync(baseAddress, reqContent).Result;
                //if (response.IsSuccessStatusCode)
                //{
                //    var x = response.Content.ReadAsAsync<CFJobResponse>().Result;
                //}
                //else
                //{
                //    string msg = response.Content.ReadAsStringAsync().Result;
                //    throw new Exception(msg);
                //}
            }
            return 0;
        }
    }
}
