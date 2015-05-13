using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crowd.Service.CrowdFlower
{
    public class CFWebhook
    {
        public string signal { get; set; }
        public Payload payload { get; set; }
        public string signature { get; set; }
    }

    public class Payload
    {
        public int id { get; set; }
        public CFOptions options { get; set; }
        public string title { get; set; }
        public string secret { get; set; }
        public int? project_number { get; set; }
        public string alias { get; set; }
        public int judgments_per_unit { get; set; }
        public int units_per_assignment { get; set; }
        public int pages_per_assignment { get; set; }
        public object max_judgments_per_worker { get; set; }
        public object max_judgments_per_ip { get; set; }
        public int gold_per_assignment { get; set; }
        public int minimum_account_age_seconds { get; set; }
        public string execution_mode { get; set; }
        public int payment_cents { get; set; }
        public bool design_verified { get; set; }
        public bool require_worker_login { get; set; }
        public bool public_data { get; set; }
        public string variable_judgments_mode { get; set; }
        public int max_judgments_per_unit { get; set; }
        public int expected_judgments_per_unit { get; set; }
        public int min_unit_confidence { get; set; }
        public object units_remain_finalized { get; set; }
        public object auto_order_timeout { get; set; }
        public int auto_order_threshold { get; set; }
        public DateTime? completed_at { get; set; }
        public string state { get; set; }
        public bool auto_order { get; set; }
        public string webhook_uri { get; set; }
        public object send_judgments_webhook { get; set; }
        public string language { get; set; }
        public Minimum_Requirements minimum_requirements { get; set; }
        public object desired_requirements { get; set; }
        public bool order_approved { get; set; }
        public int max_work_per_network { get; set; }
        public object copied_from { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public object[] included_countries { get; set; }
        public object[] excluded_countries { get; set; }
        public string instructions { get; set; }
        public string cml { get; set; }
        public string js { get; set; }
        public string css { get; set; }
        public string[] confidence_fields { get; set; }
        public Gold gold { get; set; }
        public int units_count { get; set; }
        public int golds_count { get; set; }
        public int judgments_count { get; set; }
        public string support_email { get; set; }
        public bool worker_ui_remix { get; set; }
        public int crowd_costs { get; set; }
        public bool completed { get; set; }
        public Fields fields { get; set; }
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

    public class Fields
    {
        public Dictionary<string, string> fields { get; set; }
    }
}