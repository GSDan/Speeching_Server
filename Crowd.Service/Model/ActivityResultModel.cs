using System;
using System.Collections.Generic;
using System.Linq;
using Crowd.Model.Data;

namespace Crowd.Service.Model
{
    public class ActivityResultModel
    {
        public int Id { get; set; }
        public string ResourceUrl { get; set; }
        public string ExternalAccessKey { get; set; }
        public int ParticipantActivityId { get; set; }
        public Dictionary<int, string> ParticipantTaskIdResults { get; set; }

        public static ActivityResultModel Convert(ParticipantResult crowdResult)
        {
            var retResModel = new ActivityResultModel();
            if (crowdResult != null)
            {
                retResModel.Id = crowdResult.Id;
                retResModel.ResourceUrl = crowdResult.ResourceUrl;
                retResModel.ParticipantActivityId = crowdResult.ParticipantActivityId;
                retResModel.ParticipantTaskIdResults = crowdResult.ParticipantTaskIdResults;
                retResModel.ExternalAccessKey = crowdResult.ExternalAccessToken;
            }
            return retResModel;
        }

        public static IEnumerable<ActivityResultModel> Convert(IEnumerable<ParticipantResult> crowdResults)
        {
            var resultModels = new List<ActivityResultModel>();
            if (crowdResults != null && crowdResults.Any())
            {
                foreach (var res in crowdResults)
                {
                    var resModel = Convert(res);
                    if (resModel != null)
                        resultModels.Add(resModel);
                }
            }
            return resultModels;
        }

        public static ParticipantResult ConvertToEntity(ActivityResultModel results)
        {
            var partResult = new ParticipantResult();
            if (results != null)
            {
                partResult.Id = results.Id;
                partResult.ResourceUrl = results.ResourceUrl;

                partResult.ParticipantActivityId = results.ParticipantActivityId;
                partResult.ParticipantTaskIdResults = results.ParticipantTaskIdResults;
                partResult.ExternalAccessToken = results.ExternalAccessKey;
            }
            return partResult;
        }

        public static IEnumerable<ParticipantResult> ConvertToEntity(IEnumerable<ActivityResultModel> results)
        {
            var resultModels = new List<ParticipantResult>();
            if (results != null && results.Any())
            {
                foreach (var res in results)
                {
                    var resModel = ConvertToEntity(res);
                    if (resModel != null)
                        resultModels.Add(resModel);
                }
            }
            return resultModels;
        }
    }
}