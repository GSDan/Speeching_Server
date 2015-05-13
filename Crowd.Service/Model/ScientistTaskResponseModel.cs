using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crowd.Model.Data;

namespace Crowd.Service.Model
{
    public class ScientistTaskResponseModel
    {
        //public int Id { get; set; }
        public string ExternalId { get; set; }
        public string Type { get; set; }
        public string Prompt { get; set; }
        public int ParticipantTaskId { get; set; }
        
        public static ScientistTaskResponseModel Convert(ScientistTaskResponse scientistTaskResponse)
        {
            var retTask = new ScientistTaskResponseModel();
            if (scientistTaskResponse != null)
            {
                retTask.Type = scientistTaskResponse.Type;
                retTask.ExternalId = scientistTaskResponse.ExternalId;
                //retTask.Id = scientistTaskResponse.Id;
                retTask.Prompt = scientistTaskResponse.Prompt;
                retTask.ParticipantTaskId = scientistTaskResponse.ParticipantTaskId;
            }
            return retTask;
        }

        public static IEnumerable<ScientistTaskResponseModel> Convert(IEnumerable<ScientistTaskResponse> scientistTaskResponse)
        {
            var retLst = new List<ScientistTaskResponseModel>();
            if (scientistTaskResponse != null && scientistTaskResponse.Any())
            {
                foreach (var res in scientistTaskResponse)
                {
                    var actModel = Convert(res);
                    if (actModel != null)
                        retLst.Add(actModel);
                }
            }
            return retLst;
        }
    }
}