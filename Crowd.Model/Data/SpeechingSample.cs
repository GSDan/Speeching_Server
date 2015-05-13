using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.Model.Data
{
    public class SpeechingSample
    {
        public int Id { get; set; }
        public String Filename { get; set; }
        public String Description { get; set; }
        public String Con { get; set; }
        public String Active { get; set; }
        public String Truth { get; set; }
        public String Person { get; set; }
    }
}
