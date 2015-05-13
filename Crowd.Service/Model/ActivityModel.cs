using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using Crowd.Model.Data;

namespace Crowd.Service.Model
{
    public class ActivityModel
    {
        public int Id { get; set; }
        public string ExternalId { get; set; }
        public string PrincipleInvestigatorId { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Resource { get; set; }
        public int CrowdCategoryId { get; set; }
        public virtual IEnumerable<TaskModel> Tasks { get; set; }
        public virtual IEnumerable<GuideModel> Guides { get; set; }

        public static ActivityModel Convert(ParticipantActivity crowdActivity)
        {
            var retActModel = new ActivityModel();
            if (crowdActivity != null)
            {
                retActModel.ExternalId = crowdActivity.ExternalId;
                retActModel.Icon = crowdActivity.Icon;
                retActModel.Id = crowdActivity.Id;
                retActModel.Resource = crowdActivity.Resource;
                retActModel.PrincipleInvestigatorId = crowdActivity.PrincipleInvestigatorId;
                retActModel.Title = crowdActivity.Title;
                retActModel.Tasks = TaskModel.Convert(crowdActivity.ParticipantTasks);
                retActModel.Guides = GuideModel.Convert(crowdActivity.CrowdPages);
            }
            return retActModel;
        }

        public static IEnumerable<ActivityModel> Convert(IEnumerable<ParticipantActivity> crowdActivities)
        {
            var activityModels = new List<ActivityModel>();
            if (crowdActivities != null && crowdActivities.Any())
            {
                foreach (var act in crowdActivities)
                {
                    var actModel = Convert(act);
                    if (actModel != null)
                        activityModels.Add(actModel);
                }
            }
            return activityModels;
        }
    }
}