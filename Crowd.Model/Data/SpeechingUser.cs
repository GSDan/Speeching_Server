using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.Model.Data
{
    public class SpeechingUser
    {
        public int Id { get; set; }
        public String Username { get; set; }
        public String Email { get; set; }
        public String DisplayName { get; set; }

        public virtual ICollection<ParticipantTask> SpeechingRecordings { get; set; }
    }
}
