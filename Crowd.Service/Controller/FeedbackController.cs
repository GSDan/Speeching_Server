using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Crowd.Model.Data;
using Crowd.Service.Interface;
using Crowd.Service.Model.Feedback;
using Newtonsoft.Json;

namespace Crowd.Service.Controller
{
    public class FeedbackController : BaseController
    {
        public async Task<HttpResponseMessage> Get()
        {
            // get average of all accent ratings
            float rlstaccent = (float) await (from judgement in DB.CrowdJudgements
                from data in judgement.Data
                where data.DataType == "rlstaccent"
                select data.NumResponse).AverageAsync();

            FeedbackItemRating accentFeedback = new FeedbackItemRating
            {
                Rating = rlstaccent,
                Title = "Accent Influence",
                Description = "This rating shows how much your accent affected listeners' understanding of your speech.",
                Date = DateTime.Now,
                Dismissable = false,
                Importance = 10
            };

            float rlsttrans = (float)await (from judgement in DB.CrowdJudgements
                                             from data in judgement.Data
                                            where data.DataType == "rlsttrans"
                                             select data.NumResponse).AverageAsync();

            FeedbackItemRating transFeedback = new FeedbackItemRating
            {
                Rating = rlsttrans,
                Title = "Difficulty of Understanding",
                Description = "This rating shows how difficult listeners find understanding what you say.",
                Date = DateTime.Now,
                Dismissable = false,
                Importance = 10
            };

            IFeedbackItem[] feedback = {transFeedback, accentFeedback};

            return new HttpResponseMessage
            {
                Content = new JsonContent(JsonConvert.SerializeObject(feedback))
            };
        }
    }
}
