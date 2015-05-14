using Crowd.Model.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crowd.Service.CrowdFlower
{
    public class CFWebhook
    {
        private string _json;

        public string Signal { get; set; }

        /*[JsonProperty(PropertyName = "payload")]
        private string JsonPayload
        {
            get
            {
                return _json;
            }
            set
            {
                // This property comes in as a serialized string, so have to deserialize it separately
                _json = value;
                Payload = JsonConvert.DeserializeObject<CFResponseData>(value);
            }
        }*/
        public string signature { get; set; }

        public CFResponseData Payload { get; set; }

        /// <summary>
        /// Convert the 
        /// </summary>
        /// <returns></returns>
        public List<CrowdTaskResponse> GetResponses(ParticipantTask originalTask)
        {
            List<CrowdTaskResponse> responses = new List<CrowdTaskResponse>();

            foreach(Judgement judgement in Payload.results.judgments)
            {
                CrowdTaskResponse thisResp = new CrowdTaskResponse()
                {
                    Id = judgement.id.ToString(),
                    Created_at = DateTime.Now,
                    Tainted = judgement.tainted,
                    Country = judgement.country,
                    City = judgement.city,
                    JobId = Payload.job_id,
                    WorkerId = judgement.worker_id,
                    Trust = judgement.trust,
                    ParticipantTask = originalTask,
                    ParticipantTaskId = originalTask.Id,
                    Data = judgement.data
                };
                responses.Add(thisResp);
            }

            return responses;
        }
    }

    public class CFResponseData
    {
        public int id { get; set; }
        public UnitData data { get; set; }
        public int difficulty { get; set; }
        public int judgments_count { get; set; }
        public string state { get; set; }
        public double agreement { get; set; }
        public int missed_count { get; set; }
        public object gold_pool { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public int job_id { get; set; }
        public Results results { get; set; }
    }

    public class Results
    {
        public List<Judgement> judgments { get; set; }
        public string rlsttrans { get; set; }
        public string rlstaccent { get; set; }
    }

    public class Judgement
    {
        public int id { get; set; }
        public string created_at { get; set; }
        public string started_at { get; set; }
        public object acknowledged_at { get; set; }
        public string external_type { get; set; }
        public bool golden { get; set; }
        public object missed { get; set; }
        public object rejected { get; set; }
        public bool tainted { get; set; }
        public string country { get; set; }
        public string region { get; set; }
        public string city { get; set; }
        public int job_id { get; set; }
        public int unit_id { get; set; }
        public int worker_id { get; set; }
        public double trust { get; set; }
        public double worker_trust { get; set; }
        public string unit_state { get; set; }
        public Dictionary<string, string> data { get; set; }
        public UnitData unit_data { get; set; }
    }

    public class UnitData
    {
        public string AudioUrl { get; set; }
        public string AudioTypeCodec { get; set; }
    }

    public class Minimum_Requirements
    {
        public int priority { get; set; }
        public Skill_Scores skill_scores { get; set; }
        public int min_score { get; set; }
    }

    public class Skill_Scores
    {
        public int level_1_contributors { get; set; }
    }

    public class Gold
    {
    }
}