using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using Crowd.Service.CrowdFlower;

namespace Crowd.Service.SoundCloud
{
    public class SCTrackRequest
    {
        public string title { get; set; }
        public string asset_data { get; set; }

        //Create the HttpContent for the form
        public FormUrlEncodedContent CreateRequestCUrlData()
        {
            var lstKeyValue = new List<KeyValuePair<string, string>>();
            foreach (PropertyInfo pi in typeof(SCTrackRequest).GetProperties())
            {
                if (pi.CanRead /*&& (pi.Name == "title" || pi.Name == "instructions" || pi.Name == "cml")*/)
                {
                    lstKeyValue.Add(new KeyValuePair<string, string>("track[" + pi.Name + "]", pi.GetValue(this) + ""));
                }
            }
            return new FormUrlEncodedContent(lstKeyValue);

        }
    }
}