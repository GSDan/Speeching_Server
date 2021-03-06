﻿using System.Collections.Generic;

namespace Crowd.Service.CrowdFlower
{
    public class CFWebhook
    {
        public string signature { get; set; }
        public string payload { get; set; }
        public string signal { get; set; }
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
        public AggResult rlsttrans { get; set; }
        public AggResult rlstmp { get; set; }
        public AggResult rlstaccent { get; set; }
    }

    public class AggResult
    {
        public string agg;
        public double confidence;
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
        public string TaskType { get; set; }
        public string ParticipantAssessmentTaskId { get; set; }
        public string ParticipantTaskId { get; set; }
        public string Choices { get; set; }
        public string PrevLoud { get; set; }
        public string PrevPace { get; set; }
        public string PrevPitch { get; set; }
        public string Comparison { get; set; }
        public string ExtraData { get; set; }
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