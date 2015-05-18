using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using Crowd.Model.Data;

namespace Crowd.Service.Model
{
    public class CategoryModel
    {

        public int Id { get; set; }
        public string ExternalId { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public bool Recommended { get; set; }
        public IEnumerable<ActivityModel> Activities { get; set; }

        public static CategoryModel Convert(ParticipantActivityCategory crowdCategory)
        {
            var retCat = new CategoryModel();
            if (crowdCategory != null)
            {
                retCat.Id = crowdCategory.Id;
                retCat.ExternalId = crowdCategory.ExternalId;
                retCat.Title = crowdCategory.Title;
                retCat.Icon = crowdCategory.Icon;
                retCat.Recommended = crowdCategory.Recommended;
                retCat.Activities = ActivityModel.Convert(crowdCategory.Activities);
            }
            return retCat;
        }

        public static IEnumerable<CategoryModel> Convert(IEnumerable<ParticipantActivityCategory> crowdCategories)
        {
            var categoryModels = new List<CategoryModel>();
            if (crowdCategories != null && crowdCategories.Any())
            {
                foreach (var crowdCat in crowdCategories)
                {
                    var retCat = Convert(crowdCat);
                    if (retCat != null)
                        categoryModels.Add(retCat);
                }
            }
            return categoryModels;
        }
    }
}