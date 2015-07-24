using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Crowd.Model.Data;

namespace Crowd.Service.Common
{
    public class SvcStatus
    {
        /// <summary>
        /// 0 = success
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 0 = info, 1 = warning, 2 = error
        /// </summary>
        public int Level { get; set; }
        public string Description { get; set;}
        public HttpResponseMessage Response { get; set; }
        public List<CrowdRowResponse> CreatedRows;
    }
}