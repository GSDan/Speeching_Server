using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using Crowd.Service.Common;
using Newtonsoft.Json;

namespace Crowd.Service.Model
{
    public class AudioUnit
    {
        public string AudioUrl { get; set; }
        public string AudioTypeCodec { get; set; }
        public int Id { get; set; }

        public AudioUnit()
        {
        }

        public AudioUnit(int id, string audioUrl, string audioTypeCodec)
        {
            this.AudioUrl = audioUrl;
            this.AudioTypeCodec = audioTypeCodec;
            this.Id = id;
        }

        public static string CreateCFUploadUnits(IEnumerable<string> audioPaths, string audioTypeCodec)
        {
            List<KeyValuePair<string,string>> lst = new List<KeyValuePair<string,string>>();
            string json = "";
            int count = 0;
            foreach (var path in audioPaths)
            {
                var audioUrl = SvcUtil.GetAudioUrlFromPath(path);
                json += String.Format("{{\"AudioUrl\":\"{0}\", \"AudioTypeCodec\":\"{1}\"}}\r\n", audioUrl, audioTypeCodec);
                count += 1;
            }
            json = json.TrimEnd();
            //var retLst = audioPaths.Select(path => new KeyValuePair<string, string>("unit[data][AudioUrl]", jss)));
            //jss = JsonConvert.SerializeObject(jss);

            return json;
            //return new StringContent(jss, Encoding.UTF8, "application/json");
        }
    }
}