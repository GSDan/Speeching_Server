using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Crowd.Model.Data;

namespace Crowd.Service.CrowdFlower
{
    public class CFUnitResponse
    {
        public class CFUnitItem
        {
            public string AudioUrl;
            public string AudioTypeCodec;
        }

        public Dictionary<int, CFUnitItem> ReturnedUnits { get; set; }

        private List<CrowdRowResponse> _units;

        public List<CrowdRowResponse> ProcessedUnits
        {
            get
            {
                if (_units != null) return _units;

                _units = new List<CrowdRowResponse>();

                foreach (KeyValuePair<int, CFUnitItem> unit in ReturnedUnits)
                {
                    CrowdRowResponse res = new CrowdRowResponse
                    {
                        Id = unit.Key.ToString(),
                        CreatedAt = DateTime.Now,
                        ParticipantTaskId = int.Parse(Path.GetFileNameWithoutExtension(unit.Value.AudioUrl))
                    };
                    _units.Add(res);
                }

                return _units;
            }
            
        }
    }
}