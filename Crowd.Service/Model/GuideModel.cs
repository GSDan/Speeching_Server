using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crowd.Model.Data;

namespace Crowd.Service.Model
{
    public class GuideModel
    {
        public int Id { get; set; }
        public string MediaLocation { get; set; }
        public string Text { get; set; }
        public int CrowdActivityId { get; set; }

        public static GuideModel Convert(CrowdPage cPage)
        {
            var guide = new GuideModel();
            if (cPage != null)
            {
                guide.Id = cPage.Id;
                guide.MediaLocation = cPage.MediaLocation;
                guide.Text = cPage.Text;
                guide.CrowdActivityId = cPage.CrowdActivityId;
            }
            return guide;
        }

        public static IEnumerable<GuideModel> Convert(IEnumerable<CrowdPage> cPages)
        {
            var retGuides = new List<GuideModel>();
            if (cPages != null && cPages.Any())
            {
                foreach (var crowdTask in cPages)
                {
                    var retTask = Convert(crowdTask);
                    if (retTask != null)
                        retGuides.Add(retTask);
                }
            }
            return retGuides;
        }
    }
}