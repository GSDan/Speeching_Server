using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.Service.Model.Feedback
{
    public class FeedbackItemGraph : FeedbackItem
    {
        public GraphPoint[] DataPoints;
    }

    public class GraphPoint
    {
        public double YVal;
        public DateTime XVal;
    }
}
