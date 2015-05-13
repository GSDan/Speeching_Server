using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crowd.Service.CrowdFlower
{
    public class CFOptions
    {
        public bool logical_aggregation { get; set; }
        public bool track_clones { get; set; }
        public bool include_unfinished { get; set; }
        public bool front_load { get; set; }
        public int after_gold { get; set; }
        public string mail_to { get; set; }
        public int req_ttl_in_seconds { get; set; }
    }
}